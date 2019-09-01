using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Cricket
{
    public static class Globals
    {
        public static List<int> DataCleanse(params string[] inputdata)
        {
            List<string> inputdatalist = new List<string>();
            foreach (string input in inputdata)
            {
                inputdatalist.Add(input);
            }

            List<int> outputs = new List<int>(new int[11]);

            int result = 0;
            for (int i = 0; i < inputdatalist.Count; i++)
            {
                result = 0;
                // if user entered a value, input that, otherwise, return 0 runs scored.
                outputs[i] = int.TryParse(inputdatalist[i], out result) ? result : 0;
            }

            return outputs;
        }


        public static List<Cricket_Player> Ardeley = new List<Cricket_Player>();

        public static List<Cricket_Match> GamesPlayed = new List<Cricket_Match>();

        public static Cricket_Player GetPlayerFromName(string name)
        {
            foreach (Cricket_Player person in Ardeley)
            {
                if (person.Name == name)
                {
                    return person;
                }
            }

            return null;
        }

        public static int IndexFromPlayerName(string name)
        {
            int numbplayers = Ardeley.Count;
            for(int i = 0; i < numbplayers; ++i)
            {
                if (Ardeley[i].Name == name)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Saving routine of database
        /// </summary>
        public static void SaveDatabase()
        {
            WriteToXmlFile<List<Cricket_Player>>("playerdata.xml", Ardeley);
            WriteToXmlFile<List<Cricket_Match>>("gamesdata.xml", GamesPlayed);
        }

        public static void LoadDatabase()
        {
            if (File.Exists("playerdata.xml"))
            {
                Ardeley = ReadFromXmlFile<List<Cricket_Player>>("playerdata.xml");
            }
            if (File.Exists("gamesdata.xml"))
            {
                GamesPlayed = ReadFromXmlFile<List<Cricket_Match>>("gamesdata.xml");
            }
        }

        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

    }
}

