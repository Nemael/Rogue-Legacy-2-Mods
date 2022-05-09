using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using Wob_Common;

namespace Nemael_MoreChestDrops {
    [BepInPlugin( "Nemael.ActivateZoneTeleporters", "Activate Zone Teleporters", "1.0" )]
    public class BepInExPlugin : BaseUnityPlugin {
        // Main method that kicks everything off
        private void Awake() {
            // Set up the logger and basic config items
            WobPlugin.Initialise( this, this.Logger );
            // Apply the patches if the mod is enabled
            WobPlugin.Patch();
        }

        // Patch for the method that manage the animation of a chest opening
        //[HarmonyPatch(typeof(BiomeRuleManager), "OnWorldCreationComplete")]
        //static class BiomeRuleManager_OnWorldCreationComplete_Patch
        [HarmonyPatch(typeof(PizzaGirlPropController), "HasEventDialogue")]
        static class PizzaGirlPropController_HasEventDialogue_Patch
        {
            static void Postfix(PizzaGirlPropController __instance)
            {
                WobPlugin.Log("| World creation complete");
                //None,
                //Editor
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Castle, true);
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Cave, true);
                //CaveMiddle
                //CaveBottom
                //SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Dragon, true); NOT THIS
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Forest, true);
                //ForestTop
                //ForestBottom
                //SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Garden, true); NOT THIS
                //SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.DriftHouse, true); NOT THIS
                //HubTown
                //SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Lake, true); NOT THIS
                //Lineage
                //Spawn
                //Special
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Stone, true);
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Study, true);
                //SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Sunken, true) NOT THIS;
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Tower, true);
                //SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.TowerExterior, true); NOT THIS
                //Town
                //Tutorial
                //Arena
                //Heirloom
                //Any

            }
        }
    }
}