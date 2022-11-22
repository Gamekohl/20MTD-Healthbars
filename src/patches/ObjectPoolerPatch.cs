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
        [HarmonyPatch(typeof(ObjectPooler), "Awake")]
        [HarmonyPrefix]
        static void AwakePrefix(ref ObjectPooler __instance)
        {
            foreach (ObjectPoolItem objectPoolItem in __instance.itemsToPool)
            {
                if(objectPoolItem.objectToPool.GetComponent<Health>() != null) {
                    TryAddHealthBarComponent(objectPoolItem.objectToPool.gameObject);
                }
            }
        }

        [HarmonyPatch(typeof(ObjectPooler), "AddObject")]
        [HarmonyPrefix]
        static bool AddObjectPrefix(string tag, GameObject GO, int amt, bool exp)
        {
            if(GO.GetComponent<Health>() != null) {
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