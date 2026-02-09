using Microsoft.AspNetCore.Razor.TagHelpers;

namespace votegdgc.TagHelpers
{
    [HtmlTargetElement("neo-card")]
    public class NeoCardTagHelper : TagHelper
    {
        public string? BorderColor { get; set; }
        public bool Hoverable { get; set; } = false;
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            
            var cssClasses = GetCssClasses();
            output.Attributes.SetAttribute("class", cssClasses);
        }
        
        private string GetCssClasses()
        {
            var baseClass = "border-2 shadow-neo p-6 bg-white";
            var borderClass = BorderColor ?? "border-black";
            var hoverClass = Hoverable ? "hover:border-gdg-blue transition-colors duration-300" : "";
            
            return $"{baseClass} {borderClass} {hoverClass}".Trim();
        }
    }
}
