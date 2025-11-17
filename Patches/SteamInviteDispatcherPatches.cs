using HarmonyLib;
using MelonLoader;
using TooManyPlayers.Config;
using Steamworks;

namespace TooManyPlayers.Patches
{
	/// <summary>
	/// Patches for SteamInviteDispatcher to create lobbies with increased player capacity.
	/// </summary>
	internal static class SteamInviteDispatcherPatches
	{
		[HarmonyPatch(typeof(SteamInviteDispatcher), "CreateLobby")]
		internal static class CreateLobbyPatch
		{
			private static bool Prefix(bool isOpenForRandomMatch)
			{
				try
				{
					// Create lobby with our max players instead of hardcoded 4
					// Use public lobby type like the original, but with our max players
					SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, TooManyPlayersPreferences.MaxPlayers);
					UnityEngine.PlayerPrefs.SetInt("TempLobbyIsOpen", isOpenForRandomMatch ? 1 : 0);
					return false; // Skip original method
				}
				catch (System.Exception ex)
				{
					MelonLogger.Error($"Error in CreateLobby patch: {ex.Message}");
					return true; // Fall back to original method
				}
			}
		}
	}
}
