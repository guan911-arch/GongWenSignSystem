using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace GongWenSignSystem.Utils
{
    /// <summary>
    /// EnvHelper provides utilities for path resolution and environment management.
    /// Ensures that paths for fonts, templates, and assets are resolved correctly regardless of current working directory.
    /// </summary>
    public static class EnvHelper
    {
        private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Resolves a relative path to an absolute path based on the application root.
        /// </summary>
        public static string ResolvePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return string.Empty;

            if (Path.IsPathRooted(relativePath)) return relativePath;

            // Normalize path delimiters
            string normalizedPath = relativePath.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

            return Path.GetFullPath(Path.Combine(BasePath, normalizedPath));
        }

        /// <summary>
        /// Returns the absolute path to the Assets directory.
        /// </summary>
        public static string GetAssetsPath() => ResolvePath("Assets");

        /// <summary>
        /// Returns the absolute path to the Fonts directory.
        /// </summary>
        public static string GetFontsPath() => ResolvePath("Assets/Fonts");

        /// <summary>
        /// Returns the absolute path to the config templates directory.
        /// </summary>
        public static string GetTemplatesPath() => ResolvePath("config/templates");

        /// <summary>
        /// Checks if a directory exists, creating it if it doesn't.
        /// </summary>
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
