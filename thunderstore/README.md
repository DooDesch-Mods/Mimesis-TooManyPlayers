# TooManyPlayers Mod

Increases the maximum player count beyond the default limit of 4 players. Configure your preferred maximum and play with larger groups.

## Features

- Configurable maximum player count (4-32766)
- Patches all player count checks throughout the game
- Works with server socket limits, room entry checks, Steam lobby creation, and session management
- Compatible with FakePlayers mod for testing

## Configuration

- `MaxPlayers`: Maximum number of players allowed in a session (default: `8`, minimum: `4`, maximum: `32766`)

## Installation

1. Install via Thunderstore Mod Manager
2. Or manually download and extract to `Mimesis/MelonLoader/Mods`

## How It Works

The mod uses Harmony patches to intercept and modify player count limits at multiple points:

- **ServerSocket**: Overrides maximum client limits
- **IVroom**: Patches room entry validation
- **VRoomManager**: Updates room manager checks
- **GameSessionInfo**: Modifies session player tracking
- **SteamInviteDispatcher**: Creates Steam lobbies with increased capacity


