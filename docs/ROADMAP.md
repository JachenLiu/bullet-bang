# Bullet Bang Roadmap

## Product goal

Build a server-authoritative multiplayer social lobby that can host independent
table games, beginning with BANG!. Players can join the lobby, move between
tables, sit or spectate, and play without clients receiving hidden information.

## Architectural boundary

- The social lobby remains independent from BANG! rules.
- BANG! rules live in a pure C# game-specific domain module.
- Fusion adapters validate player intentions and replicate approved state.
- UI and world objects present state but never decide game rules.
- Public and private projections prevent hidden cards and roles from leaking.

See `Assets/Bang/BulletBang/ARCHITECTURE.md` for current ownership and runtime
flows.

## Milestones

### M0 — Establish a trustworthy baseline

- Reconcile the current repository cleanup and lobby rewrite.
- Confirm that Unity imports and compiles the project.
- Manually verify the offline preview and available multiplayer lobby flows.
- Commit the verified baseline before starting normal feature tickets.

### M1 — Generic social lobby

- Host and join a Fusion session.
- Spawn and move networked lobby avatars.
- Claim and release physical table seats authoritatively.
- Support spectators and disconnection cleanup.
- Replace temporary presentation through stable boundaries when needed.

### M2 — BANG! domain foundation

- Create a Unity- and Fusion-independent C# module.
- Represent players, roles, characters, cards, deck zones, and match phases.
- Validate commands and state transitions deterministically.
- Cover core rules with edit-mode tests.

### M3 — Authoritative network match

- Attach a BANG! session adapter to a generic table.
- Accept client intentions and validate them on state authority.
- Publish separate public and per-player private projections.
- Support reconnect and spectator-safe state.

### M4 — Base-game playable slice

- Start a match with supported player counts.
- Assign roles and characters.
- Deal cards and run the turn sequence.
- Implement a small vertical slice of cards before expanding the catalogue.
- Detect elimination and victory.

### M5 — Production presentation

- Replace temporary lobby UI.
- Add match UI, targeting, feedback, and accessibility affordances.
- Add resilient loading, error, and reconnect states.

### Later

- Complete the base-game catalogue.
- Add expansion content through catalogues and strategies.
- Add persistence, matchmaking improvements, moderation, and polish only after
  the base loop is reliable.

## Planning rule

Only the next few tickets should be detailed. Later milestones are direction,
not promises. Update this roadmap when evidence from implementation changes the
plan.
