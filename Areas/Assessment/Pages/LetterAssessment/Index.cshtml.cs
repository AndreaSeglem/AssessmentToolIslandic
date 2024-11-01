using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LetterKnowledgeAssessment.Areas.Assessment.Pages.LetterAssessment
{
    [Authorize]
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
            public bool IsUpperCase { get; set; }
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
                return Redirect("/Index?returnUrl=lettertest");
            }
            
            Pupils = _pupilHandler.PupilsByClassListId(HttpContext.Session.GetString("ClassListId"));
            PupilsSerialized = JsonConvert.SerializeObject(Pupils, _serializerSettings);
            ReturnUrl = "/Assessment/LetterAssessment/Index";
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("LetterAssessment", new { PupilId = Input.PupilId, IsUpperCase = Input.IsUpperCase });
        }
    }
}
