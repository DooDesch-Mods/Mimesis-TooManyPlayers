# Mimesis TooManyPlayers

TooManyPlayers is a MelonLoader mod for Mimesis that increases the maximum player count
beyond the default limit of 4 players. Configure your preferred maximum and play with larger groups.

## Requirements
- Mimesis (latest Steam build)
- MelonLoader 0.7.1+

## Installation
1. Download the latest `TooManyPlayers.dll` release.
2. Drop it into `Mimesis/MelonLoader/Mods`.
3. Launch the game once so the config file is generated.

## Configuration
Adjustment values live in `UserData/MelonPreferences.cfg`.

Key options in the `TooManyPlayers` section:

- `MaxPlayers`: maximum number of players allowed in a session (default: `8`, minimum: `4`, maximum: `32766`).

The mod ensures that all player count checks throughout the game respect this configured limit,
including server socket limits, room entry checks, Steam lobby creation, and session management.

## How It Works

TooManyPlayers uses Harmony patches to intercept and modify player count limits at multiple points:

- **ServerSocket**: Overrides `GetMaximumClients()` and `SetMaximumClients()` to use your configured limit
- **IVroom**: Patches `CanEnterChannel()` to allow more players into rooms
- **VRoomManager**: Updates `EnterWaitingRoom()` and `EnterMaintenenceRoom()` checks
- **GameSessionInfo**: Modifies `AddPlayerSteamID()` to accept more players
- **SteamInviteDispatcher**: Creates Steam lobbies with increased capacity

All patches use transpilers or prefix/postfix methods to ensure compatibility and stability.

## Development
- Core entry: `Core.cs`
- Preferences: `Config/TooManyPlayersPreferences.cs`
- Harmony patches: `Patches/*.cs`
  - `ServerSocketPatches.cs` - Network layer limits
  - `IVroomPatches.cs` - Room entry validation
  - `VRoomManagerPatches.cs` - Room manager checks
  - `GameSessionInfoPatches.cs` - Session player tracking
  - `SteamInviteDispatcherPatches.cs` - Steam lobby creation

## License
Provided as-is under the MIT License. Contributions welcome via PR.

