using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetterKnowledgeAssessment.Areas.ClassManagement.Pages.Pupils
{
    [Authorize]
    public class AddPupilsModel : PageModel
    {
        private readonly IClassListHandler _classListHandler;
        private readonly IPupilHandler _pupilHandler;

        public AddPupilsModel(IClassListHandler classListHandler, IPupilHandler pupilHandler)
        {
            _classListHandler = classListHandler;
            _pupilHandler = pupilHandler;
        }

        public IActionResult OnPost(List<Pupil> pupils, string returnUrl)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("ClassListId")))
            {
                return Redirect("~/Index");
            }
            var classList = _classListHandler.ClassListById(HttpContext.Session.GetString("ClassListId"));
            _pupilHandler.AddPupilList(pupils, classList);
            return Redirect(returnUrl);
        }

    }
}
