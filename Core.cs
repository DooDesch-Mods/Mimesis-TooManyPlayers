using TooManyPlayers.Config;
using MelonLoader;

[assembly: MelonInfo(typeof(TooManyPlayers.Core), "TooManyPlayers", DooDesch.ModVersion.Current, "DooDesch", null)]
[assembly: MelonGame("ReLUGames", "MIMESIS")]

namespace TooManyPlayers
{
	public sealed class Core : MelonMod
	{
		public override void OnInitializeMelon()
		{
			TooManyPlayersPreferences.Initialize();
			// MelonLoader auto-applies this assembly's Harmony patches via HarmonyInit(); calling PatchAll()
			// here too would double-apply every patch (each prefix/postfix runs twice). Do NOT add it back.
			// (See FakePlayers/Core.cs.)
			
			MelonLogger.Msg($"TooManyPlayers initialized. Max players: {TooManyPlayersPreferences.MaxPlayers}");
		}
	}
}
