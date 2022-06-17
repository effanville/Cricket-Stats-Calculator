using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Common.Structure.FileAccess;

namespace CricketStructures.Tests
{
    /// <summary>
    /// Provides mechanisms to retrieve files from a standard Examples folder.
    /// </summary>
    internal static class ExampleFileHelpers
    {
        private static IDictionary<string, string> RetrievedFiles { get; } = new Dictionary<string, string>();
        private static string AssemblyLocation => Directory.GetParent(Assembly.GetAssembly(typeof(ExampleFileHelpers)).Location).FullName;
        private static string ExampleFileFolder => Path.Combine(AssemblyLocation, "Examples");

        /// <summary>
        /// Retrieves an example file from the Examples folder with caching.
        /// </summary>
        /// <param name="fileName">the name of the file in the Examples folder.</param>
        /// <returns>The string representing the file.</returns>
        public static string GetLazyCachedExampleFile(string fileName)
        {
            if (!RetrievedFiles.TryGetValue(fileName, out string result))
            {
                string file = File.ReadAllText(Path.Combine(ExampleFileFolder, fileName));
                RetrievedFiles[fileName] = file;
                return file;
            }

            return result;
        }

        /// <summary>
        /// Retrieves an example file from the Examples folder.
        /// </summary>
        /// <param name="fileName">the name of the file in the Examples folder.</param>
        /// <returns>The string representing the file.</returns>
        public static string GetExampleFile(string fileName)
        {
            string file = File.ReadAllText(Path.Combine(ExampleFileFolder, fileName));
            return file;
        }

        /// <summary>
        /// Retrieves an example file from the Examples folder.
        /// </summary>
        /// <param name="fileName">the name of the file in the Examples folder.</param>
        /// <returns>The string representing the file.</returns>
        public static T GetExampleFromXmlFile<T>(string fileName)
            where T : new()
        {
            T team = XmlFileAccess.ReadFromXmlFile<T>(Path.Combine(ExampleFileFolder, fileName), out string error);
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }
            return team;
        }
    }
}
