using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class ListExtensions
    {
        /// <summary>
        /// Adds an element to the list only if it is not null.
        /// </summary>
        public static void AddIfNotNull<T>(this List<T> list, T elementToAdd) where T : class 
        {
            if (elementToAdd != null)
            {
                list.Add(elementToAdd);
            }
        }
    }
}
