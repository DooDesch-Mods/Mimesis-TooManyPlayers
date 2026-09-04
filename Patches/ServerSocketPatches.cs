using HarmonyLib;
using MelonLoader;
using TooManyPlayers.Config;

namespace TooManyPlayers.Patches
{
	internal static class ServerSocketPatches
	{
		[HarmonyPatch(typeof(FishySteamworks.Server.ServerSocket), "GetMaximumClients")]
		internal static class GetMaximumClientsPatch
		{
			private static bool Prefix(ref int __result)
			{
				__result = System.Math.Min(TooManyPlayersPreferences.MaxPlayers, 32766);
				return false;
			}
		}

		[HarmonyPatch(typeof(FishySteamworks.Server.ServerSocket), "SetMaximumClients")]
		internal static class SetMaximumClientsPatch
		{
			private static bool Prefix(ref int value)
			{
				var targetValue = System.Math.Min(TooManyPlayersPreferences.MaxPlayers, 32766);
				if (value != targetValue)
				{
					value = targetValue;
				}
				return true;
			}
		}

		[HarmonyPatch(typeof(FishySteamworks.Server.ServerSocket), MethodType.Constructor)]
		internal static class ServerSocketConstructorPatch
		{
			private static void Postfix(FishySteamworks.Server.ServerSocket __instance)
			{
				try
				{
					var setMethod = typeof(FishySteamworks.Server.ServerSocket)
						.GetMethod("SetMaximumClients", 
							System.Reflection.BindingFlags.Instance | 
							System.Reflection.BindingFlags.NonPublic | 
							System.Reflection.BindingFlags.Public);
					
					if (setMethod == null)
					{
						MelonLogger.Error("[TooManyPlayers] ServerSocket.SetMaximumClients not found - the cap stays at the vanilla value.");
						return;
					}

					setMethod.Invoke(__instance, new object[] { TooManyPlayersPreferences.MaxPlayers });
				}
				catch (System.Exception ex)
				{
					MelonLogger.Error($"[TooManyPlayers] Failed to raise the client cap on the server socket: {ex.Message}");
				}
			}
		}
	}
}
