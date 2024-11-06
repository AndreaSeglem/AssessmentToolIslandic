using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace LetterKnowledgeAssessment.Controllers
{
    public class LanguageController : Controller
    {
        /* [HttpGet]
        public IActionResult SetLanguage(string culture, string returnUrl = "/")
        {
            // Store the selected culture in a cookie
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    } */
    
    [HttpGet]
    public IActionResult SetLanguage(string culture)
    {
        if (!string.IsNullOrEmpty(culture))
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
        }

        return RedirectToAction("Index", "Home");
    }
}
}
