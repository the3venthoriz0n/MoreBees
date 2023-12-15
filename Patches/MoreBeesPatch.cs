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
        [HarmonyPatch(typeof(RoundManager), "SpawnDaytimeEnemiesOutside")]
        [HarmonyPrefix]
        static void chooseDaytimeEnemies(RoundManager __instance)
        {
            var currentLevelField = AccessTools.Field(typeof(RoundManager), "currentLevel");

            if (currentLevelField != null)
            {
                var currentLevel = (SelectableLevel)currentLevelField.GetValue(__instance);

                    if (currentLevel != null)
                    {
                        currentLevel.maxDaytimeEnemyPowerCount = 100;
                        // currentLevel.daytimeEnemySpawnChanceThroughDay = 100;

                        var daytimeEnemies = currentLevel.DaytimeEnemies;
                        Debug.LogWarning($"--------------------Number of DaytimeEnemies: {daytimeEnemies.Count}");

                        // Iterate through the daytimeEnemies list
                        foreach (var daytimeEnemy in daytimeEnemies)
                        {
                            // Access properties or fields of each OutsideEnemy object
                            // Debug.LogWarning($"--------------------Enemy Type: {daytimeEnemy.enemyType.name}");
                            // Debug.LogWarning($"--------------------Enemy Rarity: {daytimeEnemy.rarity}");

                            // ONLY SPAWN BEES, REJECT OTHER DAYTIME SPAWNS
                            if (daytimeEnemy.enemyType.name == "RedLocustBees")
                            {
                                daytimeEnemy.rarity = 100;
                                // Debug.LogWarning($"RARITY SET RARITY SET TO: {daytimeEnemy.rarity}");

                            }
                            else
                            {
                                daytimeEnemy.rarity = 0;
                                // Debug.LogWarning($"NOT A BEE, RARITY SET RARITY SET TO: {daytimeEnemy.rarity}");
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

        // Spawn more daytime enemies
        [HarmonyPatch(typeof(RoundManager), "SpawnDaytimeEnemiesOutside")]
        [HarmonyPostfix]
        static void moreDaytimeEnemies(RoundManager __instance)
        {
            float num = 100; // timeScript.lengthOfHours * (float)currentHour;
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("OutsideAINode");

            // Iterate over each EnemyType in the list
            foreach (SpawnableEnemyWithRarity enemy in __instance.currentLevel.DaytimeEnemies)
            {
                // Access MaxCount directly on the current EnemyType instance
                int numberOfBees = enemy.enemyType.MaxCount = 1000;
                // Debug.LogWarning($"BEES SET TO: {numberOfBees}");

                System.Reflection.MethodInfo spawnRandomDaytimeEnemyMethod = AccessTools.Method(typeof(RoundManager), "SpawnRandomDaytimeEnemy", new System.Type[] { typeof(GameObject[]), typeof(float) });

                if (spawnRandomDaytimeEnemyMethod != null)
                {
                    // Execute additional code after the 'for' loop
                    for (int i = 0; i < numberOfBees; i++)
                    {
                        GameObject gameObject = spawnRandomDaytimeEnemyMethod.Invoke(__instance, new object[] { spawnPoints, num }) as GameObject;
                        if (gameObject != null)
                        {
                            Debug.LogWarning("BUZZ");
                            __instance.SpawnedEnemies.Add(gameObject.GetComponent<EnemyAI>());
                            gameObject.GetComponent<EnemyAI>().enemyType.numberSpawned++;
                            continue;
                        }
                        break;
                    }
                }
            }
        }


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