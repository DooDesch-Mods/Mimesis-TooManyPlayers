# Changelog

## [1.1.5] - 2026-10-27

### Fixed

- Apply each host's real capacity to the public tram list as well as the public room browser, so groups above four players remain visible while places are free.
- Remove the obsolete survival-result patch. MIMESIS 0.4.0 already limits the screen to its available rows and keeps the full player count for scrap results.

## [1.1.4] - 2026-10-12

- Remove the separate ten-player actor-container limit.
- Prevent the survival result screen from indexing past its four UI rows.
- Public room search uses each host's Steam capacity instead of the fixed four-player filter.
- Public room cards show the host's actual Steam capacity.

## [1.1.3] - 2026-10-11

- Joining clients automatically use the host's player limit for the lobby without changing their local config.
- Restore the vanilla limit of 4 when the host does not use TooManyPlayers.
- Log Steam lobby requests, creation result codes and the 30-second creation timeout in MelonLoader/Latest.log.

## [1.1.2] - 2026-10-11

- Clamp MaxPlayers to Steam's limit of 250 before creating a lobby.

## [1.1.1] - 2026-10-11

- Steam lobby creation reads MaxPlayers after preferences have loaded.
