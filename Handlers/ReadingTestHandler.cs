using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using System;
using System.Collections.Generic;

namespace LetterKnowledgeAssessment.Handlers
{
    public class ReadingTestHandler : IReadingTestHandler
    {
        private readonly IReadingTestRepository _readingTestRepository;
        private readonly IPupilRepository _pupilRepository; // Hent elevdata fra databasen

        // Flyttet ordlisten hit for å unngå duplisering
        private readonly List<string> _wordList = new List<string> { "sol", "is", "mamma", "hus", "øks", "bil", "gut", "sy", "lese", "rose", "nisse" };

        public ReadingTestHandler(IReadingTestRepository readingTestRepository, IPupilRepository pupilRepository)
        {
            _readingTestRepository = readingTestRepository;
            _pupilRepository = pupilRepository; // Brukes for å hente elev
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
            foreach (var word in _wordList)
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

        // Metode for å hente ordlisten dersom vi trenger den i `ReadingAssessment.cshtml.cs`
        public List<string> GetWordList()
        {
            return _wordList;
        }
    }
}


