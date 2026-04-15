using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace GongWenSignSystem.Services.Fonts
{
    public class FontService
    {
        private readonly Dictionary<string, FontFamily> _loadedFonts = new Dictionary<string, FontFamily>();

        public void LoadFont(string fontName, string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Font file not found at: {filePath}");
            }

            try
            {
                string fontPath = filePath.Replace('\\', '/');
                // Standard WPF local file font format: "file:///C:/path/to/font.ttf"
                var finalFamily = new FontFamily($"file:///{fontPath}");
                _loadedFonts[fontName] = finalFamily;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load font {fontName} from {filePath}: {ex.Message}");
            }
        }

        public FontFamily GetFontFamily(string fontName)
        {
            if (!_loadedFonts.ContainsKey(fontName))
            {
                return new FontFamily("Microsoft YaHei");
            }
            return _loadedFonts[fontName];
        }

        public void LoadAllDefaultFonts(string fontsDirectory)
        {
            string[] defaultFonts = { "fangsong.ttf", "kaiti.ttf", "xiaobiaosong.ttf" };
            foreach (var fontFile in defaultFonts)
            {
                string path = Path.Combine(fontsDirectory, fontFile);
                string name = Path.GetFileNameWithoutExtension(fontFile);
                try { LoadFont(name, path); } catch { }
            }
        }
    }
}
