using HarmonyLib;

namespace MoreBees.Patches
{
    public class MoreTentacles
    {

        [HarmonyPatch(typeof(DepositItemsDesk), "SetCompanyMood")]
        static void PostFix(CompanyMood mood, ref CompanyMood ___currentMood, ref float ___noiseBehindWallVolume)
        {
            ___currentMood.desiresSilence = true;
            ___currentMood.sensitivity = 10f;
            ___currentMood.irritability = 10f;
            ___currentMood.judgementSpeed = 10f;
            ___currentMood.timeToWaitBeforeGrabbingItem = 1f;
            ___currentMood.startingPatience = 0f;
            ___noiseBehindWallVolume = 10f;
        }
    }
}
