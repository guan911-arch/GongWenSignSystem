using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows;

namespace GongWenSignSystem.Services.Fonts
{
    /// <<summarysummary>
    /// FontService handles the loading of private fonts (FangSong, KaiTi, XiaobiaoSong)
    /// to ensure layout consistency regardless of system-installed fonts.
    /// </summary>
    public class FontService
    {
        private readonly Dictionary<<stringstring, Typeface> _loadedFonts = new Dictionary<<stringstring, Typeface>();

        /// <<summarysummary>
        /// Loads a font from a physical file path and registers it in the service.
        /// </summary>
        /// <<paramparam name="fontName">The name to associate with the font (e.g., "FangSong")</param>
        /// <<paramparam name="filePath">Absolute path to the .ttf file</param>
        public void LoadFont(string fontName, string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Font file not found at: {filePath}");
            }

            try
            {
                // In WPF, we can use the FontFamily constructor with an absolute path to the file
                // The path must follow the format: "FILE:C:\path\to\font.ttf" or "/C:/path/to/font.ttf"
                string formattedPath = $"file:///{filePath.Replace('\\', '/')}";
                var fontFamily = new FontFamily(new Uri(formattedPath));

                // We store the typeface to verify it's actually loaded
                _loadedFonts[fontName] = fontFamily.Typefaces[0];
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load font {fontName} from {filePath}: {ex.Message}");
            }
        }

        /// <<summarysummary>
        /// Gets the FontFamily for a previously loaded font name.
        /// </summary>
        public FontFamily GetFontFamily(string fontName)
        {
            if (!_loadedFonts.ContainsKey(fontName))
            {
                throw new KeyNotFoundException($"Font {fontName} has not been loaded. Please call LoadFont first.");
            }

            // Note: To return the FontFamily, we need to recreate the URI or store the FontFamily itself.
            // For simplicity in this implementation, let's extend the storage to store FontFamily.
            return new FontFamily(_loadedFonts[fontName].Source);
        }

        // Overloaded method for easier use in ViewModels:
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
