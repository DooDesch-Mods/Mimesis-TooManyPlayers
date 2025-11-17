using HarmonyLib;
using MelonLoader;
using TooManyPlayers.Config;
using Steamworks;

namespace TooManyPlayers.Patches
{
	internal static class SteamInviteDispatcherPatches
	{
		[HarmonyPatch(typeof(SteamInviteDispatcher), "CreateLobby")]
		internal static class CreateLobbyPatch
		{
			private static bool Prefix(bool isOpenForRandomMatch)
			{
				try
				{
					SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, TooManyPlayersPreferences.MaxPlayers);
					UnityEngine.PlayerPrefs.SetInt("TempLobbyIsOpen", isOpenForRandomMatch ? 1 : 0);
					return false;
				}
				catch (System.Exception)
				{
					return true;
				}
			}
		}
	}
}
