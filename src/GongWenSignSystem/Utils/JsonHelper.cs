using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace GongWenSignSystem.Utils
{
    /// <summary>
    /// JsonHelper provides generic wrapper for System.Text.Json to facilitate
    /// loading and saving of configuration templates.
    /// </summary>
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        /// <summary>
        /// Reads a JSON file and deserializes it into the specified type.
        /// </summary>
        public static T Read<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"JSON configuration file not found: {filePath}");
            }

            try
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(jsonString, Options);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deserializing JSON from {filePath}: {ex.Message}");
            }
        }

        /// <summary>
        /// Serializes an object and writes it to a JSON file.
        /// </summary>
        public static void Write<T>(string filePath, T data)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(data, Options);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing JSON to {filePath}: {ex.Message}");
            }
        }
    }
}
