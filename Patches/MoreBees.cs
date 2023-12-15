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
    class moreBeesPatch
    {
        private static FieldInfo currentLevelField;

        static moreBeesPatch()
        {
            // Initialize currentLevelField in the static constructor
            currentLevelField = AccessTools.Field(typeof(RoundManager), "currentLevel");

        }

        [HarmonyPatch(typeof(RoundManager), "SpawnDaytimeEnemiesOutside")]
        [HarmonyPrefix]
        static void chooseDaytimeEnemies(RoundManager __instance)
        {

            if (currentLevelField != null)
            {
                var currentLevel = (SelectableLevel)currentLevelField.GetValue(__instance);

                    if (currentLevel != null)
                    {
                        currentLevel.maxDaytimeEnemyPowerCount = 200; // this controls the number of enemies (bees)
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

            if (currentLevelField != null)
            {
                var currentLevel = (SelectableLevel)currentLevelField.GetValue(__instance);

                // Iterate over each EnemyType in the list
                foreach (SpawnableEnemyWithRarity enemy in __instance.currentLevel.DaytimeEnemies)
                {
                    // Access MaxCount directly on the current EnemyType instance
                    int numberOfBees = enemy.enemyType.MaxCount = currentLevel.maxDaytimeEnemyPowerCount; // controlled above ^^
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
        }
    }
}