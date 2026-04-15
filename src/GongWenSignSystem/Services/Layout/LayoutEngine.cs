using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using GongWenSignSystem.Services.Printing;

namespace GongWenSignSystem.Services.Layout
{
    /// <summary>
    /// LayoutEngine implements the national standard for government document pagination.
    /// It handles text wrapping, line spacing, and automatic pagination based on physical mm.
    /// </summary>
    public class LayoutEngine : ILayoutEngine
    {
        private const double PtToMm = 25.4 / 72.0; // 1 point = 1/72 inch; 1 inch = 25.4 mm

        public List<LayoutElement> CalculateLayout(string content, FontFamily fontFamily, double fontSizePt)
        {
            var elements = new List<LayoutElement>();
            if (string.IsNullOrEmpty(content)) return elements;

            // 1. Calculate initial physical parameters
            double currentX = LayoutConstants.MarginLeftMm;
            double currentY = LayoutConstants.MarginTopMm;
            int currentPage = 0;
            double fontSizeMm = fontSizePt * PtToMm;
            double lineSpacingMm = fontSizeMm * LayoutConstants.StandardLineSpacing;

            // 2. Prepare text measuring tool (WPF FormattedText uses pixels, we convert to mm)
            // We use a dummy FormattedText to measure the real width of characters in the specific font
            string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var lineText in lines)
            {
                string processingText = lineText;
                bool isFirstLineOfParagraph = true;

                // Split line into words/characters for wrapping
                // In Chinese official documents, we wrap by character weight
                var characters = new List<char>(processingText);
                int charIndex = 0;

                while (charIndex < characters.Count)
                {
                    // Handle indentation for the first line of a paragraph (2 characters indent)
                    if (isFirstLineOfParagraph)
                    {
                        currentX += fontSizeMm * 2;
                    }

                    // Find how many characters fit in the remaining width of the current line
                    string lineSegment = "";
                    double segmentWidthMm = 0;

                    while (charIndex < characters.Count)
                    {
                        char c = characters[charIndex];
                        double charWidthMm = MeasureCharWidth(c, fontFamily, fontSizePt);

                        if (currentX + segmentWidthMm + charWidthMm > LayoutConstants.PageWidthMm - LayoutConstants.MarginRightMm)
                        {
                            break; // Line overflow
                        }

                        lineSegment += c;
                        segmentWidthMm += charWidthMm;
                        charIndex++;
                    }

                    // Create the layout element for this segment
                    elements.Add(new LayoutElement
                    {
                        Text = lineSegment,
                        Position = new PhysicalPoint(currentX, currentY),
                        FontFamily = fontFamily,
                        FontSizePt = fontSizePt,
                        PageIndex = currentPage
                    });

                    // Prepare for next line
                    currentX = LayoutConstants.MarginLeftMm;
                    currentY += lineSpacingMm;
                    isFirstLineOfParagraph = false;

                    // Check for page overflow (Automatic Pagination)
                    if (currentY + fontSizeMm > LayoutConstants.PageHeightMm - LayoutConstants.MarginBottomMm)
                    {
                        currentY = LayoutConstants.MarginTopMm;
                        currentPage++;
                    }
                }
            }

            return elements;
        }

        /// <summary>
        /// Measures the width of a single character in millimeters using WPF's rendering engine.
        /// </summary>
        private double MeasureCharWidth(char c, FontFamily fontFamily, double fontSizePt)
        {
            // Convert pt to px for WPF measurement (1 pt = 1/72", 1 px = 1/96")
            double fontSizePx = fontSizePt * (96.0 / 72.0);

            FormattedText ft = new FormattedText(
                c.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                fontFamily,
                fontSizePx,
                Brushes.Black);

            // Convert pixels to mm: px / (96/25.4)
            return ft.Width / (96.0 / 25.4);
        }
    }
}
