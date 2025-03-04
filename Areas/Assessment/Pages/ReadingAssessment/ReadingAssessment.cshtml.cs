using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace AssessmentToolIslandic.Areas.Assessment.Pages.ReadingAssessment
{
    public class ReadingAssessmentModel : PageModel
    {
        private readonly IReadingTestHandler _readingTestHandler;
        public List<string> WordList { get; private set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string PupilId { get; set; }
            public bool IsTestPassed { get; set; }
            public Dictionary<string, bool> WordResults { get; set; }
        }

        public ReadingAssessmentModel(IReadingTestHandler readingTestHandler)
        {
            _readingTestHandler = readingTestHandler;
        }

        public void OnGet(string pupilId)
        {
            WordList = _readingTestHandler.GetWordList(); // Henter ordlisten fra handleren
        }
    }
}
