using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HalfLifeOverhaul
{
    static class Logger
    {
        public static void Log(string message, OWML.Common.MessageType type = OWML.Common.MessageType.Info)
        {
            MainBehaviour.instance.ModHelper.Console.WriteLine($"{type}: {message}", type);
        }

        public static void LogPath(GameObject obj)
        {
            Log(GetPath(obj.transform));
        }

        private static string GetPath(Transform t)
        {
            if (t.parent == null) return t.name;
            return GetPath(t.parent) + t.name;
        }
    }
}
