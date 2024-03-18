using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;


namespace MoreBees
{

    public static class PluginInfo
    {
        public const string modName = "MoreBees";
        public const string modVersion = "1.0.1";
        public const string modGUID = "the3venthoriz0n.MoreBees";
    }


    [BepInPlugin(PluginInfo.modGUID, PluginInfo.modName, PluginInfo.modVersion)]
    public class Plugin : BaseUnityPlugin
    {

        public static ManualLogSource StaticLogger;


        public void Awake()
        {
            StaticLogger = Logger;
            Harmony harmony = new Harmony(PluginInfo.modGUID);

            try
            {
                harmony.PatchAll();
                StaticLogger.LogInfo($"{PluginInfo.modName} {PluginInfo.modVersion} is loaded.");
            }
            catch (Exception e)
            {
                StaticLogger.LogError("Failed to patch: " + e);
            }

        }
    }

}
