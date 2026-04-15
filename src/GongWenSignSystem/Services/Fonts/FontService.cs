using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows;

namespace GongWenSignSystem.Services.Fonts
{
    /// <summary>
    /// FontService handles the loading of private fonts (FangSong, KaiTi, XiaobiaoSong)
    /// to ensure layout consistency regardless of system-installed fonts.
    /// </summary>
    public class FontService
    {
        private readonly Dictionary<string, Typeface> _loadedFonts = new Dictionary<string, Typeface>();

        /// <summary>
        /// Loads a font from a physical file path and registers it in the service.
        /// </summary>
        public void LoadFont(string fontName, string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Font file not found at: {filePath}");
            }

            try
            {
                string formattedPath = $"file:///{filePath.Replace('\\', '/')}";
                var fontFamily = new FontFamily(new Uri(formattedPath));
                _loadedFonts[fontName] = fontFamily.Typefaces[0];
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
                throw new KeyNotFoundException($"Font {fontName} has not been loaded.");
            }

            return new FontFamily(_loadedFonts[fontName].Source);
        }

        public void LoadAllDefaultFonts(string fontsDirectory)
        {
            string[] defaultFonts = { "fangsong.ttf", "kaiti.ttf", "xiaobiaosong.ttf" };
            foreach (var fontFile in defaultFonts)
            {
                string path = Path.Combine(fontsDirectory, fontFile);
                string name = Path.GetFileNameWithoutExtension(fontFile);
                LoadFont(name, path);
            }
        }
    }
}
