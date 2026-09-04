# MIMESIS - TooManyPlayers

> 🛟 **Need help or found a bug?** Get support at [support.doodesch.de](https://support.doodesch.de).


> Raise the hard 4-player session cap so you can host and play Mimesis with larger groups - you pick the maximum, the mod enforces it everywhere.

![Version](https://img.shields.io/badge/version-1.1.0-blue)
![Game](https://img.shields.io/badge/game-MIMESIS-purple)
![MelonLoader](https://img.shields.io/badge/MelonLoader-0.7.3+-green)
![Status](https://img.shields.io/badge/status-working-brightgreen)

Mimesis ships with a hard limit of 4 players per session. TooManyPlayers rewrites every player-count check in the game - the network socket, room entry, session tracking, and the Steam lobby - to honor a maximum you configure, so you can run bigger lobbies.

## Features

- Raises the player cap from the default 4 to any value you configure (`MaxPlayers`, default 8).
- One central override does the heavy lifting: a postfix on the `DataConsts` constructor rewrites `Bifrost.ConstEnum.DataConsts.C_MaxPlayerCount`, the single data-driven field every in-game limit check reads (waiting/maintenance rooms, `IVroom.CanEnterChannel`, `GameSessionInfo.AddPlayerSteamID`). Lifting that one field raises the cap everywhere at once.
- Network layer: prefixes on `FishySteamworks.Server.ServerSocket` `GetMaximumClients` / `SetMaximumClients` force the transport's client limit to your value (clamped to 32766), and a constructor postfix re-applies it whenever a socket is created.
- Steam lobby capacity: a transpiler on `SteamInviteDispatcher.CreateLobby` replaces the hardcoded lobby member cap of 4 with your configured maximum. (Steam still enforces its own ~250 hard limit at runtime.)
- Diagnostics: logs the old and new `C_MaxPlayerCount` values on startup, and warns if a player fails to be added to a session.
- Your configured value is validated and clamped to the 4-32766 range on load, with a warning logged if you set it out of bounds.

## Requirements

| Component | Version |
|---|---|
| MIMESIS | 0.3.0 (current Steam build) |
| MelonLoader | 0.7.3+ |

## Installation

- Recommended: install via a Thunderstore mod manager (r2modman / Gale) and let it resolve dependencies. Package: `DooDesch/TooManyPlayers`.
- Manual: download `TooManyPlayers.dll` from the [releases page](../../releases) and drop it into `MIMESIS/Mods/`. Launch the game once to generate the config file at `UserData/MelonPreferences.cfg`.

## Configuration

Stored in `UserData/MelonPreferences.cfg` under the `[TooManyPlayers]` category.

| Option | Description | Default | Values/Range |
|---|---|---|---|
| `MaxPlayers` | Maximum number of players allowed in a session (display name "Maximum Players"). Values below 4 or above 32766 are clamped on load, with a warning logged. | `8` | `4` - `32766` (clamped) |

## Usage

There are no keybinds and no in-game UI beyond the standard MelonPreferences config.

1. Install the mod and launch the game once to generate `UserData/MelonPreferences.cfg`.
2. Set `MaxPlayers` under the `[TooManyPlayers]` category to your desired cap.
3. Relaunch the game.

Host-side requirement: the cap is enforced by the host/server, so the player hosting the lobby or session must have the mod installed and configured for the larger group to take effect. Joining players do not strictly need it for the cap to apply, but matching configs avoid surprises.

## Compatibility

Built for Mimesis 0.3.0 / MelonLoader 0.7.3. Because the limit is enforced server-side, the host carries the configured cap for the whole lobby.

## Building (developers)

Standalone mod, no MimicAPI dependency.

```
dotnet build -c Release
```

Targets `netstandard2.1` and references the game DLLs in `Workspace/lib/game` plus MelonLoader/Harmony in `Workspace/lib/melonloader`. The post-build step copies `TooManyPlayers.dll` into your local `MIMESIS/Mods` folder.

Patch overview (all under `Patches/`):

| File | Target | Role |
|---|---|---|
| `DataConstsPatches.cs` | `DataConsts` constructor | Central override of `C_MaxPlayerCount` - raises the cap everywhere |
| `ServerSocketPatches.cs` | `ServerSocket` `GetMaximumClients` / `SetMaximumClients` / ctor | Forces the network client limit to your value (clamped 32766) |
| `SteamInviteDispatcherPatches.cs` | `SteamInviteDispatcher.CreateLobby` | Transpiler replacing the hardcoded lobby cap of 4 |
| `GameSessionInfoPatches.cs` | `GameSessionInfo.AddPlayerSteamID` | Diagnostic postfix only (warns on failed adds) |

## Credits / License

Author: DooDesch. Provided as-is under the MIT License. Contributions are welcome via pull requests at [Mimesis-TooManyPlayers](https://github.com/DooDesch/Mimesis-TooManyPlayers).

## AI disclosure

The mod icon is AI-generated. Code and configuration are hand-written.
