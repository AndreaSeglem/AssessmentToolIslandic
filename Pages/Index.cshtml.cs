using System.Globalization;
using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.WebUtilities;


namespace LetterKnowledgeAssessment.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IClassListHandler _classListHandler;
        private readonly UserManager<Teacher> _userManager;

        private readonly IStringLocalizer _localizer;


        public IndexModel(ILogger<IndexModel> logger, IClassListHandler classListHandler, UserManager<Teacher> userManager, IStringLocalizerFactory localizerFactory)
        {
            _logger = logger;
            _classListHandler = classListHandler;
            _userManager = userManager;
            _localizer = localizerFactory.Create("Strings", "LetterKnowledgeAssessment");
        }

        public List<ClassList> ClassLists { get; set; }
        public string ReturnUrl { get; set; }
        public StatusMessage StatusMessage { get; set; }

        public async Task OnGet(string? returnUrl)
        {
            var culture = Request.Query["culture"].FirstOrDefault() ?? 
                  Request.Cookies[CookieRequestCultureProvider.DefaultCookieName]?.Split('|')[0].Split('=')[1] ?? 
                  CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            ReturnUrl = $"/Overview/Pupils?culture={culture}";
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (returnUrl.Equals("lettertest"))
                {
                    ReturnUrl = $"/Assessment/LetterAssessment/Index?culture={culture}";
                    StatusMessage = new StatusMessage { Error = true, Message = @_localizer["ChooseClass"] };
                }
                if (returnUrl.Equals("pupils"))
                {
                    StatusMessage = new StatusMessage { Error = true, Message = @_localizer["ChooseClass"] };
                }
            }
            
            var teacher = await _userManager.GetUserAsync(User);
            ClassLists = _classListHandler.ClassListsByTeacher(teacher);
        }

        public IActionResult OnPost(string classListId, string returnUrl, string culture)
        {
            HttpContext.Session.SetString("ClassListId", classListId);
            // Check if the culture is passed, if not, default to the current culture
            var currentCulture = !string.IsNullOrEmpty(culture) ? culture : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            // Set the culture cookie
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(currentCulture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Include culture in `returnUrl`
            if (!returnUrl.Contains("culture="))
            {
                returnUrl = QueryHelpers.AddQueryString(returnUrl, "culture", currentCulture);
            }
            return Redirect(returnUrl);
        }
     }
}