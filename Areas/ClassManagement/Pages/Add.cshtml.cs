using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using LetterKnowledgeAssessment.Interfaces;

namespace LetterKnowledgeAssessment.Areas.ClassManagement.Pages
{
    [Authorize]
    public class AddModel : PageModel
    {
        private readonly IClassListHandler _classListHandler;
        private readonly UserManager<Teacher> _userManager;
        public AddModel(IClassListHandler classListHandler, UserManager<Teacher> userManager)
        {
            _classListHandler = classListHandler;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPostAsync(string grade)
        {
            var teacher = await _userManager.GetUserAsync(User);
            if (!string.IsNullOrEmpty(grade))
            {
                var classList = new ClassList
                {
                    Id = Guid.NewGuid(),
                    Grade = grade
                };
                _classListHandler.AddClassList(classList, teacher);
                HttpContext.Session.SetString("ClassListId", classList.Id.ToString());
                return Redirect("/Overview/Pupils");
            }
            return Page();
        }
    }
}
