using HarmonyLib;
using MelonLoader;
using TooManyPlayers.Config;
using System.Reflection.Emit;

namespace TooManyPlayers.Patches
{
	internal static class IVroomPatches
	{
		[HarmonyPatch(typeof(IVroom), "CanEnterChannel")]
		internal static class CanEnterChannelPatch
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
				catch (System.NullReferenceException)
				{
				}
				
				for (int i = 0; i < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Ldc_I4_4)
					{
						codes[i] = new CodeInstruction(OpCodes.Ldc_I4, maxPlayers);
					}
				}
				
				return codes;
			}
		}

		[HarmonyPatch(typeof(IVroom), "ProcessEnterWaitQueue")]
		internal static class ProcessEnterWaitQueuePatch
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
				catch (System.NullReferenceException)
				{
				}
				
				for (int i = 0; i < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Ldc_I4_4)
					{
						codes[i] = new CodeInstruction(OpCodes.Ldc_I4, maxPlayers);
					}
				}
				
				return codes;
			}
		}
	}
}
