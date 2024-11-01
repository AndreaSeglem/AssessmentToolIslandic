using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using LetterKnowledgeAssessment.Models.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public IActionResult OnGet(string pupilId, bool isUpperCase)
        {
            Pupil = _pupilHandler.GetPupilById(pupilId);
            if (Pupil == null)
            {
                return NotFound();
            }
            UpperCaseSelected = isUpperCase;
            testLetters = GetTestLetters(UpperCaseSelected);
            

            return Page();
        }

        public IActionResult OnPost(string pupilId, List<LetterModel> testLetters, bool isUpperCase)
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
            
            return LocalRedirect($"~/Overview/DetailedOverview?pupilId={pupil.PupilId}");
        }


        private List<string> GetTestLetters(bool upperCaseSelected)
        {
            var lowerCase = new List<string>
            {
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "æ", "ø", "å"
            };
            var upperCase = new List<string> 
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Æ", "Ø", "Å" 
            };

            var testLetters = upperCaseSelected ? upperCase.Shuffle().ToList() : lowerCase.Shuffle().ToList();

            return testLetters;
        }
    }
}
