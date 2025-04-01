using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;


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
                HttpContext.Session.SetString("culture", culture);
                
                Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
                );
            }
             // Decode returnUrl to clean up double-encoding
            returnUrl = HttpUtility.UrlDecode(returnUrl);

            // Remove any existing culture parameters
            if (returnUrl.Contains("culture="))
            {
                var uri = new UriBuilder(Request.Scheme + "://" + Request.Host + returnUrl);
                var query = QueryHelpers.ParseQuery(uri.Query)
                    .Where(kvp => kvp.Key != "culture")
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

                uri.Query = QueryHelpers.AddQueryString("", query);
                returnUrl = uri.Path + uri.Query;
            }

            // Add the new culture
            returnUrl = QueryHelpers.AddQueryString(returnUrl, "culture", culture);

            return LocalRedirect(returnUrl);
        }

}
}
