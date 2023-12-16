using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreBees.Patches
{
    [HarmonyPatch]
    class moreBatts : moreBees
    {

        [HarmonyPatch(typeof(GrabbableObject), "Update")]
        static void PostFix(ref Battery ___insertedBattery)
        {
            ___insertedBattery.charge = 1f;
            ___insertedBattery.empty = false;

        }
    }
}
