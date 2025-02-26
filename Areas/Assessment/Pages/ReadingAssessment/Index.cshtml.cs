using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using LetterKnowledgeAssessment.Models;
using LetterKnowledgeAssessment.Interfaces;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentToolIslandic.Areas.Assessment.Pages.ReadingAssessment
{
    public class IndexModel : PageModel
    {
        private readonly IPupilHandler _pupilHandler;
        private readonly JsonSerializerSettings _serializerSettings;

        public List<Pupil> Pupils { get; set; }
        public string PupilsSerialized { get; set; }
        public string ReturnUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string PupilId { get; set; }
        }

        public IndexModel(IPupilHandler pupilHandler)
        {
            _pupilHandler = pupilHandler;
            _serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
            };
        }

        public async Task<IActionResult> OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("ClassListId")))
        {
            return Redirect("/Index?returnUrl=readingtest");
        }
        
        Pupils = _pupilHandler.PupilsByClassListId(HttpContext.Session.GetString("ClassListId"));
        PupilsSerialized = JsonConvert.SerializeObject(Pupils, _serializerSettings);
        ReturnUrl = "/Assessment/ReadingAssessment/Index";
        return Page();

        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Test", new { PupilId = Input.PupilId });
        }
    }
}
