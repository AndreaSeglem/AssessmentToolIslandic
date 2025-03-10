using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using LetterKnowledgeAssessment.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace AssessmentToolIslandic.Areas.Assessment.Pages.ReadingAssessment
{
    public class ReadingAssessmentModel : PageModel
    {
        private readonly IReadingTestHandler _readingTestHandler;
        private readonly IReadingTestRepository _readingTestRepository;
        private readonly IPupilRepository _pupilRepository;
        public List<string> WordList { get; private set; }
        public Pupil Pupil { get; set; } // ✅ Definer en instans av Pupil


        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string PupilId { get; set; }
            public bool IsTestPassed { get; set; }
            public Dictionary<string, bool> WordResults { get; set; }
        }

        public ReadingAssessmentModel(IReadingTestHandler readingTestHandler, IReadingTestRepository readingTestRepository, IPupilRepository pupilRepository)
        {
            _readingTestHandler = readingTestHandler;
            _readingTestRepository = readingTestRepository;
            _pupilRepository = pupilRepository;
        }

        public void OnGet(string pupilId)
        {
            if (string.IsNullOrEmpty(pupilId))
            {
                RedirectToPage("/Error"); // Hvis ingen elev er valgt, send til feilmelding
                return;
            }
            Pupil = _pupilRepository.GetPupilById(pupilId); // Hent eleven basert på ID
            if (Pupil == null)
            {
                throw new Exception("Pupil not found in database.");
            }
            WordList = _readingTestHandler.GetWordList(); // Hent ordlisten
            if (WordList == null || !WordList.Any())
            {
                Console.WriteLine("❌ Feil: Ingen ord funnet for testen!");
            }
            else
            {
                Console.WriteLine("✅ Antall ord lastet inn: " + WordList.Count);
            }

            // Start en ny test i databasen. OBS! tror det kan være denne som gjør at testen lagres før den er gjennomført?
            _readingTestHandler.StartTest(pupilId);
        }

        public IActionResult OnPostSubmitTest([FromBody] InputModel input)
        {
            Console.WriteLine("🔵 OnPostSubmitTest() ble kalt!");

            if (input == null)
            {
                Console.WriteLine("❌ Input er null!");
                return BadRequest("Ugyldige data sendt (Input er null).");
            }
            if (string.IsNullOrEmpty(input.PupilId))
            {
                Console.WriteLine("❌ PupilId er tom eller null!");
                return BadRequest("Ugyldige data sendt (PupilId er tom).");
            }
            if (input.WordResults == null)
            {
                Console.WriteLine("❌ WordResults er null!");
                return BadRequest("Ugyldige data sendt (WordResults er null).");
            }

            Console.WriteLine($"✅ Mottatt data: PupilId={input.PupilId}, IsTestPassed={input.IsTestPassed}, WordResults Count={input.WordResults.Count}");

            foreach (var word in input.WordResults)
            {
                Console.WriteLine($"📝 Ord: {word.Key}, Korrekt lest: {word.Value}");
            }

            // Opprett testresultat basert på input
            var test = new ReadingTest
            {
                Id = Guid.NewGuid(),
                PupilId = Guid.Parse(input.PupilId),
                Date = DateTime.UtcNow,
                IsTestPassed = input.IsTestPassed,
                WordResults = new List<ReadingTestWord>()
            };

            foreach (var word in input.WordResults)
            {
                Console.WriteLine($"📌 Lagrer ord: {word.Key} - {word.Value}");

                test.WordResults.Add(new ReadingTestWord
                {
                    Id = Guid.NewGuid(),
                    Word = word.Key,
                    IsReadCorrectly = word.Value
                });
            }

            _readingTestRepository.SaveTest(test);

            // Hent gjeldende kultur
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

            return LocalRedirect($"~/Overview/DetailedOverview?pupilId={input.PupilId}&culture={currentCulture}");
        }

    }
}
