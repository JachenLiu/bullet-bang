# Bullet Bang Engineering Standards

These rules apply to every future code change in this repository.

## Architecture

- Keep the generic social lobby independent from every playable game.
- Put networking orchestration in `Network`, world presentation in `Environment`,
  interface code in `UI`, editor-only tooling in `Editor`, and reusable domain
  logic in a game-specific module.
- Give each class one primary responsibility. Split input, movement, camera,
  presentation, networking, and game rules when they can change independently.
- Depend on abstractions at module boundaries. Do not make game rules depend on
  Unity scenes, UI widgets, Photon callbacks, or visual prefabs.
- Keep the server authoritative. Clients submit intentions; authority validates
  and changes replicated state.

## Object-oriented design

- Prefer composition over inheritance except where Unity or Fusion requires a
  framework base class.
- Keep fields private and expose the smallest useful public API.
- Use immutable values and read-only properties where practical.
- Avoid global state. If local process state is necessary, isolate it behind one
  documented class.
- Do not create catch-all manager classes. Name classes after the responsibility
  they own.

## Documentation

- Add XML documentation to public types and public APIs.
- Explain intent, authority, lifecycle, invariants, and non-obvious tradeoffs.
- Do not add comments that merely restate the code.
- Update `Assets/Bang/BulletBang/ARCHITECTURE.md` when module ownership or an
  important runtime flow changes.

## Quality

- Preserve existing behavior unless the task explicitly changes it.
- Compile after code changes and test the affected Unity flow where possible.
- Treat warnings in custom code as work to resolve; identify third-party warnings
  separately.
- Keep placeholders replaceable through stable interfaces and serialized
  references.

## Ticket workflow

- Implement one identified ticket at a time.
- Before editing, read `docs/CURRENT_STATE.md` and the complete ticket in
  `docs/TICKETS.md`.
- Stay inside the ticket's allowed areas and do not touch excluded systems.
- Do not implement non-goals, future tickets, or unrelated refactors.
- Record useful out-of-scope discoveries in `docs/KNOWN_ISSUES.md` instead of
  fixing them automatically.
- Preserve user changes already present in the working tree.
- Compile after code changes and run the ticket's verification checks where
  possible.
- Do not mark a ticket complete until its acceptance criteria are satisfied.
- Update `docs/CURRENT_STATE.md` after a ticket is verified.
- End implementation work with a report containing the summary, changed files,
  validation results, remaining manual checks, risks, and follow-ups.
