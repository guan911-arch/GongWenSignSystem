using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using GongWenSignSystem.Services.Printing;
using GongWenSignSystem.Services.Layout;
using GongWenSignSystem.Services.Fonts;
using GongWenSignSystem.Utils;
using GongWenSignSystem.Models.Templates;

namespace GongWenSignSystem.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IPrintEngine _printEngine;
        private readonly ILayoutEngine _layoutEngine;
        private readonly FontService _fontService;

        public MainViewModel()
        {
            _printEngine = new PrintEngine();
            _layoutEngine = new LayoutEngine();
            _fontService = new FontService();

            // Initialize foundations
            InitSystem();
        }

        private void InitSystem()
        {
            // 1. Load Fonts
            string fontsPath = EnvHelper.GetFontsPath();
            _fontService.LoadAllDefaultFonts(fontsPath);

            // 2. Setup Print Engine
            _printEngine.Initialize(LayoutConstants.PageWidthMm, LayoutConstants.PageHeightMm);
        }

        private LayoutConfig _currentConfig;
        public LayoutConfig CurrentConfig
        {
            get => _currentConfig;
            set { _currentConfig = value; OnPropertyChanged(); }
        }

        private string _documentContent;
        public string DocumentContent
        {
            get => _documentContent;
            set
            {
                _documentContent = value;
                OnPropertyChanged();
                RefreshPreview();
            }
        }

        public void LoadTemplate(string templateName)
        {
            string path = System.IO.Path.Combine(EnvHelper.GetTemplatesPath(), $"{templateName}_header.json");
            CurrentConfig = JsonHelper.Read<LayoutConfig>(path);
            RefreshPreview();
        }

        public void RefreshPreview()
        {
            // Clear previous
            // In a real implementation, this would reset the PrintEngine's visual

            if (CurrentConfig != null)
            {
                _printEngine.SetBackgroundTemplate(CurrentConfig.PdfBackgroundPath);
            }

            if (!string.IsNullOrEmpty(DocumentContent))
            {
                // Use FangSong as default for official documents
                var font = _fontService.GetFontFamily("fangsong");
                var layoutElements = _layoutEngine.CalculateLayout(DocumentContent, font, 14); // 14pt approx Small 3rd size

                foreach (var element in layoutElements)
                {
                    _printEngine.DrawText(element.Text, element.Position, element.FontFamily, element.FontSizePt, Colors.Black);
                }
            }

            OnPropertyChanged(nameof(PreviewVisual));
        }

        // This is what the PrintPreviewer.xaml binds to
        public object PreviewVisual => ((PrintEngine)_printEngine).GetRenderedVisual();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
