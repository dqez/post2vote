using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace votegdgc.Helpers
{
    public static class IconHelper
    {
        public static IHtmlContent Icon(this IHtmlHelper htmlHelper, string iconName, string cssClass = "w-4 h-4")
        {
            var svg = GetIconSvg(iconName, cssClass);
            return new HtmlString(svg);
        }
        
        private static string GetIconSvg(string iconName, string cssClass)
        {
            return iconName.ToLower() switch
            {
                "heart" => $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-heart {cssClass}""><path d=""M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z""></path></svg>",
                
                "x" => $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-x {cssClass}""><path d=""M18 6 6 18""></path><path d=""m6 6 12 12""></path></svg>",
                
                "external-link" => $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-external-link {cssClass}""><path d=""M15 3h6v6""></path><path d=""M10 14 21 3""></path><path d=""M18 13v6a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h6""></path></svg>",
                
                "crown" => $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-crown {cssClass}""><path d=""m2 4 3 12h14l3-12-6 7-4-7-4 7-6-7zm3 16h14""></path></svg>",
                
                "medal" => $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-medal {cssClass}""><path d=""M7.21 15 2.66 7.14a2 2 0 0 1 .13-2.2L4.4 2.8A2 2 0 0 1 6 2h12a2 2 0 0 1 1.6.8l1.6 2.14a2 2 0 0 1 .14 2.2L16.79 15""></path><path d=""M11 12 5.12 2.2""></path><path d=""m13 12 5.88-9.8""></path><path d=""M8 7h8""></path><circle cx=""12"" cy=""17"" r=""5""></circle><path d=""M12 18v-2h-.5""></path></svg>",
                
                "trophy" => $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-trophy {cssClass}""><path d=""M6 9H4.5a2.5 2.5 0 0 1 0-5H6""></path><path d=""M18 9h1.5a2.5 2.5 0 0 0 0-5H18""></path><path d=""M4 22h16""></path><path d=""M10 14.66V17c0 .55-.47.98-.97 1.21C7.85 18.75 7 20.24 7 22""></path><path d=""M14 14.66V17c0 .55.47.98.97 1.21C16.15 18.75 17 20.24 17 22""></path><path d=""M18 2H6v7a6 6 0 0 0 12 0V2Z""></path></svg>",
                
                "send" => $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-send {cssClass}""><path d=""m22 2-7 20-4-9-9-4Z""></path><path d=""M22 2 11 13""></path></svg>",
                
                "vote" => $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-vote {cssClass}""><path d=""m9 12 2 2 4-4""></path><path d=""M5 7c0-1.1.9-2 2-2h10a2 2 0 0 1 2 2v12H5V7Z""></path><path d=""M22 19H2""></path></svg>",
                
                "log-out" => $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-log-out {cssClass}""><path d=""M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4""></path><polyline points=""16 17 21 12 16 7""></polyline><line x1=""21"" x2=""9"" y1=""12"" y2=""12""></line></svg>",
                
                "menu" => $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-menu {cssClass}""><line x1=""4"" x2=""20"" y1=""12"" y2=""12""></line><line x1=""4"" x2=""20"" y1=""6"" y2=""6""></line><line x1=""4"" x2=""20"" y1=""18"" y2=""18""></line></svg>",
                
                _ => string.Empty
            };
        }
        
        public static string GetButtonIcon(string iconName)
        {
            return GetIconSvg(iconName, "w-4 h-4 mr-2");
        }
    }
}
