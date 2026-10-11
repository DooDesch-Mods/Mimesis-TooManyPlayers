using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace TooManyPlayers.Patches
{
    [HarmonyPatch(typeof(IVroom), MethodType.Constructor,
        typeof(VRoomManager), typeof(long), typeof(IVRoomProperty), typeof(OnCreateRoomDelegate))]
    internal static class PlayerContainerPatches
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i + 1 < codes.Count; i++)
            {
                if (codes[i + 1].opcode == OpCodes.Newobj &&
                    codes[i + 1].operand is ConstructorInfo constructor &&
                    constructor.DeclaringType == typeof(VActorDict<int, VPlayer>) &&
                    codes[i].LoadsConstant(10))
                {
                    // Admission still checks the effective host cap in CanEnterChannel.
                    // The container must also fit hosts with a larger cap received later.
                    codes[i].opcode = OpCodes.Ldc_I4;
                    codes[i].operand = 250;
                    return codes;
                }
            }
            throw new InvalidOperationException("[TooManyPlayers] IVroom player container capacity argument was not found.");
        }
    }
}
