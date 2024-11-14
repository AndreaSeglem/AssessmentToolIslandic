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
    
    /*[HttpGet]
    public IActionResult SetLanguage(string culture, string returnUrl = "/")
    {
        if (!string.IsNullOrEmpty(culture))
        {
            // Clear any existing culture cookie
            Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
            );

            // Optional: log or debug output
                Console.WriteLine($"Culture set to {culture}, returning to {returnUrl}");
        }

         // Redirect to the previous page (referer URL) if available, else to Home
        //var refererUrl = Request.Headers["Referer"].ToString();
        //if (!string.IsNullOrEmpty(refererUrl))
        //{
            //return Redirect(refererUrl);
        //}

        //return RedirectToAction("Index", "Home");

        // Redirect back to the page specified in returnUrl, or home page if null
            return LocalRedirect(returnUrl);
    }*/
    [HttpGet]
public IActionResult SetLanguage(string culture, string returnUrl = "/")
{
    Console.WriteLine($"SetLanguage called with culture: {culture}, returnUrl: {returnUrl}");
     // Set the culture in the response cookie
    if (!string.IsNullOrEmpty(culture))
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
        );
    }

    // Redirect back to the page specified in returnUrl
    return LocalRedirect(returnUrl);
}

}
}
