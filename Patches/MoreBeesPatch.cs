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
        // TODO Spawn more daytime enemies
        // TODO Choose only BEES, Increase probability / raise rarity

        [HarmonyPatch(typeof(RoundManager), "SpawnDaytimeEnemiesOutside")]
        static void Prefix(RoundManager __instance)
        {

            Debug.LogWarning("--------------------INSIDE SPAWN DAYTIME ENEMIES OUTSIDE!!!!!!!!!!!!!!!!!!!");

            // Access private members using reflection
            var currentLevelField = AccessTools.Field(typeof(RoundManager), "currentLevel");

            if (currentLevelField != null)
            {
                var currentLevel = (SelectableLevel)currentLevelField.GetValue(__instance);
                var spawnProbabilitiesField = AccessTools.Field(typeof(RoundManager), "SpawnProbabilities");


                if (spawnProbabilitiesField != null)
                {
                    var spawnProbabilities = (List<int>)spawnProbabilitiesField.GetValue(__instance);
                    
                    foreach(var spawnProb in spawnProbabilities)
                    {
                        Debug.LogWarning($"--------------------Spawn Prob: {spawnProb}");
                    }

                }


                    // Check if currentLevel is not null
                    if (currentLevel != null)
                    {
                        currentLevel.maxDaytimeEnemyPowerCount = 100;
                        // currentLevel.daytimeEnemySpawnChanceThroughDay = 100;


                        // Access the DaytimeEnemies list
                        var daytimeEnemies = currentLevel.DaytimeEnemies;

                        Debug.LogWarning($"--------------------Number of DaytimeEnemies: {daytimeEnemies.Count}");

                        // Iterate through the daytimeEnemies list
                        foreach (var daytimeEnemy in daytimeEnemies)
                        {
                            // Access properties or fields of each OutsideEnemy object
                            Debug.LogWarning($"--------------------Enemy Type: {daytimeEnemy.enemyType.name}");
                            Debug.LogWarning($"--------------------Enemy Rarity: {daytimeEnemy.rarity}");


                            if (daytimeEnemy.enemyType.name == "RedLocustBees")
                            {
                                daytimeEnemy.rarity = 100;
                                Debug.LogWarning($"RARITY SET RARITY SET TO: {daytimeEnemy.rarity}");
                            }
                            else
                            {
                                daytimeEnemy.rarity = 0;
                                Debug.LogWarning($"NOT A BEE, RARITY SET RARITY SET TO: {daytimeEnemy.rarity}");
                        }

                        }

             
                }
                else
                {
                    Debug.LogError("currentLevel is NULL");
                }
            }
            else
            {
                Debug.LogError("CurrentLevelField is NULL");
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