using HarmonyLib;
using MelonLoader;
using TooManyPlayers.Config;
using System.Reflection;
using Bifrost.Const;
using Bifrost.ConstEnum;

namespace TooManyPlayers.Patches
{
	// Source-of-truth patch for the player cap.
	//
	// Mimesis 0.3.0 moved the hardcoded max-player literal (4) into a data-driven
	// config field: Bifrost.ConstEnum.DataConsts.C_MaxPlayerCount, surfaced game-wide
	// via Hub.s.dataman.ExcelDataManager.Consts.C_MaxPlayerCount. Every player-limit
	// check now reads that single field:
	//   VRoomManager.EnterWaitingRoom / EnterMaintenenceRoom (GetMemberCount() >= C_MaxPlayerCount)
	//   IVroom.CanEnterChannel (_vPlayerDict.Count >= C_MaxPlayerCount)
	//   GameSessionInfo.AddPlayerSteamID (TotalPlayerSteamIDs.Count >= C_MaxPlayerCount)
	//
	// Overriding this one field after DataConsts is constructed raises the cap
	// everywhere at once, replacing the old per-method Ldc_I4_4 transpilers.
	internal static class DataConstsPatches
	{
		private static readonly FieldInfo MaxPlayerCountField =
			typeof(DataConsts).GetField("C_MaxPlayerCount", BindingFlags.Instance | BindingFlags.Public);

		[HarmonyPatch(typeof(DataConsts), MethodType.Constructor, typeof(Const_MasterDataHolder))]
		internal static class ConstructorPatch
		{
			private static void Postfix(DataConsts __instance)
			{
				try
				{
					if (MaxPlayerCountField == null)
					{
						MelonLogger.Error("[TooManyPlayers] DataConsts.C_MaxPlayerCount field not found - cannot raise player cap.");
						return;
					}

					int maxPlayers = TooManyPlayersPreferences.MaxPlayers;
					int original = (int)MaxPlayerCountField.GetValue(__instance);

					// readonly field, but reflection can still write it after the ctor ran.
					MaxPlayerCountField.SetValue(__instance, maxPlayers);

					MelonLogger.Msg($"[TooManyPlayers] Raised C_MaxPlayerCount from {original} to {maxPlayers}.");
				}
				catch (System.Exception ex)
				{
					MelonLogger.Error($"[TooManyPlayers] Failed to raise C_MaxPlayerCount: {ex.Message}");
				}
			}
		}
	}
}
