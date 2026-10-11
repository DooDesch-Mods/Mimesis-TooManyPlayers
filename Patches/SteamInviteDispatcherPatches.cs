using HarmonyLib;
using TooManyPlayers.Config;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace TooManyPlayers.Patches
{
	internal static class SteamInviteDispatcherPatches
	{
		// Read the configured cap when a lobby is created. Harmony registers this patch
		// before OnInitializeMelon initializes the preferences.
		[HarmonyPatch(typeof(SteamInviteDispatcher), "CreateLobby")]
		internal static class CreateLobbyPatch
		{
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
