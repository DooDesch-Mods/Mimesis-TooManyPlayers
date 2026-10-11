# Changelog

## [1.1.3] - 2026-10-11

- Joining clients automatically use the host's player limit for the lobby without changing their local config.
- Restore the vanilla limit of 4 when the host does not use TooManyPlayers.
- Log Steam lobby requests, creation result codes and the 30-second creation timeout in MelonLoader/Latest.log.

## [1.1.2] - 2026-10-11

- Clamp MaxPlayers to Steam's limit of 250 before creating a lobby.

## [1.1.1] - 2026-10-11

- Steam lobby creation reads MaxPlayers after preferences have loaded.
