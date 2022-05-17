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
    [BepInPlugin("Nemael.SkipRelicObtainedCutscene", "SkipRelicObtainedCutscene", "1.0")]
    public class BepInExPlugin : BaseUnityPlugin
    {
        // Main method that kicks everything off
        private void Awake()
        {
            // Set up the logger and basic config items
            WobPlugin.Initialise(this, this.Logger);
            // Apply the patches if the mod is enabled
            WobPlugin.Patch();
        }

        // Patch for the method that display a drop obtained
        [HarmonyPatch(typeof(ItemDropManager), "Internal_DropSpecialItem")]
        static class ItemDropManager_Internal_DropSpecialItem_Patch
        {
            // Change the starting level in the method, which sets whether the labour cost box is displayed
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                // Put the instructions into a list for easier manipulation
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                WobPlugin.Log("Searching opcodes");
                // Iterate through the instruction codes
                for (int i = 0; i < codes.Count; i++)
                {
                    // Search for instruction 'ldc.i4.s' which pushes an int8 onto the stack
                    if (codes[i].opcode == OpCodes.Brfalse_S)
                    {
                        WobPlugin.Log("Found matching opcode at " + i + ": " + codes[i].ToString());
                        // Check if the operand is correct for level 18
                        if ((string)codes[i].operand == "IL_01CF")
                        {
                            WobPlugin.Log("Correct operand - patching");
                            // Set the operand to the new value from the config file
                            codes[i].opcode = OpCodes.Br_S;
                            //codes[i].operand = configStartLevel.Value;
                        }
                    }
                }
                // Return the modified instructions to complete the patch
                return codes.AsEnumerable();
            }
        }
    }
}