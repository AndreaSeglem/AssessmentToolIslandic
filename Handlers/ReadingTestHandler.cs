using LetterKnowledgeAssessment.Data;
using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using System.Globalization; 
using System;
using System.Collections.Generic;

namespace LetterKnowledgeAssessment.Handlers
{
    public class ReadingTestHandler : IReadingTestHandler
    {
        private readonly IReadingTestRepository _readingTestRepository;
        private readonly IPupilRepository _pupilRepository;
        private readonly ApplicationDbContext _context;


        private readonly List<string> _norwegianWordList = new List<string> { "sol", "is", "mamma", "hus", "øks", "bil", "gut", "sy", "lese", "rose", "nisse" };
        private readonly List<string> _icelandicWordList = new List<string> { "sól", "ís", "mamma", "hús", "mál", "moli", "mús", "röð", "lesa", "rós", "kisa", "síli" };

        public ReadingTestHandler(IReadingTestRepository readingTestRepository, IPupilRepository pupilRepository, ApplicationDbContext context)
        {
            _readingTestRepository = readingTestRepository;
            _pupilRepository = pupilRepository;
            _context = context;
        }

        public void StartTest(string pupilId)
        {
            var pupil = _pupilRepository.GetPupilById(pupilId);
            if (pupil == null)
            {
                throw new Exception($"Fant ikke elev med ID {pupilId}");
            }

            var test = new ReadingTest
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                IsTestPassed = false,
                Pupil = pupil,
                WordResults = new List<ReadingTestWord>()
            };

            foreach (var word in GetLocalizedWordList())
            {
                test.WordResults.Add(new ReadingTestWord
                {
                    Id = Guid.NewGuid(),
                    Word = word,
                    IsReadCorrectly = false
                });
            }

            // Save test in database
            _readingTestRepository.SaveTest(test);
        }

        // Get correct language word list
        private List<string> GetLocalizedWordList()
        {
            string culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            return culture == "is" ? _icelandicWordList : _norwegianWordList;
        }

        public List<string> GetWordList()
        {
            return GetLocalizedWordList();
        }

        public ReadingTest? GetLatestTestByPupilId(string pupilId)
        {
            return _context.ReadingTests
                .Where(rt => rt.PupilId.ToString() == pupilId)
                .OrderByDescending(rt => rt.Date)
                .FirstOrDefault();
        }

    }
}


