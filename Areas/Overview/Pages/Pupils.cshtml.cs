using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LetterKnowledgeAssessment.Areas.Overview.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IPupilHandler _pupilHandler;
        private readonly IClassListHandler _classListHandler;
        private readonly ILetterTestHandler _letterTestHandler;
        private readonly JsonSerializerSettings _serializerSettings;

        public IndexModel(IPupilHandler pupilHandler, IClassListHandler classListHandler, ILetterTestHandler letterTestHandler)
        {
            _pupilHandler = pupilHandler;
            _classListHandler = classListHandler;
            _letterTestHandler = letterTestHandler;
            _serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,

            };
        }
      
        public ClassList ClassList { get; set; }
  
        public IEnumerable<Pupil> AllPupils { get; set; }
        public string PupilsSerialized { get; set; }
        public Dictionary<string, double> AverageLetterKnowledge { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            var classListId = HttpContext.Session.GetString("ClassListId");
            if (string.IsNullOrEmpty(classListId))
            {
                return Redirect("~/Index?returnUrl=pupils");
            }
            ClassList = _classListHandler.ClassListById(classListId);
            AllPupils = _pupilHandler.PupilsByClassListId(classListId);
            PupilsSerialized = JsonConvert.SerializeObject(AllPupils, _serializerSettings);
            var testedPupils = AllPupils.Where(p => p.LetterSoundKnowledgeTestResults.Count > 0).ToList();
            AverageLetterKnowledge = _letterTestHandler.CalculateAverageOfLatestTests(testedPupils);
            ReturnUrl = "/Overview/Pupils";
            return Page();
        }
    }
}

