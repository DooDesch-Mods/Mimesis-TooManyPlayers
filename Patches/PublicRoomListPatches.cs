using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MelonLoader;
using Steamworks;

namespace TooManyPlayers.Patches
{
    [HarmonyPatch]
    internal static class PublicRoomListPatches
    {
        private static bool _warnedMissingLimit;
        private static MethodBase TargetMethod() =>
            AccessTools.Method(AccessTools.TypeByName("UIPrefab_PublicRoomList"), "SetRoomList") ??
            throw new MissingMethodException("UIPrefab_PublicRoomList.SetRoomList");

        internal static int GetLobbyLimit(CSteamID lobby)
        {
            int limit = SteamMatchmaking.GetLobbyMemberLimit(lobby);
            if (limit > 0) return limit;
            if (!_warnedMissingLimit)
            {
                _warnedMissingLimit = true;
                MelonLogger.Warning("[TooManyPlayers] Public lobby member limit unavailable; using the vanilla limit of 4.");
            }
            return 4;
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            CodeInstruction lobbyLoad = null;
            for (int i = 1; i < codes.Count; i++)
            {
                if (codes[i].operand is MethodInfo method && method.DeclaringType == typeof(SteamMatchmaking) &&
                    method.Name == "GetNumLobbyMembers")
                {
                    lobbyLoad = new CodeInstruction(codes[i - 1].opcode, codes[i - 1].operand);
                    break;
                }
            }
            if (lobbyLoad == null)
                throw new InvalidOperationException("[TooManyPlayers] Public room lobby ID load was not found.");
            for (int i = 1; i < codes.Count; i++)
            {
                if (!codes[i].LoadsConstant(4) || codes[i - 1].opcode != OpCodes.Ldfld ||
                    !(codes[i - 1].operand is FieldInfo field) || field.Name != "PlayerCount") continue;
                // Use each listed host's Steam limit, independent of this client's preferences.
                codes[i].opcode = lobbyLoad.opcode;
                codes[i].operand = lobbyLoad.operand;
                codes.Insert(i + 1, new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(PublicRoomListPatches), nameof(GetLobbyLimit))));
                return codes;
            }
            throw new InvalidOperationException("[TooManyPlayers] Public room player limit comparison was not found.");
        }
    }
}
