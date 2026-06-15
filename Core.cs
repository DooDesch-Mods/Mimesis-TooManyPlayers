using TooManyPlayers.Config;
using MelonLoader;

[assembly: MelonInfo(typeof(TooManyPlayers.Core), "TooManyPlayers", "1.1.0", "DooDesch", null)]
[assembly: MelonGame("ReLUGames", "MIMESIS")]

namespace TooManyPlayers
{
	public sealed class Core : MelonMod
	{
		public override void OnInitializeMelon()
		{
			TooManyPlayersPreferences.Initialize();
			HarmonyInstance.PatchAll();
			
			MelonLogger.Msg($"TooManyPlayers initialized. Max players: {TooManyPlayersPreferences.MaxPlayers}");
		}
	}
}
