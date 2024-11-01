using LetterKnowledgeAssessment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetterKnowledgeAssessment.Areas.ClassManagement.Pages
{
    [Authorize]
    public class RemoveModel : PageModel
    {
        private readonly IClassListHandler _classListHandler;

        public RemoveModel(IClassListHandler classListHandler)
        {
            _classListHandler = classListHandler;
        }

        public IActionResult OnPost(string classListId)
        {
            var classList = _classListHandler.ClassListById(classListId);
            _classListHandler.RemoveClassList(classList);
            HttpContext.Session.Remove("ClassListId");
            return RedirectToPage("/Index");
        }
    }
}
