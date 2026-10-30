using System;
using DooDesch;
using MelonLoader;

namespace TooManyPlayers.Config
{
	internal static class TooManyPlayersPreferences
	{
		private const string CategoryId = "TooManyPlayers";

		private static MelonPreferences_Category _category;
		private static MelonPreferences_Entry<int> _maxPlayers;
		private static HostSetting<int> _effectiveMaxPlayers;
		private static MelonPreferences_Entry<bool> _enabled;
		private static HostSetting<bool> _effectiveEnabled;
		private static HostSettings _hostSettings;
		internal static event Action EffectiveValuesChanged;

		internal static void Initialize()
		{
			if (_category != null)
			{
				return;
			}

			_category = MelonPreferences.CreateCategory(CategoryId, "TooManyPlayers");
			_enabled = CreateEntry("Enabled", true, "Enabled", "Host setting, next lobby. Enable the larger player cap. Disabled uses the vanilla cap of four without changing MaxPlayers.");
			_maxPlayers = CreateEntry("MaxPlayers", 8, "Maximum Players", 
				"The maximum number of players allowed in a session. Default: 8. Minimum: 4, Maximum: 250.");
			
			// Validate and clamp the value
			if (_maxPlayers.Value < 4)
			{
				MelonLogger.Warning($"MaxPlayers value {_maxPlayers.Value} is below minimum of 4. Clamping to 4.");
				_maxPlayers.Value = 4;
			}
			else if (_maxPlayers.Value > 250)
			{
				MelonLogger.Warning($"MaxPlayers value {_maxPlayers.Value} exceeds Steam's maximum of 250. Clamping to 250.");
				_maxPlayers.Value = 250;
			}

			_hostSettings = new HostSettings(CategoryId);
			_effectiveEnabled = _hostSettings.Register(_enabled, false, legacy: false);
			_effectiveMaxPlayers = _hostSettings.Register(_maxPlayers, 4, value => value >= 4 && value <= 250, localValue: () => _enabled.Value ? _maxPlayers.Value : 4);
			_hostSettings.EffectiveValuesChanged += () => EffectiveValuesChanged?.Invoke();
		}

		private static MelonPreferences_Entry<T> CreateEntry<T>(string identifier, T defaultValue, string displayName, string description = null)
		{
			if (_category == null)
			{
				throw new InvalidOperationException("Preference category not initialized.");
			}

			return _category.CreateEntry(identifier, defaultValue, displayName, description);
		}

		internal static bool Enabled => _hostSettings?.UsingLegacyHost == true || (_effectiveEnabled?.Value ?? _enabled?.Value ?? true);
		internal static int MaxPlayers => Enabled ? (_effectiveMaxPlayers?.Value ?? _maxPlayers?.Value ?? 8) : 4;
	}
}
