using TooManyPlayers.Config;
using MelonLoader;

[assembly: MelonInfo(typeof(TooManyPlayers.Core), "TooManyPlayers", "1.0.0", "DooDesch", null)]
[assembly: MelonGame("ReLUGames", "MIMESIS")]

namespace TooManyPlayers
{
	public sealed class Core : MelonMod
	{
		public override void OnInitializeMelon()
		{
			TooManyPlayersPreferences.Initialize();
			HarmonyInstance.PatchAll();
			
			var maxPlayers = TooManyPlayersPreferences.MaxPlayers;
			MelonLogger.Msg($"TooManyPlayers initialized. Max players: {maxPlayers}");
			
			if (maxPlayers < 4)
			{
				MelonLogger.Warning($"Max players is set to {maxPlayers}, which is below the default of 4. This may cause issues.");
			}
			else if (maxPlayers > 999)
			{
				MelonLogger.Warning($"Max players is set to {maxPlayers}, which exceeds the recommended maximum of 999.");
			}
		}
	}
}
