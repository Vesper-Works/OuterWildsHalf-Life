using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HalfLifeOverhaul.Util
{
    public static class Utility
    {
        public static Transform SearchInChildren(Transform parent, string target)
        {
            if (parent.name.Equals(target)) return parent;

            foreach (Transform child in parent)
            {
                var search = SearchInChildren(child, target);
                if (search != null) return search;
            }

            return null;
        }

        public static T SearchComponentInParents<T>(Transform start)
        {
            var component = start.GetComponentInChildren<T>(true);
            if (component != null) return component;
            if (start.parent != null) return SearchComponentInParents<T>(start.parent);
            return default(T);
        } 

        public static GameObject[] FindObjectsWithName(string name)
        {
            return Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Equals(name)).ToArray();
        }

        public static string GetPath(Transform t)
        {
            if (t.parent == null) return t.name;
            return GetPath(t.parent) + "/" + t.name;
        }

        public static void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = (int)Random.Range(0, n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
