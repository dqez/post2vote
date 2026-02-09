using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace votegdgc.TagHelpers
{
    [HtmlTargetElement("neo-button")]
    public class NeoButtonTagHelper : TagHelper
    {
        public string Type { get; set; } = "button";
        public string? Href { get; set; }
        public string Variant { get; set; } = "primary"; // primary, secondary, danger, disabled
        public string Size { get; set; } = "md"; // sm, md, lg
        public bool Disabled { get; set; }
        public string? Icon { get; set; }
        public bool FullWidth { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var cssClasses = GetCssClasses();
            var iconSvg = GetIconSvg();
            
            if (!string.IsNullOrEmpty(Href))
            {
                output.TagName = "a";
                output.Attributes.SetAttribute("href", Href);
            }
            else
            {
                output.TagName = "button";
                output.Attributes.SetAttribute("type", Type);
            }
            
            if (Disabled)
            {
                output.Attributes.SetAttribute("disabled", "disabled");
            }
            
            output.Attributes.SetAttribute("class", cssClasses);
            
            var contentBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(iconSvg))
            {
                contentBuilder.Append(iconSvg);
            }
            
            var childContent = output.GetChildContentAsync().Result.GetContent();
            contentBuilder.Append(childContent);
            
            output.Content.SetHtmlContent(contentBuilder.ToString());
        }
        
        private string GetCssClasses()
        {
            var baseClass = "relative inline-flex items-center justify-center font-bold border-2 border-black transition-all active:translate-x-[2px] active:translate-y-[2px] active:shadow-none disabled:opacity-50 disabled:cursor-not-allowed";
            
            var variantClass = Variant.ToLower() switch
            {
                "primary" => "bg-gdg-blue text-white shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000]",
                "secondary" => "bg-gdg-yellow text-black shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000]",
                "danger" => "bg-gdg-red text-black shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000]",
                "white" => "bg-white text-black shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000]",
                "disabled" => "bg-gray-200 text-gray-500 shadow-neo",
                _ => "bg-gdg-blue text-white shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000]"
            };
            
            var sizeClass = Size.ToLower() switch
            {
                "sm" => "px-4 py-2 text-sm",
                "md" => "px-6 py-3 text-base",
                "lg" => "px-8 py-4 text-lg",
                _ => "px-6 py-3 text-base"
            };
            
            var widthClass = FullWidth ? "w-full" : "";
            
            return $"{baseClass} {variantClass} {sizeClass} {widthClass}".Trim();
        }
        
        private string GetIconSvg()
        {
            if (string.IsNullOrEmpty(Icon)) return string.Empty;
            
            return Icon.ToLower() switch
            {
                "heart" => @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-heart w-4 h-4 mr-2""><path d=""M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z""></path></svg>",
                "x" => @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-x w-4 h-4 mr-2""><path d=""M18 6 6 18""></path><path d=""m6 6 12 12""></path></svg>",
                "external-link" => @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-external-link w-4 h-4 mr-2""><path d=""M15 3h6v6""></path><path d=""M10 14 21 3""></path><path d=""M18 13v6a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h6""></path></svg>",
                _ => string.Empty
            };
        }
    }
}
