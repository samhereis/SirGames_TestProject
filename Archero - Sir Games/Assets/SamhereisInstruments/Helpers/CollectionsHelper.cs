using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Helpers
{
    public static class CollectionsHelper
    {

        #region List

        public static void RemoveNulls<T>(this List<T> list)
        {
            list.RemoveAll(x => x == null);
        }

        public static void SafeAdd<T>(this List<T> list, T item)
        {
            if (list.Contains(item) == false) list.Add(item);
        }

        public static void SafeRemove<T>(this List<T> list, T item)
        {
            if (list.Contains(item) == true) list.Remove(item);
        }

        public static void RemoveDuplicates<T>(this List<T> list)
        {
            var listCopy = new HashSet<T>();

            foreach (var item in list.ToHashSet())
            {
                listCopy.Add(item);
            }

            list = listCopy.ToList();
        }

        public static T GetRandom<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static int GetRandomIndex<T>(this List<T> list)
        {
            return Random.Range(0, list.Count);
        }

        #endregion

        #region Array

        public static T GetRandom<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        #endregion
    }
}