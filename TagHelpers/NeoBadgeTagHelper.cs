using Microsoft.AspNetCore.Razor.TagHelpers;

namespace votegdgc.TagHelpers
{
    [HtmlTargetElement("neo-badge")]
    public class NeoBadgeTagHelper : TagHelper
    {
        public string Variant { get; set; } = "yellow"; // yellow, blue, red
        public string Size { get; set; } = "md"; // sm, md
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            
            var cssClasses = GetCssClasses();
            output.Attributes.SetAttribute("class", cssClasses);
        }
        
        private string GetCssClasses()
        {
            var variantClass = Variant.ToLower() switch
            {
                "yellow" => "bg-gdg-yellow text-black",
                "blue" => "bg-gdg-blue text-white",
                "red" => "bg-gdg-red text-black",
                _ => "bg-gdg-yellow text-black"
            };
            
            var sizeClass = Size.ToLower() switch
            {
                "sm" => "px-2 py-1 text-xs",
                "md" => "px-3 py-1 text-sm",
                _ => "px-2 py-1 text-xs"
            };
            
            return $"{variantClass} {sizeClass} border-2 border-black font-bold uppercase inline-block shadow-[2px_2px_0px_0px_#000]";
        }
    }
}
