using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;


namespace MoreBees
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class MoreBeesBase : BaseUnityPlugin

        // TODO make this work with separate files
    {
        private const string modGUID = "the3venthoriz0n.MoreBees";
        private const string modName = "More Bees";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        public static ManualLogSource Log = new ManualLogSource(modName);
        private static MoreBeesBase Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            harmony.PatchAll();
            Logger.LogInfo($"PluginName: {modName}, VersionString: {modVersion} is loaded.");
            Log = Logger;

        }
    }

}
