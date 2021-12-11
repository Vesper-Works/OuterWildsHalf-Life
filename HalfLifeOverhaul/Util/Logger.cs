using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HalfLifeOverhaul.Util
{
    static class Logger
    {
        public static void Log(string message, OWML.Common.MessageType type = OWML.Common.MessageType.Info)
        {
            MainBehaviour.instance.ModHelper.Console.WriteLine($"{type}: {message}", type);
        }

        public static void LogWarning(string message)
        {
            Log(message, OWML.Common.MessageType.Warning);
        }

        public static void LogError(string message)
        {
            Log(message, OWML.Common.MessageType.Error);
        }

        public static void LogPath(GameObject obj)
        {
            Log(Utility.GetPath(obj.transform));
        }

        public static void LogSkeleton(SkinnedMeshRenderer mesh)
        {
            foreach(var bone in mesh.bones)
            {
                MainBehaviour.instance.ModHelper.Console.WriteLine($"BONE: {bone.parent.name} -> {bone.name}");
            }
        }
    }
}
