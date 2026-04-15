using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace GongWenSignSystem.Services.Printing
{
    /// <summary>
    /// Represents a point in physical millimeters.
    /// </summary>
    public struct PhysicalPoint
    {
        public double X; // mm
        public double Y; // mm

        public PhysicalPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// IPrintEngine defines the contract for rendering content onto a physical document.
    /// It abstracts the PDF generation library to allow changing the underlying engine.
    /// </summary>
    public interface IPrintEngine
    {
        /// <summary>
        /// Initializes the print engine with a specific page size (usually A4).
        /// </summary>
        void Initialize(double pageWidthMm, double pageHeightMm);

        /// <summary>
        /// Sets the background image or PDF template for the current page.
        /// </summary>
        void SetBackgroundTemplate(string templatePath);

        /// <summary>
        /// Draws a string of text at a precise physical location.
        /// </summary>
        /// <param name="text">The text to render</param>
        /// <param name="position">Physical coordinates in mm</param>
        /// <param name="fontFamily">The loaded font family</param>
        /// <param name="fontSizePt">Font size in points</param>
        /// <param name="color">Text color</param>
        void DrawText(string text, PhysicalPoint position, FontFamily fontFamily, double fontSizePt, Color color);

        /// <summary>
        /// Saves the current document to a PDF file.
        /// </summary>
        void SavePdf(string outputPath);
    }
}
