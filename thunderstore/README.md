# MIMESIS - TooManyPlayers

> 🛟 **Need help or found a bug?** Get support at [support.doodesch.de/toomanyplayers](https://support.doodesch.de/toomanyplayers).


> Raise the hard 4-player session cap so you can host and play Mimesis with larger groups - you pick the maximum, the host enforces it.

![Version](https://img.shields.io/badge/version-1.1.5-blue)
![Game](https://img.shields.io/badge/game-MIMESIS-purple)
![MelonLoader](https://img.shields.io/badge/MelonLoader-0.7.3+-green)
![Status](https://img.shields.io/badge/status-working-brightgreen)

Mimesis ships with a hard limit of 4 players per session. TooManyPlayers raises the network, room, session and Steam lobby limits to the host's configured maximum.

## Features

- Raises the player cap from the default 4 to your configured value from 4 to 250 (`MaxPlayers`, default 8).
- Sets the room and session admission limit and supports more than ten player actors.
- Forces the network transport's client limit to your value (up to 250) and re-applies it whenever a socket is created.
- Raises the Steam lobby member cap from 4 to your configured maximum.
- Public room search uses each host's Steam capacity and shows it on the room card. The searching client needs this mod.
- Keeps the four existing survival result rows and reads scrap results correctly after larger player lists.
- Logs Steam lobby requests, creation result codes, the 30-second timeout, player-cap changes and failed session additions in `MelonLoader/Latest.log`.
- Your configured value is validated and clamped to the 4-250 range on load.

## Requirements

| Component | Version |
|---|---|
| MIMESIS | 0.4.0 |
| MelonLoader | 0.7.3+ |

## Installation

- Recommended: install via a Thunderstore mod manager (r2modman / Gale) and let it resolve dependencies.
- Manual: download `TooManyPlayers.dll` from [GitHub Releases](https://github.com/DooDesch-Mods/Mimesis-TooManyPlayers/releases), drop it into `MIMESIS/Mods/`, then launch the game once to generate the config file at `UserData/MelonPreferences.cfg`.

## Configuration

Stored in `UserData/MelonPreferences.cfg` under the `[TooManyPlayers]` category.

The host's `MaxPlayers` applies automatically to joining clients with this mod. Settings are fixed for the lobby; changes apply to the next lobby. Your local config is kept. A host without TooManyPlayers uses the vanilla limit of 4.

| Option | Description | Default | Values/Range |
|---|---|---|---|
| `MaxPlayers` | Maximum number of players allowed in a session (display name "Maximum Players"). Values below 4 or above 250 are clamped on load. | `8` | `4` - `250` (clamped) |

## Usage

There are no keybinds and no in-game UI beyond the standard MelonPreferences config.

1. Install the mod and launch the game once to generate `UserData/MelonPreferences.cfg`.
2. Set `MaxPlayers` under the `[TooManyPlayers]` category to your desired cap.
3. Relaunch the game.

Host-side requirement: the cap is enforced by the host/server, so the player hosting the lobby or session must have the mod installed and configured for the larger group to take effect.

## Compatibility

Built for Mimesis 0.4.0 / MelonLoader 0.7.3.

## Links

Source and releases: [github.com/DooDesch-Mods/Mimesis-TooManyPlayers](https://github.com/DooDesch-Mods/Mimesis-TooManyPlayers)

## AI disclosure

The mod icon is AI-generated.
