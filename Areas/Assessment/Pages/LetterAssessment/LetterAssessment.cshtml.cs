using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using LetterKnowledgeAssessment.Models.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Threading;

namespace LetterKnowledgeAssessment.Areas.Assessment.Pages.LetterAssessment
{
    public class LetterAssessmentModel : PageModel
    {
        private readonly IPupilHandler _pupilHandler;
        private readonly ILetterTestHandler _letterTestHandler;

        public LetterAssessmentModel(IPupilHandler pupilHandler, ILetterTestHandler letterTestHandler)
        {
             _pupilHandler = pupilHandler;
            _letterTestHandler = letterTestHandler;
        }

        public List<string> testLetters { get; set; }

        public Pupil Pupil { get; set; }
        public bool UpperCaseSelected { get; set; }

        public IActionResult OnGet(string pupilId, bool isUpperCase, string? culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }
            Pupil = _pupilHandler.GetPupilById(pupilId);
            if (Pupil == null)
            {
                return NotFound();
            }

            UpperCaseSelected = isUpperCase;
            testLetters = GetTestLetters(UpperCaseSelected);
            return Page();
        }

        public IActionResult OnPost(string pupilId, List<LetterModel> testLetters, bool isUpperCase )
        {
            var pupil = _pupilHandler.GetPupilById(pupilId);
            if (pupil == null)
            {
                return NotFound();
            }
            if (testLetters.Count == 0)
            {
                return NotFound();
            }

            var testKnowledgeResult = new List<LetterSoundKnowledge>();
            foreach (var letter in testLetters)
            {
                testKnowledgeResult.Add(new LetterSoundKnowledge { Id = Guid.NewGuid(),Letter = letter.Letter, KnowledgeLevel = letter.KnowledgeLevel });
            }

            var testResult = new LetterSoundKnowledgeTestResult { Id = Guid.NewGuid(), IsUpperCase = isUpperCase, Date = DateTime.Now, LetterTestResult = testKnowledgeResult};
            _letterTestHandler.AddTestResult(testResult, pupil);

            // Hent gjeldende kultur
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            
            return LocalRedirect($"~/Overview/DetailedOverview?pupilId={pupil.PupilId}&culture={currentCulture}");
        }


        private List<string> GetTestLetters(bool upperCaseSelected)
        {
            var culture = Thread.CurrentThread.CurrentCulture.Name;

            // Adjust culture check to include both "is" and "is-IS"
            var isIcelandic = culture.StartsWith("is");

            var lowerCaseNorwegian = new List<string>
            {
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "æ", "ø", "å"
            };
            var upperCaseNorwegian = new List<string>
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Æ", "Ø", "Å"
            };

            var lowerCaseIcelandic = new List<string>
            {
                "a", "á", "b", "d", "ð", "e", "é", "f", "g", "h", "i", "í", "j", "k", "l", "m", "n", "o", "ó", "p", "r", "s", "t", "u", "ú", "v", "x", "y", "ý", "þ", "æ", "ö"
            };
            var upperCaseIcelandic = new List<string>
            {
                "A", "Á", "B", "D", "Ð", "E", "É", "F", "G", "H", "I", "Í", "J", "K", "L", "M", "N", "O", "Ó", "P", "R", "S", "T", "U", "Ú", "V", "X", "Y", "Ý", "Þ", "Æ", "Ö"
            };

            // Use Icelandic alphabet if culture starts with "is"
            var lowerCase = isIcelandic ? lowerCaseIcelandic : lowerCaseNorwegian;
            var upperCase = isIcelandic ? upperCaseIcelandic : upperCaseNorwegian;

            return upperCaseSelected ? upperCase.Shuffle().ToList() : lowerCase.Shuffle().ToList();
        }
    }
}
