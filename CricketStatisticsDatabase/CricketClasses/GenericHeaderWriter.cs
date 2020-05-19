using StructureCommon.Extensions;

namespace ExportHelpers
{
    public static class GenericHeaderWriter
    {
        public static string TableHeader<T>(T objectToUse, string separator) where T : class
        {
            var properties = objectToUse.GetType().GetProperties();
            string header = string.Empty;
            foreach (var property in properties)
            {
                header += property.Name;
                header += separator;
            }

            return header;
        }

        public static string TableData<T>(T objectToWrite, string separator)
        {
            var properties = objectToWrite.GetType().GetProperties();
            string data = string.Empty;

            for (int i = 0; i < properties.Length; i++)
            {
                bool isDouble = double.TryParse(properties[i].GetValue(objectToWrite).ToString(), out double value);
                if (isDouble)
                {
                    data += value.TruncateToString();
                }
                else
                {
                    data += properties[i].GetValue(objectToWrite);
                }

                data += separator;
            }

            return data;
        }
    }
}
