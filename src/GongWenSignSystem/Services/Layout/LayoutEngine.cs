using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using GongWenSignSystem.Services.Printing;

namespace GongWenSignSystem.Services.Layout
{
    public class LayoutEngine : ILayoutEngine
    {
        private const double PtToMm = 25.4 / 72.0;

        public List<LayoutElement> CalculateLayout(string content, FontFamily fontFamily, double fontSizePt)
        {
            var elements = new List<LayoutElement>();
            if (string.IsNullOrEmpty(content)) return elements;

            double currentX = LayoutConstants.MarginLeftMm;
            double currentY = LayoutConstants.MarginTopMm;
            int currentPage = 0;
            double fontSizeMm = fontSizePt * PtToMm;
            double lineSpacingMm = fontSizeMm * LayoutConstants.StandardLineSpacing;

            string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var lineText in lines)
            {
                string processingText = lineText;
                bool isFirstLineOfParagraph = true;
                var characters = new List<char>(processingText);
                int charIndex = 0;

                while (charIndex < characters.Count)
                {
                    if (isFirstLineOfParagraph)
                    {
                        currentX += fontSizeMm * 2;
                    }

                    string lineSegment = "";
                    double segmentWidthMm = 0;

                    while (charIndex < characters.Count)
                    {
                        char la = characters[charIndex];
                        double charWidthMm = MeasureCharWidth(la, fontFamily, fontSizePt);

                        if (currentX + segmentWidthMm + charWidthMm > LayoutConstants.PageWidthMm - LayoutConstants.MarginRightMm)
                        {
                            break;
                        }

                        lineSegment += la;
                        segmentWidthMm += charWidthMm;
                        charIndex++;
                    }

                    elements.Add(new LayoutElement
                    {
                        Text = lineSegment,
                        Position = new PhysicalPoint(currentX, currentY),
                        FontFamily = fontFamily,
                        FontSizePt = fontSizePt,
                        PageIndex = currentPage
                    });

                    currentX = LayoutConstants.MarginLeftMm;
                    currentY += lineSpacingMm;
                    isFirstLineOfParagraph = false;

                    if (currentY + fontSizeMm > LayoutConstants.PageHeightMm - LayoutConstants.MarginBottomMm)
                    {
                        currentY = LayoutConstants.MarginTopMm;
                        currentPage++;
                    }
                }
            }
            return elements;
        }

        private double MeasureCharWidth(char c, FontFamily fontFamily, double fontSizePt)
        {
            double fontSizePx = fontSizePt * (96.0 / 72.0);

            // FIX: Use Typeface constructor instead of non-existent GetTypeface method
            Typeface typeface = new Typeface(fontFamily);

            FormattedText ft = new FormattedText(
                c.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSizePx,
                Brushes.Black,
                96.0);

            return ft.Width / (96.0 / 25.4);
        }
    }
}
