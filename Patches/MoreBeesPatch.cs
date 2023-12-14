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
            // Access the private field "SpawnedEnemies" using reflection
            var spawnedEnemiesField = typeof(RoundManager).GetField("SpawnedEnemies", BindingFlags.NonPublic | BindingFlags.Instance);
            var spawnedEnemiesList = (List<EnemyAI>)spawnedEnemiesField.GetValue(__instance);

            // Modify the enemyType for each spawned enemy
            foreach (var spawnedEnemy in spawnedEnemiesList)
            {
                // Access the private field "enemyType" using reflection
                var enemyTypeField = typeof(EnemyAI).GetField("enemyType", BindingFlags.NonPublic | BindingFlags.Instance);
                var enemyType = (EnemyType)enemyTypeField.GetValue(spawnedEnemy);

                // Modify the enemyType as needed
                // For example, setting a new value to the "powerLevel" property
                enemyType.PowerLevel = 100;
                enemyType.enemyName = "RedLocustBees";

                // Update the "enemyType" field with the modified value
                enemyTypeField.SetValue(spawnedEnemy, enemyType);
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