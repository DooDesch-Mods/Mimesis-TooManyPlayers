using HarmonyLib;
using MelonLoader;
using TooManyPlayers.Config;
using System.Reflection.Emit;
using System.Reflection;

namespace TooManyPlayers.Patches
{
	internal static class GameSessionInfoPatches
	{
		private static int GetMaxPlayers()
		{
			try
			{
				return TooManyPlayersPreferences.MaxPlayers;
			}
			catch (System.NullReferenceException)
			{
				return 8;
			}
			catch (System.Exception)
			{
				return 8;
			}
		}

		[HarmonyPatch(typeof(GameSessionInfo), "AddPlayerSteamID")]
		internal static class AddPlayerSteamIDPatch
		{
			private static void Postfix(GameSessionInfo __instance, ulong steamID, bool isHost, bool __result)
			{
				if (!__result)
				{
					MelonLogger.Warning($"[TooManyPlayers] Failed to add player (SteamID: {steamID})");
				}
			}

			private static System.Collections.Generic.IEnumerable<CodeInstruction> Transpiler(
				System.Collections.Generic.IEnumerable<CodeInstruction> instructions)
			{
				var codes = new System.Collections.Generic.List<CodeInstruction>(instructions);
				
				var getMaxPlayersMethod = typeof(GameSessionInfoPatches).GetMethod(
					nameof(GetMaxPlayers), 
					BindingFlags.NonPublic | BindingFlags.Static);
				
				if (getMaxPlayersMethod == null)
				{
					return codes;
				}
				
				bool alreadyPatched = false;
				for (int i = 0; i < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Call && codes[i].operand is MethodInfo method && method == getMaxPlayersMethod)
					{
						alreadyPatched = true;
						break;
					}
				}
				
				if (alreadyPatched)
				{
					return codes;
				}
				
				bool patchApplied = false;
				for (int i = 0; i < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Ldc_I4_4)
					{
						bool isPlayerCountCheck = false;
						for (int j = System.Math.Max(0, i - 10); j < i; j++)
						{
							if (codes[j].opcode == OpCodes.Callvirt || codes[j].opcode == OpCodes.Call)
							{
								var method = codes[j].operand as MethodInfo;
								if (method != null && method.Name == "get_Count")
								{
									isPlayerCountCheck = true;
									break;
								}
							}
						}
						
						if (isPlayerCountCheck)
						{
							codes[i] = new CodeInstruction(OpCodes.Call, getMaxPlayersMethod);
							patchApplied = true;
							break;
						}
					}
				}
				
				if (!patchApplied)
				{
					MelonLogger.Error("[TooManyPlayers] Failed to apply GameSessionInfo patch!");
				}
				
				return codes;
			}
		}
	}
}
