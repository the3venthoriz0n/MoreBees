using GameNetcodeStuff;
using HarmonyLib;

namespace MoreBees.Patches
{
    [HarmonyPatch]
    public class MoreSprint
    {
        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void MoreSprintPatch(ref float ___sprintMeter)
        {
            ___sprintMeter = 1f;

        }
    }
}

