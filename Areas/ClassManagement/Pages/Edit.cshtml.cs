using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetterKnowledgeAssessment.Areas.ClassManagement.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IClassListHandler _classListHandler;

        public EditModel(IClassListHandler classListHandler)
        {
            _classListHandler = classListHandler;
        }

        public IActionResult OnPost(string classListId, [Bind("Grade")] ClassList classList)
        {
            var editTarget = _classListHandler.ClassListById(classListId);
            editTarget.Grade = classList.Grade ?? editTarget.Grade;
            _classListHandler.UpdateClassList(editTarget);
            return RedirectToPage("/Index");
        }
    }
}
