using System;
using MelonLoader;

namespace TooManyPlayers.Config
{
	internal static class TooManyPlayersPreferences
	{
		private const string CategoryId = "TooManyPlayers";

		private static MelonPreferences_Category _category;
		private static MelonPreferences_Entry<int> _maxPlayers;

		internal static void Initialize()
		{
			if (_category != null)
			{
				return;
			}

			_category = MelonPreferences.CreateCategory(CategoryId, "TooManyPlayers");
			_maxPlayers = CreateEntry("MaxPlayers", 999, "Maximum Players", 
				"The maximum number of players allowed in a session. Default: 999. Minimum: 4, Maximum: 999.");
			
			// Validate and clamp the value
			if (_maxPlayers.Value < 4)
			{
				MelonLogger.Warning($"MaxPlayers value {_maxPlayers.Value} is below minimum of 4. Clamping to 4.");
				_maxPlayers.Value = 4;
			}
			else if (_maxPlayers.Value > 999)
			{
				MelonLogger.Warning($"MaxPlayers value {_maxPlayers.Value} exceeds maximum of 999. Clamping to 999.");
				_maxPlayers.Value = 999;
			}
		}

		private static MelonPreferences_Entry<T> CreateEntry<T>(string identifier, T defaultValue, string displayName, string description = null)
		{
			if (_category == null)
			{
				throw new InvalidOperationException("Preference category not initialized.");
			}

			return _category.CreateEntry(identifier, defaultValue, displayName, description);
		}

		internal static int MaxPlayers => _maxPlayers.Value;
	}
}

