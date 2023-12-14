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


        [HarmonyPatch(typeof(RoundManager), "SpawnRandomOutsideEnemy")]
        static void Prefix(RoundManager __instance, GameObject[] spawnPoints, float timeUpToCurrentHour)
        {
            Debug.LogWarning("--------------------INSIDE THE BEES AND SHIT");
            Debug.LogAssertion("--------------------INSIDE THE BEES AND SHIT");
            Debug.LogError("--------------------INSIDE THE BEES AND SHIT");
            
            // Access private members using reflection
            var currentLevelField = AccessTools.Field(typeof(RoundManager), "currentLevel");

            if (currentLevelField != null)
            {
                var currentLevel = (SelectableLevel)currentLevelField.GetValue(__instance);

                // Check if currentLevel is not null
                if (currentLevel != null)
                {
                    // Access the OutsideEnemies list
                    var outsideEnemies = currentLevel.OutsideEnemies;

                    // Log or perform actions with the OutsideEnemies list
                    Debug.LogAssertion($"--------------------Number of OutsideEnemies: {outsideEnemies.Count}");

                    // Iterate through the OutsideEnemies list
                    foreach (var outsideEnemy in outsideEnemies)
                    {
                        // Access properties or fields of each OutsideEnemy object
                        Debug.LogAssertion($"--------------------Enemy Type: {outsideEnemy.enemyType}");
                    }
                }
            }
        }


        



















        // Working Mods
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