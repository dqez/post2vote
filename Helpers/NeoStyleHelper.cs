using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace votegdgc.Helpers
{
    public static class NeoStyleHelper
    {
        /// <summary>
        /// Generates CSS classes for neo-brutalism buttons
        /// </summary>
        public static string NeoButton(string variant = "primary", string size = "md", bool fullWidth = false)
        {
            var baseClass = "relative inline-flex items-center justify-center font-bold border-2 border-black transition-all active:translate-x-[2px] active:translate-y-[2px] active:shadow-none disabled:opacity-50 disabled:cursor-not-allowed";
            
            var variantClass = variant.ToLower() switch
            {
                "primary" => "bg-gdg-blue text-white shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000]",
                "secondary" => "bg-gdg-yellow text-black shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000]",
                "danger" => "bg-gdg-red text-black shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000]",
                "white" => "bg-white text-black shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000]",
                _ => "bg-gdg-blue text-white shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000]"
            };
            
            var sizeClass = size.ToLower() switch
            {
                "sm" => "px-4 py-2 text-sm",
                "md" => "px-6 py-3 text-base",
                "lg" => "px-8 py-4 text-lg",
                _ => "px-6 py-3 text-base"
            };
            
            var widthClass = fullWidth ? "w-full" : "";
            
            return $"{baseClass} {variantClass} {sizeClass} {widthClass}".Trim();
        }
        
        /// <summary>
        /// Generates CSS classes for neo-brutalism cards
        /// </summary>
        public static string NeoCard(bool hoverable = false, string borderColor = "border-black")
        {
            var baseClass = "border-2 shadow-neo p-6 bg-white";
            var hoverClass = hoverable ? "hover:border-gdg-blue transition-colors duration-300" : "";
            
            return $"{baseClass} {borderColor} {hoverClass}".Trim();
        }
        
        /// <summary>
        /// Generates CSS classes for neo-brutalism badges
        /// </summary>
        public static string NeoBadge(string variant = "yellow", string size = "sm")
        {
            var variantClass = variant.ToLower() switch
            {
                "yellow" => "bg-gdg-yellow text-black",
                "blue" => "bg-gdg-blue text-white",
                "red" => "bg-gdg-red text-black",
                _ => "bg-gdg-yellow text-black"
            };
            
            var sizeClass = size.ToLower() switch
            {
                "sm" => "px-2 py-1 text-xs",
                "md" => "px-3 py-1 text-sm",
                _ => "px-2 py-1 text-xs"
            };
            
            return $"{variantClass} {sizeClass} border-2 border-black font-bold uppercase inline-block shadow-[2px_2px_0px_0px_#000]";
        }
    }
}
