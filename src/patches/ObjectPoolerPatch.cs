using flanne;
using HarmonyLib;
using UnityEngine;
using static Healthbars.Plugin;
using System.Collections.Generic;

// HealthBar offset fix -> sprite renderer

namespace Healthbars
{
    internal class ObjectPoolerPatch
    {
        private static List<string> tagList = new List<string> {
            "BrainMonster",
            "Boomer",
            "ElderBrain",
            "EyeMonster",
            "Lamprey",
            "WingedMonster",
            "SpawnerMonster",
            "ShubNiggurath",
            "Shoggoth"
        };

        [HarmonyPatch(typeof(ObjectPooler), "Awake")]
        [HarmonyPrefix]
        static void AwakePrefix(ref ObjectPooler __instance)
        {
            foreach (ObjectPoolItem objectPoolItem in __instance.itemsToPool)
            {
                if (tagList.Contains(objectPoolItem.tag))
                {
                    TryAddHealthBarComponent(objectPoolItem.objectToPool.gameObject);
                }
            }
        }

        [HarmonyPatch(typeof(ObjectPooler), "AddObject")]
        [HarmonyPrefix]
        static bool AddObjectPrefix(string tag, GameObject GO, int amt, bool exp)
        {
            if (tagList.Contains(tag))
            {
                TryAddHealthBarComponent(GO);
            }

            return true;
        }

        private static void TryAddHealthBarComponent(GameObject go)
        {
            if (go.GetComponent<HealthBar>() is null)
            {
                Log.LogDebug("Adding HealthBar to: " + go.tag);
                go.AddComponent<HealthBar>();
            }
        }
    }
}