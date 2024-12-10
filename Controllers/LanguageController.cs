using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace LetterKnowledgeAssessment.Controllers
{
    public class LanguageController : Controller
    {
        [HttpGet]
        public IActionResult SetLanguage(string culture, string returnUrl = "/")
        {
            Console.WriteLine($"SetLanguage called with culture: {culture}, returnUrl: {returnUrl}");
            // Set the culture
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
                );
            }
            return LocalRedirect(returnUrl);
        }

}
}
