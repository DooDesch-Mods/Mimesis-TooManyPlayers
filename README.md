# Mimesis TooManyPlayers

A MelonLoader mod that increases the maximum player count in Mimesis beyond the default limit of 4 players. Configure your preferred maximum and play with larger groups.

---

## Table of Contents

- [Requirements](#requirements)
- [Installation](#installation)
- [Configuration](#configuration)
- [How It Works](#how-it-works)
- [Development](#development)
- [License](#license)

---

## Requirements

| Component | Version |
|-----------|---------|
| **Mimesis** | Latest Steam build |
| **MelonLoader** | 0.7.1 or higher |

---

## Installation

1. Download the latest `TooManyPlayers.dll` release from the [releases page](../../releases)
2. Place the file into your Mimesis mods directory:
   ```
   Mimesis/MelonLoader/Mods/TooManyPlayers.dll
   ```
3. Launch the game once to generate the configuration file

> **Note:** The configuration file will be created automatically on first launch at `UserData/MelonPreferences.cfg`

---

## Configuration

Configuration values are stored in `UserData/MelonPreferences.cfg` under the `TooManyPlayers` category.

### Available Options

| Option | Description | Default | Range |
|--------|-------------|---------|-------|
| `MaxPlayers` | Maximum number of players allowed in a session | `8` | `4` - `32766` |

### What Gets Modified

The mod ensures that all player count checks throughout the game respect your configured limit:

- Server socket limits
- Room entry validation
- Steam lobby creation
- Session management
- Waiting room capacity
- Maintenance room capacity

---

## How It Works

TooManyPlayers uses Harmony patches to intercept and modify player count limits at multiple critical points in the game's codebase.

### Patch Overview

| Component | Method(s) | Purpose |
|-----------|-----------|---------|
| **ServerSocket** | `GetMaximumClients()`, `SetMaximumClients()` | Overrides network layer client limits |
| **IVroom** | `CanEnterChannel()` | Allows more players into rooms |
| **VRoomManager** | `EnterWaitingRoom()`, `EnterMaintenenceRoom()` | Updates room manager capacity checks |
| **GameSessionInfo** | `AddPlayerSteamID()` | Modifies session player tracking |
| **SteamInviteDispatcher** | Steam lobby creation | Creates Steam lobbies with increased capacity |

### Technical Details

All patches use **transpilers** or **prefix/postfix** methods to ensure:
- Compatibility with game updates
- Stability and reliability
- Minimal performance impact

---

## Development

### Project Structure

```
MorePlayers/
├── Core.cs                          # Main entry point
├── Config/
│   └── TooManyPlayersPreferences.cs # Configuration management
└── Patches/
    ├── ServerSocketPatches.cs       # Network layer limits
    ├── IVroomPatches.cs             # Room entry validation
    ├── VRoomManagerPatches.cs       # Room manager checks
    ├── GameSessionInfoPatches.cs    # Session player tracking
    └── SteamInviteDispatcherPatches.cs # Steam lobby creation
```

### Key Files

- **`Core.cs`** - Core entry point and mod initialization
- **`Config/TooManyPlayersPreferences.cs`** - Preference management and configuration
- **`Patches/*.cs`** - Harmony patches for each modified component

---

## License

This project is provided as-is under the **MIT License**. Contributions are welcome via pull requests.

---
