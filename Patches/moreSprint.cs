using GameNetcodeStuff;
using HarmonyLib;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreBees.Patches
{
    [HarmonyPatch]
    public class MoreSprint
    {
        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void MoreSprintPatch(ref float ___sprintMeter, PlayerControllerB __instance)
        {

            if (__instance.currentlyHeldObjectServer != null)
            {
                var obj_name = __instance.currentlyHeldObjectServer.name;
                // Debug.LogWarning("Obj Name: " + obj_name); // Print currently held object

                // Check if the currently held object is a RedLocustHive
                if (obj_name == "RedLocustHive(Clone)")
                {
                    // Modify sprint behavior while holding the hive
                    ___sprintMeter = 1f;
                }
            }
        }
    }
}
 