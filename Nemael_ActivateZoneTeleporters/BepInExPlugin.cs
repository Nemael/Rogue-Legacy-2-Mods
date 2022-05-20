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

        [HarmonyPatch(typeof(BiomeRuleManager), "OnWorldCreationComplete")]
        static class BiomeRuleManager_OnWorldCreationComplete_Patch
        //static class PizzaGirlPropController_HasEventDialogue_Patch
        {
            //Executes this code when generating the level
            static void Postfix()
            {
                //List of biome types:
                //None,
                //Editor
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Castle, true);
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Cave, true);
                //CaveMiddle
                //CaveBottom
                //Dragon
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Forest, true);
                //ForestTop
                //ForestBottom
                //Garden
                //DriftHouse
                //HubTown
                //Lake
                //Lineage
                //Spawn
                //Special
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Stone, true);
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Study, true);
                //Sunken
                SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(BiomeType.Tower, true);
                //TowerExterior
                //Town
                //Tutorial
                //Arena
                //Heirloom
                //Any

            }
        }
    }
}