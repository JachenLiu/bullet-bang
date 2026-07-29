# Bullet Bang Known Issues and Follow-ups

Use this file for valuable findings that are outside the active ticket. Do not
silently expand a ticket to fix them.

## Open

No open issues are currently recorded.

## Resolved

Move entries here with the resolving ticket and commit once verified.

### KI-0002 — Large deletion required owner confirmation

- Found during: workflow setup
- Impact: committing without review could have recorded accidental removal of
  legacy gameplay code, data assets, prefabs, and a scene
- Evidence: the working tree contains hundreds of deleted paths
- Resolution: repository owner confirmed on 2026-07-29 that the deletions are
  intentional
- Resolved by: T0001 verification

### KI-0001 — Baseline runtime behavior was unverified

- Found during: workflow setup
- Impact: the lobby rewrite could not be treated as a stable baseline
- Evidence: import, compilation, and runtime checks had not yet been recorded
- Resolution: repository owner reported successful import, compilation, Play
  Mode, movement, host/join, seating, spectating, and disconnect checks with no
  errors on 2026-07-29
- Resolved by: T0001 verification

## Entry template

```markdown
### KI-#### — Short description

- Found during:
- Impact:
- Evidence:
- Recommended action:
- Resolved by:
```
