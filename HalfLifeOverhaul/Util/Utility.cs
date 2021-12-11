using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

        public static GameObject[] FindObjectsWithName(string name)
        {
            return Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Equals(name)).ToArray();
        }

        public static string GetPath(Transform t)
        {
            if (t.parent == null) return t.name;
            return GetPath(t.parent) + "/" + t.name;
        }
    }
}
