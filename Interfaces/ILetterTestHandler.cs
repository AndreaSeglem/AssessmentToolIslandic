using LetterKnowledgeAssessment.Models;

namespace LetterKnowledgeAssessment.Interfaces
{
    public interface ILetterTestHandler
    {
        List<LetterSoundKnowledgeTestResult> TestResultsByPupilId(string pupilId);
        LetterSoundKnowledgeTestResult TestResultByTestId(string testId);
        Dictionary<string, double> CalculateAverageOfLatestTests(List<Pupil> pupils);
        LetterSoundKnowledgeTestResult AddTestResult(LetterSoundKnowledgeTestResult testResult, Pupil pupil);
        LetterSoundKnowledgeTestResult UpdateTestResult(LetterSoundKnowledgeTestResult testResult);
        Dictionary<string, double> SoundKnowledgeAveragesBasedOnTime(int currentMonth);
    }
}
