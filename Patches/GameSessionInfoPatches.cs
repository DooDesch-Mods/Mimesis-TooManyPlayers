using HarmonyLib;
using MelonLoader;
using TooManyPlayers.Config;
using System.Reflection.Emit;

namespace TooManyPlayers.Patches
{
	/// <summary>
	/// Patches for GameSessionInfo to override player count limits.
	/// </summary>
	internal static class GameSessionInfoPatches
	{
		[HarmonyPatch(typeof(GameSessionInfo), "AddPlayerSteamID")]
		internal static class AddPlayerSteamIDPatch
		{
			private static System.Collections.Generic.IEnumerable<CodeInstruction> Transpiler(
				System.Collections.Generic.IEnumerable<CodeInstruction> instructions)
			{
				var codes = new System.Collections.Generic.List<CodeInstruction>(instructions);
				
				// Find and replace the hardcoded 4 with our max players value
				for (int i = 0; i < codes.Count; i++)
				{
					// Look for Ldc_I4_4 (load constant int 4)
					if (codes[i].opcode == OpCodes.Ldc_I4_4)
					{
						// Replace with our max players
						codes[i] = new CodeInstruction(OpCodes.Ldc_I4, TooManyPlayersPreferences.MaxPlayers);
					}
				}
				
				return codes;
			}
		}
	}
}
