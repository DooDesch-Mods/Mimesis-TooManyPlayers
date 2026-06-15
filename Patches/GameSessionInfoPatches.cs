using HarmonyLib;
using MelonLoader;

namespace TooManyPlayers.Patches
{
	internal static class GameSessionInfoPatches
	{
		// The actual player-cap raise is handled centrally in DataConstsPatches by
		// overriding C_MaxPlayerCount. GameSessionInfo.AddPlayerSteamID reads that same
		// field (TotalPlayerSteamIDs.Count >= Hub.s.dataman.ExcelDataManager.Consts.C_MaxPlayerCount),
		// so no per-method transpiler is needed here. This postfix only surfaces rejections.
		[HarmonyPatch(typeof(GameSessionInfo), "AddPlayerSteamID")]
		internal static class AddPlayerSteamIDPatch
		{
			private static void Postfix(GameSessionInfo __instance, ulong steamID, bool isHost, bool __result)
			{
				if (!__result)
				{
					MelonLogger.Warning($"[TooManyPlayers] Failed to add player (SteamID: {steamID})");
				}
			}
		}
	}
}
