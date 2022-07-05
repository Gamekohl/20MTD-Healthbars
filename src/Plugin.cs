using System;
using BepInEx;
using HarmonyLib;
using BepInEx.Logging;

namespace Healthbars
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        private void Awake()
        {
            // Plugin startup logic
            Log = base.Logger;

            try
            {
                Harmony.CreateAndPatchAll(typeof(Healthbars.ObjectPoolerPatch));
            }
            catch (Exception e)
            {
                Log.LogError(e.GetType() + " " + e.Message);
            }

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
