# MIMESIS - TooManyPlayers

> Raise the hard 4-player session cap so you can host and play Mimesis with larger groups - you pick the maximum, the mod enforces it everywhere.

![Version](https://img.shields.io/badge/version-1.1.0-blue)
![Game](https://img.shields.io/badge/game-MIMESIS-purple)
![MelonLoader](https://img.shields.io/badge/MelonLoader-0.7.3+-green)
![Status](https://img.shields.io/badge/status-working-brightgreen)

Mimesis ships with a hard limit of 4 players per session. TooManyPlayers rewrites every player-count check in the game - the network socket, room entry, session tracking, and the Steam lobby - to honor a maximum you configure, so you can run bigger lobbies.

## Features

- Raises the player cap from the default 4 to any value you configure (`MaxPlayers`, default 8).
- One central override raises the cap everywhere at once by rewriting the single data-driven field every in-game limit check reads (waiting/maintenance rooms, room entry, session tracking).
- Forces the network transport's client limit to your value (clamped to 32766) and re-applies it whenever a socket is created.
- Raises the Steam lobby member cap from 4 to your configured maximum. (Steam still enforces its own ~250 hard limit at runtime.)
- Logs the old and new cap values on startup and warns if a player fails to be added to a session.
- Your configured value is validated and clamped to the 4-32766 range on load.

## Requirements

| Component | Version |
|---|---|
| MIMESIS | 0.3.0 (current Steam build) |
| MelonLoader | 0.7.3+ |

## Installation

- Recommended: install via a Thunderstore mod manager (r2modman / Gale) and let it resolve dependencies.
- Manual: download `TooManyPlayers.dll` from [GitHub Releases](https://github.com/DooDesch/Mimesis-TooManyPlayers/releases), drop it into `MIMESIS/Mods/`, then launch the game once to generate the config file at `UserData/MelonPreferences.cfg`.

## Configuration

Stored in `UserData/MelonPreferences.cfg` under the `[TooManyPlayers]` category.

| Option | Description | Default | Values/Range |
|---|---|---|---|
| `MaxPlayers` | Maximum number of players allowed in a session (display name "Maximum Players"). Values below 4 or above 32766 are clamped on load. | `8` | `4` - `32766` (clamped) |

## Usage

There are no keybinds and no in-game UI beyond the standard MelonPreferences config.

1. Install the mod and launch the game once to generate `UserData/MelonPreferences.cfg`.
2. Set `MaxPlayers` under the `[TooManyPlayers]` category to your desired cap.
3. Relaunch the game.

Host-side requirement: the cap is enforced by the host/server, so the player hosting the lobby or session must have the mod installed and configured for the larger group to take effect.

## Compatibility

Built for Mimesis 0.3.0 / MelonLoader 0.7.3.

## Links

Source and releases: [github.com/DooDesch/Mimesis-TooManyPlayers](https://github.com/DooDesch/Mimesis-TooManyPlayers)
