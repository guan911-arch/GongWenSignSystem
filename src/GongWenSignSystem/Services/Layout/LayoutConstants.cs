using System;

namespace GongWenSignSystem.Services.Layout
{
    /// <summary>
    /// LayoutConstants defines the standard physical dimensions for official government documents
    /// according to national standards (A4).
    /// </summary>
    public static class LayoutConstants
    {
        // Page Size (A4) in mm
        public const double PageWidthMm = 210.0;
        public const double PageHeightMm = 297.0;

        // Standard Margins in mm (Approximate based on GB/T 9704)
        public const double MarginTopMm = 35.0;
        public const double MarginBottomMm = 30.0;
        public const double MarginLeftMm = 28.0;
        public const double MarginRightMm = 28.0;

        // Text Area dimensions
        public static readonly double TextAreaWidthMm = PageWidthMm - MarginLeftMm - MarginRightMm;
        public static readonly double TextAreaHeightMm = PageHeightMm - MarginTopMm - MarginBottomMm;

        // Common Line Spacing (Multiplier)
        public const double StandardLineSpacing = 1.5;
    }
}
