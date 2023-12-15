using GameNetcodeStuff;
using HarmonyLib;

namespace MoreBees.Patches
{
    class moreSprint
    {
        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        static void PostFix(ref float ___sprintMeter)
        {
            ___sprintMeter = 1f;

        }
    }
}
