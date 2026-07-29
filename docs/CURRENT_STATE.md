# Bullet Bang Current State

Last inspected: 2026-07-29

## Repository

- Branch: `main`
- HEAD at inspection: `43d26dc`
- Working tree: not clean
- Approximate changed paths at inspection: 508
- The verified baseline is ready to commit.

The working tree contains a substantial in-progress cleanup and lobby rewrite:
hundreds of legacy scripts, assets, prefabs, and a scene are deleted; lobby,
network, editor, package, and project-setting files are modified; new lobby and
placeholder assets are untracked. These changes predate the workflow documents.
On 2026-07-29, the repository owner confirmed that the large legacy-code and
asset deletion is intentional.

## Toolchain

- Unity: `6000.0.75f1`
- Photon Fusion: `2.1.1`
- Unity Multiplayer Play Mode package: `1.6.3`

## Current architecture

The active Bullet Bang folders are:

- `Editor` — development-only scene construction
- `Environment` — world and avatar presentation
- `Network` — Fusion lifecycle, input, players, and table membership
- `UI` — runtime interface presentation

The intended game boundary is documented in
`Assets/Bang/BulletBang/ARCHITECTURE.md`. BANG! rules should later live in a
separate pure C# module.

## Observed implementation

Based on repository inspection, the current rewrite includes:

- Fusion runner and lobby orchestration
- Replicated network players and player intentions
- Authoritative table seat and spectator membership
- Temporary runtime lobby UI
- Offline lobby preview
- Development scene-building tools

These statements describe code structure, not verified runtime behavior.

## Validation status

- Unity import: passed; owner reported a successful import on 2026-07-29
- Custom-code compilation: passed; owner reported no Unity Console errors
- Offline preview: passed
- Host/client connection: passed
- Movement replication: passed
- Seat and spectator behavior: passed
- Disconnect cleanup: passed

The runtime results above were reported by the repository owner on 2026-07-29.

## Completed tickets

- `T0001 — Verify the current lobby baseline`

## Active ticket

None. Select and refine the next candidate ticket after the baseline commit.

## Known risks

- The current change is too large to treat as a routine feature ticket.
- The baseline verification is manual; automated lobby-flow coverage does not
  yet exist.

## Next action

Commit the verified baseline, then refine one candidate follow-up ticket before
starting more implementation.
