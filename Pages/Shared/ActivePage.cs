using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LetterKnowledgeAssessment.Pages.Shared
{
    public static class ActivePage
    {
        private static string AppendCultureParam(string url)
        {
            var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            
            // If the URL already contains a culture parameter, replace it.
            if (url.Contains("culture="))
            {
                return Regex.Replace(url, "culture=[a-zA-Z]{2}", $"culture={currentCulture}");
            }
            else
            {
                // Append the culture parameter
                return $"{url}?culture={currentCulture}";
            }
        }

        public static string Index => AppendCultureParam("/Index");
        public static string Pupils => AppendCultureParam("/Overview/Pupils");
        public static string LetterTest => AppendCultureParam("/Assessment/LetterAssessment/Index");
        public static string ReadingTest => AppendCultureParam("/Assessment/ReadingAssessment/Index");
        public static string Help => AppendCultureParam("/Help");
        public static string Profile => AppendCultureParam("/Account/Manage/Index");
        
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string PupilsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Pupils);

        public static string LetterTestNavClass(ViewContext viewContext) => PageNavClass(viewContext, LetterTest);
        public static string ReadingTestNavClass(ViewContext viewContext) => PageNavClass(viewContext, ReadingTest);
        public static string HelpNavClass(ViewContext viewContext) => PageNavClass(viewContext, Help);
        public static string ProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.HttpContext.Request.Path.Value;

            // Remove query-parameters from `page` if existing
            var formattedPage = page.Contains('?') ? page.Split('?')[0] : page;

            // Compare paths
            return string.Equals(activePage, formattedPage, StringComparison.OrdinalIgnoreCase) ? "active" : "";
        }
    }
}
