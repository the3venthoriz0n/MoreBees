using GameNetcodeStuff;
using HarmonyLib;

namespace MoreBees.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB), "Update")]
    public class MoreSprint
    {
        
        static void PostFix(ref float ___sprintMeter)
        {
            ___sprintMeter = 1f;

        }
    }
}
