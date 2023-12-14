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


        [HarmonyPatch(typeof(RoundManager), "SpawnEnemiesOutside")]
        static void Postfix(ref RoundManager __instance)
        {
            
            Debug.LogWarning("-------------------------INSIDE BEES--------------------------");
            // Access the private field "SpawnedEnemies" using reflection
            var spawnedEnemiesField = typeof(RoundManager).GetField("SpawnedEnemies");

            // Check if the field is found and not null
            if (spawnedEnemiesField == null)
            {
                Debug.LogError("SpawnedEnemies field not found in RoundManager.");
                return;
            }
            // Ensure that the field type is List<EnemyAI>
            if (spawnedEnemiesField.FieldType != typeof(List<EnemyAI>))
            {
                Debug.LogError("SpawnedEnemies field is not of type List<EnemyAI>.");
                return;
            }


            var spawnedEnemiesList = (List<EnemyAI>)spawnedEnemiesField.GetValue(__instance);

            // Check if the list is not null
            if (spawnedEnemiesList == null)
            {
                Debug.LogError("SpawnedEnemies list is null in RoundManager.");
                return;
            }

            // Modify the enemyType for each spawned enemy. List of AI Objects
            foreach (var spawnedEnemy in spawnedEnemiesList)
            {
                // Access the private field "enemyType" using reflection
                var enemyTypeField = typeof(EnemyAI).GetField("enemyType");

                // Check if the field is found and not null
                if (enemyTypeField == null)
                {
                    
                    Debug.LogError("enemyType field not found in EnemyAI.");
                    continue; // Skip to the next spawned enemy
                }

                var enemyType = (EnemyType)enemyTypeField.GetValue(spawnedEnemy);
                
                // Check if the enemyType is not null
                if (enemyType == null)
                {
                    Debug.LogError("enemyType is null in EnemyAI.");
                    continue; // Skip to the next spawned enemy
                }

                Debug.LogWarning("OLD OLD OLD OLD OLD OLD  ENEMY: " + spawnedEnemy);
                // Modify the enemyType as needed
                // For example, setting a new value to the "powerLevel" property
                enemyType.PowerLevel = 100;
                enemyType.enemyName = "RedLocustBees";
                // enemyType.isOutsideEnemy = true;
                enemyType.MaxCount = 200;

                // Update the "enemyType" field with the modified value
                // enemyTypeField.SetValue(spawnedEnemy, enemyType);
                enemyTypeField.SetValue(spawnedEnemy, enemyType);

                Debug.LogWarning("NEW NEW NEW NEW NEW NEW NEW ENEMY: " + spawnedEnemy);
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