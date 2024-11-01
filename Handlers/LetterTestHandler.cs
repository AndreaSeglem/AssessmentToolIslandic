using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace LetterKnowledgeAssessment.Handlers
{
    public class LetterTestHandler : ILetterTestHandler
    {
        private readonly ILetterTestRepository _letterTestRepository;
        private readonly IPupilHandler _pupilHandler;

        public LetterTestHandler(ILetterTestRepository letterTestRepository, IPupilHandler pupilHandler)
        {
            _letterTestRepository = letterTestRepository;
            _pupilHandler = pupilHandler;
        }

        public List<LetterSoundKnowledgeTestResult> TestResultsByPupilId(string pupilId)
        { 
            return _letterTestRepository.TestResultsByPupilId(pupilId).ToList();
        }

        public LetterSoundKnowledgeTestResult TestResultByTestId(string testId)
        {
            return _letterTestRepository.TestResultById(testId);
        }

        public Dictionary<string, double> CalculateAverageOfLatestTests(List<Pupil> pupils)
        {
            if(pupils.Count == 0)
            {
                return new Dictionary<string, double> { { "UpperCase", 0 }, { "LowerCase", 0 } };
            }
            var testResultsUpper = 0;
            var countUpper = 0;
            var testResultsLower = 0;
            var countLower = 0;
            double averageUpper = 0;
            double averageLower = 0;

            foreach (var pupil in pupils)
            {
                var upperResult = _letterTestRepository.TestResultsByPupilId(pupil.PupilId.ToString()).Where(t => t.IsUpperCase.Equals(true)).FirstOrDefault();
                var lowerResult = _letterTestRepository.TestResultsByPupilId(pupil.PupilId.ToString()).Where(t => t.IsUpperCase.Equals(false)).FirstOrDefault();
                if (upperResult != null)
                {
                    testResultsUpper += upperResult.LetterTestResult.Count(l => l.KnowledgeLevel.Equals(LetterKnowledgeLevel.NameSound) || l.KnowledgeLevel.Equals(LetterKnowledgeLevel.Sound));
                    countUpper++;
                }
                if (lowerResult != null)
                {
                    testResultsLower += lowerResult.LetterTestResult.Count(l => l.KnowledgeLevel.Equals(LetterKnowledgeLevel.NameSound) || l.KnowledgeLevel.Equals(LetterKnowledgeLevel.Sound));
                    countLower++; 
                }
            }

            if (countUpper != 0)
            {
                averageUpper = (double)testResultsUpper / countUpper;
            }
            if (countLower != 0)
            {
                averageLower = (double)testResultsLower / countLower;
            }
            return new Dictionary<string, double> { { "UpperCase", averageUpper}, { "LowerCase", averageLower} };
        }

        public LetterSoundKnowledgeTestResult AddTestResult(LetterSoundKnowledgeTestResult testResult, Pupil pupil)
        {
            testResult.Pupil = pupil;
            try
            {
                _letterTestRepository.AddTestResult(testResult);
            }
            catch (DbUpdateException)
            {
                return null;
            }
            if(pupil.LetterSoundKnowledgeTestResults == null)
            {
                pupil.LetterSoundKnowledgeTestResults = new List<LetterSoundKnowledgeTestResult>();
            }
            pupil.LetterSoundKnowledgeTestResults.Add(testResult);

            pupil.KnowledgeLevel = CalculateKnowledgeLevel(pupil.LetterSoundKnowledgeTestResults.ToList(), testResult.Date.Month);
            _pupilHandler.UpdatePupil(pupil);

            return testResult;
        }

        public LetterSoundKnowledgeTestResult UpdateTestResult(LetterSoundKnowledgeTestResult testResult)
        {
            try
            {
                _letterTestRepository.UpdateTestResult(testResult);
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return testResult;
        }

        /// <summary>
        /// Uses a function for cubic regression based on the results found in Sigmundsson et al (2018) "Gender Gaps in Letter-Sound Knowledge Persist Across the First School Year"
        /// (https://www.frontiersin.org/articles/10.3389/fpsyg.2018.00301/full)
        /// where start of school is august (0)
        /// and end of school year is june (10).
        /// </summary>
        /// <returns>Average letter sound knowledge for both upper case and lower case.</returns>
        public Dictionary<string, double> SoundKnowledgeAveragesBasedOnTime(int currentMonth)
        {
            var result = new Dictionary<string, double>();
            int timeStep;
            if (currentMonth < 8)
            {
                if (currentMonth > 6)
                {
                    currentMonth = 6;
                }
                timeStep = currentMonth + 4;
            }
            else
            {
                timeStep = currentMonth - 8;
            }
            result.Add("lowerCaseAvg", -0.029 * Math.Pow(timeStep, 2) + 1.636 * timeStep + 7.24);
            result.Add("upperCaseAvg", -0.0737 * Math.Pow(timeStep, 2) + (2.1655 * timeStep) + 9.585);
            return result;

        }
        private int CalculateKnowledgeLevel(List<LetterSoundKnowledgeTestResult> testResults, int month)
        {
            var averages = SoundKnowledgeAveragesBasedOnTime(month);
            var latestUpper = testResults.Where(t => t.IsUpperCase).OrderByDescending(t => t.Date).FirstOrDefault();
            var latestLower = testResults.Where(t => !t.IsUpperCase).OrderByDescending(t => t.Date).FirstOrDefault();
            if(latestUpper == null|| latestLower == null)
            {
                return 0;
            }

            var resultUpper = _letterTestRepository.TestResultById(latestUpper.Id.ToString());
            var resultLower = _letterTestRepository.TestResultById(latestLower.Id.ToString());
            var soundCountUpperCase = latestUpper.LetterTestResult.Count(l => l.KnowledgeLevel.Equals(LetterKnowledgeLevel.Sound) || l.KnowledgeLevel.Equals(LetterKnowledgeLevel.NameSound));
            var soundCountLowerCase = latestLower.LetterTestResult.Count(l => l.KnowledgeLevel.Equals(LetterKnowledgeLevel.Sound) || l.KnowledgeLevel.Equals(LetterKnowledgeLevel.NameSound));
            var standardDeviationApproximation = 8.5375;


            if (soundCountUpperCase >= averages["upperCaseAvg"] && soundCountLowerCase >= averages["lowerCaseAvg"])
            {
                return 3;
            }
            if (soundCountUpperCase >= averages["upperCaseAvg"]-standardDeviationApproximation && soundCountLowerCase >= averages["lowerCaseAvg"] - standardDeviationApproximation)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

    }
}
