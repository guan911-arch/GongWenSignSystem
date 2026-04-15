using System;
using System.Collections.Generic;

namespace GongWenSignSystem.Models.Templates
{
    public class TemplateField
    {
        public string FieldName { get; set; }
        public double X { get; set; } // mm
        public double Y { get; set; } // mm
        public double FontSizePt { get; set; }
        public string DefaultFont { get; set; }
    }

    public class LayoutConfig
    {
        public string TemplateName { get; set; }
        public string PdfBackgroundPath { get; set; }
        public List<TemplateField> Fields { get; set; } = new List<TemplateField>();
    }
}
