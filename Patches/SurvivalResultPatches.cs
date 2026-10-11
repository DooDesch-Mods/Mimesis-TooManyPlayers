using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Bifrost.ConstEnum;
using HarmonyLib;

namespace TooManyPlayers.Patches
{
    [HarmonyPatch]
    internal static class SurvivalResultPatches
    {
        private static MethodBase TargetMethod() =>
            AccessTools.Method(AccessTools.TypeByName("UIPrefab_SurvivalResult"), "PatchParameter") ??
            throw new MissingMethodException("UIPrefab_SurvivalResult.PatchParameter");

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            var cap = AccessTools.Field(typeof(DataConsts), "C_MaxPlayerCount");
            int matches = 0;
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode != OpCodes.Ldfld || !Equals(codes[i].operand, cap)) continue;
                // The prefab has four rows regardless of the session cap. Preserve the
                // original player count so following scrap parameters keep their offsets.
                codes[i].opcode = OpCodes.Pop;
                codes[i].operand = null;
                codes.Insert(++i, new CodeInstruction(OpCodes.Ldc_I4_4));
                matches++;
            }
            if (matches != 1)
                throw new InvalidOperationException($"[TooManyPlayers] Survival result UI cap matches: {matches}, required: 1.");
            return codes;
        }
    }
}
