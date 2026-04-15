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
                // In WPF, the correct way to load a standalone .ttf file is using a path string
                // with the format "C:\path\to\font.ttf#Font Name"
                // Since we might not know the internal font name, we can try the file path directly
                // however, for custom fonts, the most reliable way is embedding or installing.
                // For a standalone app, we use the path-to-file syntax.
                string fontPath = filePath.Replace('\\', '/');
                var fontFamily = new FontFamily(new Uri(fontPath), new Uri(""));

                // Correct WPF FontFamily constructor for local files:
                // new FontFamily("file:///C:/path/to/font.ttf")
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
                return new FontFamily("Microsoft YaHei"); // Fallback
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
                try { LoadFont(name, path); } catch { /* Log and continue */ }
            }
        }
    }
}
