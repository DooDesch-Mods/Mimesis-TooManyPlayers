using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace TooManyPlayers.Patches
{
    [HarmonyPatch]
    internal static class RoomCardPatches
    {
        private static MethodBase TargetMethod() =>
            AccessTools.Method(AccessTools.TypeByName("UiPrefab_RoomCard"), "SetRoomData") ??
            throw new MissingMethodException("UiPrefab_RoomCard.SetRoomData");

        private static string GetSuffix(PublicRoomListData data) =>
            "/" + PublicRoomListPatches.GetLobbyLimit(data.lobbyID);

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode != OpCodes.Ldstr || !Equals(codes[i].operand, "/4")) continue;
                codes[i].opcode = OpCodes.Ldarg_1;
                codes[i].operand = null;
                codes.Insert(i + 1, new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(RoomCardPatches), nameof(GetSuffix))));
                return codes;
            }
            throw new InvalidOperationException("[TooManyPlayers] Public room capacity label was not found.");
        }
    }
}
