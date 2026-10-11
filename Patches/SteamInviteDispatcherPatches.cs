using HarmonyLib;
using TooManyPlayers.Config;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using MelonLoader;
using Steamworks;
using UnityEngine;

namespace TooManyPlayers.Patches
{
	internal static class SteamInviteDispatcherPatches
	{
		private static bool _awaitingLobby;
		private static float _lobbyRequestedAt;
		internal static void UpdateDiagnostics()
		{
			if (!_awaitingLobby || Time.realtimeSinceStartup - _lobbyRequestedAt < 30f) return;
			_awaitingLobby = false;
			MelonLogger.Error($"[TooManyPlayers] Steam lobby creation has no successful response after {Time.realtimeSinceStartup - _lobbyRequestedAt:F1}s (timeout: 30s).");
		}

		[HarmonyPatch(typeof(SteamInviteDispatcher), "OnLobbyCreated")]
		internal static class LobbyCreatedDiagnosticPatch
		{
			private static void Prefix(LobbyCreated_t callback)
			{
				if (callback.m_eResult == EResult.k_EResultOK)
				{
					_awaitingLobby = false;
					MelonLogger.Msg("[TooManyPlayers] Steam lobby creation succeeded.");
				}
				else
					MelonLogger.Error($"[TooManyPlayers] Steam lobby creation failed: {callback.m_eResult} ({(int)callback.m_eResult}).");
			}
		}
		// Read the configured cap when a lobby is created. Harmony registers this patch
		// before OnInitializeMelon initializes the preferences.
		[HarmonyPatch(typeof(SteamInviteDispatcher), "CreateLobby")]
		internal static class CreateLobbyPatch
		{
			private static void Prefix(bool isRetryAttempt)
			{
				if (!isRetryAttempt)
				{
					_awaitingLobby = true;
					_lobbyRequestedAt = Time.realtimeSinceStartup;
				}
				MelonLogger.Msg($"[TooManyPlayers] Request Steam lobby: maxPlayers={TooManyPlayersPreferences.MaxPlayers}, retry={isRetryAttempt}.");
			}

			private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				var codes = new List<CodeInstruction>(instructions);
				var maxPlayersGetter = typeof(TooManyPlayersPreferences)
					.GetProperty(nameof(TooManyPlayersPreferences.MaxPlayers), BindingFlags.Static | BindingFlags.NonPublic)
					.GetGetMethod(nonPublic: true);

				bool patched = false;
				for (int i = 0; i + 1 < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Ldc_I4_4 &&
						codes[i + 1].opcode == OpCodes.Call &&
						codes[i + 1].operand is MethodInfo method &&
						method.DeclaringType?.FullName == "Steamworks.SteamMatchmaking" &&
						method.Name == "CreateLobby")
					{
						codes[i].opcode = OpCodes.Call;
						codes[i].operand = maxPlayersGetter;
						patched = true;
						break;
					}
				}

				if (!patched)
				{
					throw new InvalidOperationException("[TooManyPlayers] SteamInviteDispatcher.CreateLobby cap argument was not found.");
				}

				return codes;
			}
		}
	}
}
