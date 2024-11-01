using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LetterKnowledgeAssessment.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IClassListHandler _classListHandler;
        private readonly UserManager<Teacher> _userManager;
        public IndexModel(ILogger<IndexModel> logger, IClassListHandler classListHandler, UserManager<Teacher> userManager)
        {
            _logger = logger;
            _classListHandler = classListHandler;
            _userManager = userManager;
        }

        public List<ClassList> ClassLists { get; set; }
        public string ReturnUrl { get; set; }
        public StatusMessage StatusMessage { get; set; }

        public async Task OnGet(string? returnUrl)
        {
            ReturnUrl = "/Overview/Pupils";
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (returnUrl.Equals("lettertest"))
                {
                    ReturnUrl = "/Assessment/LetterAssessment/Index";
                    StatusMessage = new StatusMessage { Error = true, Message = "Du må velge en klasse først!" };
                }
                if (returnUrl.Equals("pupils"))
                {
                    StatusMessage = new StatusMessage { Error = true, Message = "Du må velge en klasse først!" };
                }
            }
            
            var teacher = await _userManager.GetUserAsync(User);
            ClassLists = _classListHandler.ClassListsByTeacher(teacher);
        }

        public IActionResult OnPost(string classListId, string returnUrl)
        {
            HttpContext.Session.SetString("ClassListId", classListId);
            return Redirect(returnUrl);
        }
    }
}