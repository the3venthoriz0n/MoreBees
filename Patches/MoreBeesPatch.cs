using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx;
using BepInEx.Logging;
using Unity.Netcode;
using System.ComponentModel;
using System.Xml.Linq;
using DunGen.Graph;
using System.Reflection.Emit;
using System.Reflection;
using GameNetcodeStuff;

namespace MoreBees.Patches
{

    [HarmonyPatch]
    class MoreBeesPatch
    {
         
        public enum EnemyType
        {
            RedLocustBees, // Add more enemy types as needed
        }


        // Transpiler method
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);

            for (int i = 0; i < instructionList.Count; i++)
            {
                if (instructionList[i].opcode == OpCodes.Stfld && instructionList[i].operand.ToString().Contains("enemyType"))
                {
                    // Replace the instruction that sets the value of enemyType
                    instructionList[i].operand = AccessTools.Field(typeof(SpawnableEnemyWithRarity), "enemyType");
                }
            }

            return instructionList.AsEnumerable();
        }


        //[HarmonyPatch(typeof(SpawnableEnemyWithRarity), nameof(SpawnableEnemyWithRarity.enemyType))]

        //static bool Prefix(ref EnemyType value)
        //{
        //    // Perform your validation here
        //    if (value != EnemyType.RedLocustBees)
        //    {
        //        Debug.LogError("Invalid enemy type detected!");
        //        value = EnemyType.RedLocustBees; // Set a default value or handle the error as needed
        //    }

        //    // Allow the original method to proceed
        //    return true;
        //}


        // Not working as far as I can tell
        //[HarmonyPatch(typeof(SpawnableEnemyWithRarity))]
        //[HarmonyPatch(MethodType.Constructor)]
        //static void Postfix(ref SpawnableEnemyWithRarity __instance)
        //{
        //    // Set enemyType to RedLocustBees
        //    __instance.enemyType.enemyName = "RedLocustBees";

        //}


        //[HarmonyPatch(typeof(SelectableLevel))]
        //[HarmonyPatch(MethodType.Constructor)]
        ////[HarmonyPatch(new Type[] { typeof(int) })] // Add constructor parameters if needed
        //static void Postfix(SelectableLevel __instance)
        //{
        //    // Your code here to modify the instance of SelectableLevel
        //    __instance.maxEnemyPowerCount = 100;
        //    __instance.maxOutsideEnemyPowerCount = 100;
        //    __instance.maxDaytimeEnemyPowerCount = 100;
        //    __instance.spawnProbabilityRange = 10f;
        //    __instance.daytimeEnemiesProbabilityRange = 100f;


        //    __instance.minTotalScrapValue = 9000;
        //}


        [HarmonyPatch(typeof(DepositItemsDesk), "SetCompanyMood")]
        [HarmonyPostfix]
        static void moreTentacles(CompanyMood mood, ref CompanyMood ___currentMood, ref float ___noiseBehindWallVolume)
        {
           
            ___currentMood.desiresSilence = true;
            ___currentMood.sensitivity = 10f;
            ___currentMood.irritability = 10f;
            ___currentMood.judgementSpeed = 10f;
            ___currentMood.timeToWaitBeforeGrabbingItem = 1f;
            ___currentMood.startingPatience = 0f;

            ___noiseBehindWallVolume = 10f;

        }


        [HarmonyPatch(typeof(GrabbableObject), "Update")]
        [HarmonyPostfix]
        static void moreBatts(ref Battery ___insertedBattery)
        {
            ___insertedBattery.charge = 1f;
            ___insertedBattery.empty = false;

        }


        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void moreSprint(ref float ___sprintMeter)
        {
            ___sprintMeter = 1f;

        }



    }


}