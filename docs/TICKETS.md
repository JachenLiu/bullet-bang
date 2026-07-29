# Bullet Bang Tickets

## Status meanings

- `Draft`: incomplete or not yet approved.
- `Ready`: safe to begin.
- `In progress`: the only active implementation ticket.
- `Blocked`: requires a decision or external action.
- `Verification`: implementation is done but checks remain.
- `Done`: acceptance criteria and required verification passed.

## T0001 — Verify the current lobby baseline

**Status:** Done

### Goal

Turn the current large, uncommitted cleanup and lobby rewrite into a known,
reviewable baseline without losing intentional work.

### Dependencies

None.

### Allowed areas

- Existing uncommitted changes
- `Assets/Bang/BulletBang/`
- Relevant lobby scenes and prefabs
- `Packages/`
- `ProjectSettings/`
- Project documentation

### Do not touch

- Third-party source code unless compilation proves it is necessary
- New BANG! gameplay rules
- Expansion content
- Unrelated art replacements

### Requirements

- Review the repository diff at a useful summary level.
- Confirm whether the large legacy-code and data deletion is intentional.
- Open the project with Unity `6000.0.75f1`.
- Allow Unity to import and report all custom-code errors and warnings.
- Verify every currently available lobby flow described in `VERIFICATION.md`.
- Record failures in `KNOWN_ISSUES.md`.
- Update `CURRENT_STATE.md` with observed facts.

### Non-goals

- Implementing missing card-game features
- Production UI
- Broad refactoring
- Automatically fixing every discovered problem

### Acceptance criteria

- The user confirms that the current deletion scope is intentional.
- Unity finishes importing.
- Custom code compiles, or each blocking error is recorded and handled by a
  narrowly scoped follow-up ticket.
- Completed manual checks and their results are recorded.
- The baseline changes are ready for an intentional commit.

### Manual verification

Run sections 1 and 2 of `VERIFICATION.md`, followed by any multiplayer checks
that the current scene supports.

### Verification record

- 2026-07-29: repository owner confirmed the large legacy-code and asset
  deletion is intentional.
- 2026-07-29: repository owner reported that Unity `6000.0.75f1` completed its
  import with no Console errors.
- 2026-07-29: repository owner reported that Play Mode, movement, hosting,
  joining, table seating, spectating, and disconnect behavior all passed with
  no errors.

## Candidate follow-up tickets

These are placeholders and must be refined after T0001:

- `T0002` — Fix one specific baseline blocker, if T0001 finds one.
- `T0003` — Harden authoritative table seat release on disconnect.
- `T0004` — Define the pure C# BANG! module boundary and assemblies.
- `T0005` — Add the smallest tested match-state model.

Do not implement candidate tickets until their scope, boundaries, acceptance
criteria, and verification steps are written.

## Ticket template

```markdown
## T#### — Short title

**Status:** Draft

### Goal
One observable outcome.

### Dependencies
Earlier tickets or required decisions.

### Allowed areas
Files, folders, or modules that may change.

### Do not touch
Explicit exclusions.

### Requirements
What must be implemented.

### Non-goals
Tempting adjacent work that is excluded.

### Acceptance criteria
Objective conditions for completion.

### Manual verification
Short steps a person can perform.
```
