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
    [BepInPlugin( "Nemael.MoreChestDrops", "More Chest Drops", "1.0" )]
    public class BepInExPlugin : BaseUnityPlugin {
        // Main method that kicks everything off
        private void Awake() {
            // Set up the logger and basic config items
            WobPlugin.Initialise( this, this.Logger );
            // Apply the patches if the mod is enabled
            WobPlugin.Patch();
        }

        // Patch for the method that manage the animation of a chest opening
        [HarmonyPatch(typeof(ChestObj), "OpenChestAnimCoroutine")]
        static class ChestObj_OpenChestAnimCoroutine_Patch
        {
            static void Postfix(ChestObj __instance)
            {
                if (__instance.ChestType != ChestType.Bronze)
                {
                    WobPlugin.Log("| Chest isn't bronze or golden");
                    //Uses reflection to obtain the item that we want to drop
                    //For the method to be found, we need to specify the types of all the arguments of the method, and give it all the argument
                    //We need to specify the type of every argument because they will be converted to object when put in an array.
                    //And then calls the method that is used to drop the item
                    object specialItemDropObj;
                    //Equipment
                    specialItemDropObj = Traverse.Create(__instance).Method("CalculateSpecialItemDropObj", SpecialItemType.Blueprint ).GetValue(new object[] { SpecialItemType.Blueprint });
                    Traverse.Create(__instance).Method("DropRewardFromRegularChest", new System.Type[] { typeof(SpecialItemType), typeof(ISpecialItemDrop), typeof(int) }, new object[] { SpecialItemType.Blueprint, (ISpecialItemDrop)specialItemDropObj, __instance.Level }).GetValue(new object[] { SpecialItemType.Blueprint, specialItemDropObj, __instance.Level });
                    //Rune
                    specialItemDropObj = Traverse.Create(__instance).Method("CalculateSpecialItemDropObj", SpecialItemType.Rune).GetValue(new object[] { SpecialItemType.Rune });
                    Traverse.Create(__instance).Method("DropRewardFromRegularChest", new System.Type[] { typeof(SpecialItemType), typeof(ISpecialItemDrop), typeof(int) }, new object[] { SpecialItemType.Blueprint, (ISpecialItemDrop)specialItemDropObj, __instance.Level }).GetValue(new object[] { SpecialItemType.Blueprint, specialItemDropObj, __instance.Level });
                    //Challenge emotionnal connection
                    specialItemDropObj = Traverse.Create(__instance).Method("CalculateSpecialItemDropObj", SpecialItemType.Challenge).GetValue(new object[] { SpecialItemType.Challenge });
                    Traverse.Create(__instance).Method("DropRewardFromRegularChest", new System.Type[] { typeof(SpecialItemType), typeof(ISpecialItemDrop), typeof(int) }, new object[] { SpecialItemType.Blueprint, (ISpecialItemDrop)specialItemDropObj, __instance.Level }).GetValue(new object[] { SpecialItemType.Blueprint, specialItemDropObj, __instance.Level });

                    //Drops more equipment and rune ore
                    //We need to use reflection to obtain the drop coordinates of the ore
                    Vector3 my_drop_position = (Vector3)Traverse.Create(__instance).Field("m_dropPosition").GetValue();
                    int amount_equipment_ore = my_GetOreDropAmount(ItemDropType.EquipmentOre, __instance.Level);
                    int amount_rune_ore = my_GetOreDropAmount(ItemDropType.RuneOre, __instance.Level);
                    //Multiplies the ore obtained
                    amount_equipment_ore = amount_equipment_ore * 4;
                    amount_rune_ore = amount_rune_ore * 4;
                    //This static function drops the ore at our request
                    ItemDropManager.DropItem(ItemDropType.EquipmentOre, 200, my_drop_position, true, true, true);
                    ItemDropManager.DropItem(ItemDropType.RuneOre, 200, my_drop_position, true, true, true);
                    WobPlugin.Log("| Amount of ore dropped: EQUIPMENT " + amount_equipment_ore.ToString() + " RUNE " + amount_rune_ore.ToString());
                }
                else
                {
                    WobPlugin.Log("| Bronze or golden chest");
                }
            }

            public static int my_GetOreDropAmount(ItemDropType oreDropType, int chestLevel)
            {
                float num = (float)((oreDropType == ItemDropType.RuneOre) ? 150 : 175);
                float num2 = (oreDropType == ItemDropType.RuneOre) ? 2f : 5f;
                return (int)(num + (float)chestLevel * num2);
            }
        }
    }
}