# Bullet Bang Architecture

## Purpose

Bullet Bang is a generic networked social lobby capable of hosting independent
table games. The lobby must not know BANG! rules. A future BANG! module will
attach to a table through a game-session boundary and will expose only public
spectator state.

## Current modules

### Network

- `MainLobbyManager` owns Fusion runner lifecycle, connection callbacks, network
  spawning, and network input collection.
- `NetworkPlayer` is the authoritative replicated player aggregate. It coordinates
  movement state, table membership commands, and local presentation entry points.
- `GameTable` owns authoritative seat and spectator membership. Players request a
  specific physical seat index; state authority accepts it only while that chair
  is free. It does not own any card-game rules.
- `NetworkInputData` is the transport value for player intentions.

### Presentation

- `LobbyAvatarAnimator` adapts replicated movement state to the current humanoid
  placeholder. It can later be replaced by an Animator Controller without
  changing networking or table code.
- `RuntimeLobbyUI` builds the temporary connection interface.
- `LobbyPlayModePreview` provides an offline scene preview only.

### Editor

- `MainLobbySceneBuilder` creates the development lobby scene. Editor code must
  never be required at runtime.

## Authority flow

1. A local client samples input and submits `NetworkInputData`.
2. Fusion simulates the input-authoritative `NetworkPlayer`.
3. Table requests are sent to state authority.
4. `GameTable` validates membership and mutates replicated state.
5. Clients render replicated public state.

## Planned game boundary

The BANG! implementation should be introduced as its own assembly/module:

- Pure C# domain entities and rules with no Unity or Fusion dependencies.
- An authoritative network session adapter that translates commands into domain
  actions and publishes filtered projections.
- A private player projection containing that player's hand and role.
- A public projection containing only information players and spectators may see.
- Presentation components that render projections but never decide game rules.

Expansion content should register cards, characters, and rule modifiers through
catalogues/strategies rather than branching throughout the base-game engine.
