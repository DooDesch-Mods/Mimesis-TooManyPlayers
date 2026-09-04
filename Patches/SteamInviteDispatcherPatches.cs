using HarmonyLib;
using MelonLoader;
using TooManyPlayers.Config;
using System.Reflection.Emit;

namespace TooManyPlayers.Patches
{
	internal static class SteamInviteDispatcherPatches
	{
		// SteamInviteDispatcher.CreateLobby still hardcodes the Steam lobby member cap:
		//   SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);   (decompiled/SteamInviteDispatcher.cs:793)
		// That literal 4 is the only Ldc_I4_4 in the method. We transpile just that
		// literal so the rest of the method - the CreateLobby timeout coroutine and the
		// isRetryAttempt handling added in 0.3.0 - runs unchanged.
		// Note: Steam itself caps lobby members at ~250, so very large values are
		// clamped/rejected by Steam at runtime regardless.
		[HarmonyPatch(typeof(SteamInviteDispatcher), "CreateLobby")]
		internal static class CreateLobbyPatch
		{
			private static System.Collections.Generic.IEnumerable<CodeInstruction> Transpiler(
				System.Collections.Generic.IEnumerable<CodeInstruction> instructions)
			{
				var codes = new System.Collections.Generic.List<CodeInstruction>(instructions);

				int maxPlayers = 8;
				try
				{
					maxPlayers = TooManyPlayersPreferences.MaxPlayers;
				}
				catch (System.NullReferenceException ex)
				{
					// A transpiler can run before the preferences exist. The lobby then keeps the
					// vanilla size, and that has to be visible - it looks exactly like a mod that
					// never ran.
					MelonLogger.Warning($"[TooManyPlayers] Preferences not ready, the lobby keeps {maxPlayers} seats: {ex.Message}");
				}

				bool patched = false;
				for (int i = 0; i < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Ldc_I4_4)
					{
						codes[i] = new CodeInstruction(OpCodes.Ldc_I4, maxPlayers);
						patched = true;
						break;
					}
				}

				if (!patched)
				{
					MelonLogger.Error("[TooManyPlayers] Failed to apply SteamInviteDispatcher.CreateLobby patch (lobby cap literal not found)!");
				}

				return codes;
			}
		}
	}
}
