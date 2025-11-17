using HarmonyLib;
using MelonLoader;
using TooManyPlayers.Config;

namespace TooManyPlayers.Patches
{
	/// <summary>
	/// Patches for FishySteamworks.Server.ServerSocket to override maximum client limits.
	/// </summary>
	internal static class ServerSocketPatches
	{
		[HarmonyPatch(typeof(FishySteamworks.Server.ServerSocket), "GetMaximumClients")]
		internal static class GetMaximumClientsPatch
		{
			private static bool Prefix(ref int __result)
			{
				// Return our max players, but respect the game's hard limit of 32766
				__result = System.Math.Min(TooManyPlayersPreferences.MaxPlayers, 32766);
				return false; // Skip original method
			}
		}

		[HarmonyPatch(typeof(FishySteamworks.Server.ServerSocket), "SetMaximumClients")]
		internal static class SetMaximumClientsPatch
		{
			private static bool Prefix(ref int value)
			{
				// Ensure the value matches our configured max, but respect the game's max of 32766
				// This allows artificially limiting to lower values if desired, but ensures consistency
				var targetValue = System.Math.Min(TooManyPlayersPreferences.MaxPlayers, 32766);
				if (value != targetValue)
				{
					value = targetValue;
				}
				return true; // Continue to original method
			}
		}

		[HarmonyPatch(typeof(FishySteamworks.Server.ServerSocket), MethodType.Constructor)]
		internal static class ServerSocketConstructorPatch
		{
			private static void Postfix(FishySteamworks.Server.ServerSocket __instance)
			{
				try
				{
					// Set max clients after construction
					var setMethod = typeof(FishySteamworks.Server.ServerSocket)
						.GetMethod("SetMaximumClients", 
							System.Reflection.BindingFlags.Instance | 
							System.Reflection.BindingFlags.NonPublic | 
							System.Reflection.BindingFlags.Public);
					
					setMethod?.Invoke(__instance, new object[] { TooManyPlayersPreferences.MaxPlayers });
				}
				catch (System.Exception ex)
				{
					MelonLogger.Error($"Failed to set max clients in ServerSocket constructor: {ex.Message}");
				}
			}
		}
	}
}
