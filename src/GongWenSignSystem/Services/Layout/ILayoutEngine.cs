using System;
using System.Collections.Generic;
using System.Windows.Media;
using GongWenSignSystem.Services.Printing;

namespace GongWenSignSystem.Services.Layout
{
    /// <summary>
    /// Represents a calculated piece of text ready for rendering,
    /// containing its precise physical position and required font.
    /// </summary>
    public class LayoutElement
    {
        public string Text { get; set; }
        public PhysicalPoint Position { get; set; }
        public FontFamily FontFamily { get; set; }
        public double FontSizePt { get; set; }
        public int PageIndex { get; set; }
    }

    /// <summary>
    /// ILayoutEngine handles the logical arrangement of text on pages.
    /// It converts a stream of text and formatting rules into a list of physical layout elements.
    /// </summary>
    public interface ILayoutEngine
    {
        /// <summary>
        /// Calculates the layout for a given text content based on standard official rules.
        /// </summary>
        /// <param name="content">The raw text content of the document</param>
        /// <param name="fontFamily">The primary font to use for width calculations</param>
        /// <param name="fontSizePt">The font size in points</param>
        /// <returns>A list of elements with precise physical coordinates</returns>
        List<LayoutElement> CalculateLayout(string content, FontFamily fontFamily, double fontSizePt);
    }
}
