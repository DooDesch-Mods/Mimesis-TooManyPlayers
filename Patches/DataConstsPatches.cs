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
		private static DataConsts _currentConsts;

		internal static void ApplyEffectiveValues()
		{
			if (_currentConsts != null) Apply(_currentConsts);
		}

		private static void Apply(DataConsts consts)
		{
			try
			{
				if (MaxPlayerCountField == null)
				{
					MelonLogger.Error("[TooManyPlayers] DataConsts.C_MaxPlayerCount field not found - cannot set player cap.");
					return;
				}
				int maxPlayers = TooManyPlayersPreferences.MaxPlayers;
				int original = (int)MaxPlayerCountField.GetValue(consts);
				if (original == maxPlayers) return;
				MaxPlayerCountField.SetValue(consts, maxPlayers);
				MelonLogger.Msg($"[TooManyPlayers] Set C_MaxPlayerCount from {original} to {maxPlayers}.");
			}
			catch (System.Exception ex)
			{
				MelonLogger.Error($"[TooManyPlayers] Failed to set C_MaxPlayerCount: {ex}");
			}
		}

		[HarmonyPatch(typeof(DataConsts), MethodType.Constructor, typeof(Const_MasterDataHolder))]
		internal static class ConstructorPatch
		{
			private static void Postfix(DataConsts __instance)
			{
				_currentConsts = __instance;
				Apply(__instance);
			}
		}
	}
}
