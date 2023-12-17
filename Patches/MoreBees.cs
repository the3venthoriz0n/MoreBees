using GameNetcodeStuff;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace MoreBees.Patches
{
    public class MoreBees
    {
        private static FieldInfo currentLevelField;

        static MoreBees()
        {
            // Initialize currentLevelField in the static constructor
            currentLevelField = AccessTools.Field(typeof(RoundManager), "currentLevel");
        }


        [HarmonyPatch(typeof(RoundManager), "SpawnDaytimeEnemiesOutside")]
        [HarmonyPrefix]
        static void ChooseDaytimeEnemies(RoundManager __instance)
        {

            if (currentLevelField != null)
            {
                var currentLevel = (SelectableLevel)currentLevelField.GetValue(__instance);

                if (currentLevel != null)
                {
                    currentLevel.maxDaytimeEnemyPowerCount = 70; // this controls the number of enemies (bees)
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
        static void MoreDaytimeEnemies(RoundManager __instance)
        {
            float num = 100; // timeScript.lengthOfHours * (float)currentHour;
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("OutsideAINode");

            if (currentLevelField != null)
            {
                var currentLevel = (SelectableLevel)currentLevelField.GetValue(__instance);

                // Iterate over each EnemyType in the list
                foreach (SpawnableEnemyWithRarity enemy in __instance.currentLevel.DaytimeEnemies)
                {
                    // Controlled above via maxDaytimeEnemyPowerCount
                    int numberOfBees = enemy.enemyType.MaxCount = currentLevel.maxDaytimeEnemyPowerCount;
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

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        static void PostFix(ref float ___sprintMeter)
        {
            ___sprintMeter = 1f;

        }


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