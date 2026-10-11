# MIMESIS - TooManyPlayers

> 🛟 **Need help or found a bug?** Get support at [support.doodesch.de/toomanyplayers](https://support.doodesch.de/toomanyplayers).


> Raise the hard 4-player session cap so you can host and play Mimesis with larger groups - you pick the maximum, the host enforces it.

![Version](https://img.shields.io/badge/version-1.1.4-blue)
![Game](https://img.shields.io/badge/game-MIMESIS-purple)
![MelonLoader](https://img.shields.io/badge/MelonLoader-0.7.3+-green)
![Status](https://img.shields.io/badge/status-working-brightgreen)

Mimesis ships with a hard limit of 4 players per session. TooManyPlayers raises the network, room, session and Steam lobby limits to the host's configured maximum.

## Features

- Raises the player cap from the default 4 to your configured value from 4 to 250 (`MaxPlayers`, default 8).
- A postfix on the `DataConsts` constructor sets the room and session admission limit (`IVroom.CanEnterChannel`, `GameSessionInfo.AddPlayerSteamID`). The player container also supports up to 250 entries; room entry still enforces the host's configured limit.
- Network layer: prefixes on `FishySteamworks.Server.ServerSocket` `GetMaximumClients` / `SetMaximumClients` force the transport's client limit to your value (up to 250), and a constructor postfix re-applies it whenever a socket is created.
- Steam lobby capacity: a transpiler on `SteamInviteDispatcher.CreateLobby` replaces the hardcoded lobby member cap of 4 with your configured maximum.
- Public room search uses each host's Steam lobby capacity instead of hiding every room with four players. Joining through the public list requires the mod on the searching client.
- Public room cards show the host's actual Steam capacity.
- The survival result screen keeps its four existing rows and correctly reads scrap results after larger player lists. Extra menu/result rows are not added.
- Diagnostics: logs Steam lobby requests, creation result codes, the 30-second timeout, player-cap changes and failed session additions in `MelonLoader/Latest.log`.
- Your configured value is validated and clamped to the 4-250 range on load, with a warning logged if you set it out of bounds.

## Requirements

| Component | Version |
|---|---|
| MIMESIS | 0.3.2 |
| MelonLoader | 0.7.3+ |

## Installation

- Recommended: install via a Thunderstore mod manager (r2modman / Gale) and let it resolve dependencies. Package: `DooDesch/TooManyPlayers`.
- Manual: download `TooManyPlayers.dll` from the [releases page](https://github.com/DooDesch-Mods/Mimesis-TooManyPlayers/releases) and drop it into `MIMESIS/Mods/`. Launch the game once to generate the config file at `UserData/MelonPreferences.cfg`.

## Configuration

Stored in `UserData/MelonPreferences.cfg` under the `[TooManyPlayers]` category.

The host's `MaxPlayers` applies automatically to joining clients with this mod. Settings are fixed for the lobby; changes apply to the next lobby. Your local config is kept. A host without TooManyPlayers uses the vanilla limit of 4.

| Option | Description | Default | Values/Range |
|---|---|---|---|
| `MaxPlayers` | Maximum number of players allowed in a session (display name "Maximum Players"). Values below 4 or above 250 are clamped on load, with a warning logged. | `8` | `4` - `250` (clamped) |

## Usage

There are no keybinds and no in-game UI beyond the standard MelonPreferences config.

1. Install the mod and launch the game once to generate `UserData/MelonPreferences.cfg`.
2. Set `MaxPlayers` under the `[TooManyPlayers]` category to your desired cap.
3. Relaunch the game.

Host-side requirement: the cap is enforced by the host/server, so the player hosting the lobby or session must have the mod installed and configured for the larger group to take effect. Joining players with the mod automatically use the host's cap.

## Compatibility

Built for Mimesis 0.3.2 / MelonLoader 0.7.3. Because the limit is enforced server-side, the host carries the configured cap for the whole lobby.

## Building (developers)

Standalone mod, no MimicAPI dependency.

```
dotnet build -c Release
```

Targets `netstandard2.1` and references the game DLLs in `Workspace/lib/game` plus MelonLoader/Harmony in `Workspace/lib/melonloader`. The shared deploy configuration defaults to `_Test/MIMESIS/Mods`.

Patch overview (all under `Patches/`):

| File | Target | Role |
|---|---|---|
| `DataConstsPatches.cs` | `DataConsts` constructor | Effective host cap for room and session admission |
| `ServerSocketPatches.cs` | `ServerSocket` `GetMaximumClients` / `SetMaximumClients` / ctor | Forces the network client limit to your value (up to 250) |
| `SteamInviteDispatcherPatches.cs` | `SteamInviteDispatcher.CreateLobby` | Transpiler replacing the hardcoded lobby cap of 4 |
| `PlayerContainerPatches.cs` | `IVroom` constructor | Capacity for up to 250 player actors |
| `SurvivalResultPatches.cs` | `UIPrefab_SurvivalResult.PatchParameter` | Keep the fixed four-row UI bound |
| `PublicRoomListPatches.cs` | `UIPrefab_PublicRoomList.SetRoomList` | Filter using each listed host's Steam capacity |
| `RoomCardPatches.cs` | `UiPrefab_RoomCard.SetRoomData` | Show the listed host's Steam capacity |
| `GameSessionInfoPatches.cs` | `GameSessionInfo.AddPlayerSteamID` | Diagnostic postfix only (warns on failed adds) |

## Credits / License

Author: DooDesch. Provided as-is under the MIT License. Contributions are welcome via pull requests at [Mimesis-TooManyPlayers](https://github.com/DooDesch-Mods/Mimesis-TooManyPlayers).

## AI disclosure

The mod icon is AI-generated.
