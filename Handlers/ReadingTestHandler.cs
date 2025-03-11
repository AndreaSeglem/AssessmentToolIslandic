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
        private readonly IPupilRepository _pupilRepository; // Hent elevdata fra databasen
        private readonly ApplicationDbContext _context;


        // Flyttet ordlisten hit for å unngå duplisering
        private readonly List<string> _norwegianWordList = new List<string> { "sol", "is", "mamma", "hus", "øks", "bil", "gut", "sy", "lese", "rose", "nisse" };
        private readonly List<string> _icelandicWordList = new List<string> { "sól", "ís", "mamma", "hús", "öxi", "bíll", "strákur", "sauma", "lesa", "rós", "jólasveinn" };

        public ReadingTestHandler(IReadingTestRepository readingTestRepository, IPupilRepository pupilRepository, ApplicationDbContext context)
        {
            _readingTestRepository = readingTestRepository;
            _pupilRepository = pupilRepository; // Brukes for å hente elev
            _context = context;
        }

        public void StartTest(string pupilId)
        {
            // Henter elev fra databasen for å sikre at referansen er korrekt
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
                Pupil = pupil, // Bruker den faktiske Pupil-referansen
                WordResults = new List<ReadingTestWord>()
            };

            // Oppretter testordene fra _wordList
            foreach (var word in GetLocalizedWordList())
            {
                test.WordResults.Add(new ReadingTestWord
                {
                    Id = Guid.NewGuid(),
                    Word = word,
                    IsReadCorrectly = false
                });
            }

            // Lagre testen i databasen
            _readingTestRepository.SaveTest(test);
        }

        // Metode for å hente riktig ordliste basert på valgt språk
        private List<string> GetLocalizedWordList()
        {
            string culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            return culture == "is" ? _icelandicWordList : _norwegianWordList;
        }

        // Metode for å hente ordlisten
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


