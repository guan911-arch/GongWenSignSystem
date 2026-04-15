using System;
using System.Collections.Generic;

namespace GongWenSignSystem.Models.Templates
{
    public class TemplateField
    {
        public string? FieldName { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }
        public double FontSizePt { get; set; }
        public string? DefaultFont { get; set; } = string.Empty;
    }

    public class LayoutConfig
    {
        public string? TemplateName { get; set; } = string.Empty;
        public string? PdfBackgroundPath { get; set; } = string.Empty;
        public List<TemplateField> Fields { get; set; } = new List<TemplateField>();
    }
}
