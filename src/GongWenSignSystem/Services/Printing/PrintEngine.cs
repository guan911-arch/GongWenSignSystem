using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GongWenSignSystem.Services.Printing
{
    public class PrintEngine : IPrintEngine
    {
        private double _pageWidthPx;
        private double _pageHeightPx;
        private double _pageWidthMm;
        private double _pageHeightMm;
        private const double MmToPxRatio = 96.0 / 25.4;

        private readonly DrawingVisual _rootVisual = new DrawingVisual();

        public void Initialize(double pageWidthMm, double pageHeightMm)
        {
            _pageWidthMm = pageWidthMm;
            _pageHeightMm = pageHeightMm;
            _pageWidthPx = pageWidthMm * MmToPxRatio;
            _pageHeightPx = pageHeightMm * MmToPxRatio;
        }

        public void SetBackgroundTemplate(string templatePath)
        {
            if (string.IsNullOrEmpty(templatePath) || !File.Exists(templatePath))
                return;

            try
            {
                BitmapImage bitmap = new BitmapImage(new Uri(templatePath));
                using (DrawingContext dc = _rootVisual.RenderOpen())
                {
                    dc.DrawImage(bitmap, new Rect(0, 0, _pageWidthPx, _pageHeightPx));
                }
            }
            catch { /* Log error */ }
        }

        public void DrawText(string text, PhysicalPoint position, FontFamily fontFamily, double fontSizePt, Color color)
        {
            double xPx = position.X * MmToPxRatio;
            double yPx = position.Y * MmToPxRatio;
            double fontSizePx = fontSizePt * (96.0 / 72.0);

            using (DrawingContext dc = _rootVisual.RenderOpen())
            {
                FormattedText formattedText = new FormattedText(
                    text,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    fontFamily,
                    fontSizePx,
                    new SolidColorBrush(color),
                    // Setting DPI explicitly to avoidVisualTreeHelper errors during non-UI thread calls
                    96.0);

                dc.DrawText(formattedText, new Point(xPx, yPx));
            }
        }

        public void SavePdf(string outputPath)
        {
            File.WriteAllText(outputPath, "PDF Export Simulation");
        }

        public DrawingVisual GetRenderedVisual() => _rootVisual;
    }
}
