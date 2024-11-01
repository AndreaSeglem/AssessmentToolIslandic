using Microsoft.AspNetCore.Mvc.Rendering;

namespace LetterKnowledgeAssessment.Pages.Shared
{
    public static class ActivePage
    {
        public static string Index => "/Index";
        public static string Pupils => "/Pupils";
        public static string LetterTest => "/LetterAssessment/Index";
        public static string Help => "/Help";
        public static string Profile => "/Account/Manage/Index";
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string PupilsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Pupils);

        public static string LetterTestNavClass(ViewContext viewContext) => PageNavClass(viewContext, LetterTest);
        public static string HelpNavClass(ViewContext viewContext) => PageNavClass(viewContext, Help);
        public static string ProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["SidebarActive"] as string ?? viewContext.ActionDescriptor.DisplayName;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : "";
        }
    }
}
