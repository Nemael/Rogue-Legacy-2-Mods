using BepInEx;
using HarmonyLib;
using RL_Windows;
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

        [HarmonyPatch()]
        static class RelicRoomPropController_ChooseRelic_Patch
        {
            static MethodInfo TargetMethod()
            {
                // Find the nested class of 'RelicRoomPropController' that 'ChooseRelic' implicitly created
                System.Type type = AccessTools.FirstInner(typeof(RelicRoomPropController), t => t.Name.Contains("<ChooseRelic>d__"));
                // Find the 'MoveNext' method on the nested class
                return AccessTools.FirstMethod(type, method => method.Name.Contains("MoveNext"));
            }
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                // Iterate through the instruction codes
                for (int i = 0; i < codes.Count; i++)
                {
                    // No-op the instructions between these two ids, these are the instructions that starts the window opening animation
                    if (i >= 62 && i <= 91)
                    {
                        // Set the operand to the new value
                        codes[i].opcode = OpCodes.Nop;
                    }
                }

                // Return the modified instructions to complete the patch
                return codes.AsEnumerable();
            }
        }
    }
}