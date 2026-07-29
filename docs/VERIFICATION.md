# Bullet Bang Manual Verification

Record the date, build or commit, tester, and result whenever a section is run.
Use `Pass`, `Fail`, or `Blocked`; do not mark an unperformed check as passing.

## Result record

```text
Date:
Branch/commit:
Unity version:
Tester:
Checks run:
Result:
Notes:
```

## 1. Import and compile

1. Open the repository folder in Unity Hub with Unity `6000.0.75f1`.
2. Wait for package resolution and asset import to finish.
3. Open the Console and clear old messages.
4. Trigger a script recompile if Unity has not already done so.
5. Confirm there are no errors from project-owned code.
6. Record project-owned warnings separately from third-party warnings.

Expected result: Unity finishes importing and project-owned code compiles
without errors.

## 2. Offline lobby preview

1. Open the development lobby scene currently configured for the project.
2. Enter Play Mode without starting an external client.
3. Confirm the lobby environment appears.
4. Confirm the local placeholder avatar and temporary interface appear if the
   preview is designed to create them.
5. Exercise available movement and camera controls.
6. Exit Play Mode and check the Console for errors.

Expected result: the preview is usable and produces no project-owned errors.

## 3. Host session

1. Enter Play Mode in the main lobby scene.
2. Start a host session using the temporary interface.
3. Confirm a network player is spawned.
4. Move the player and observe the avatar presentation.
5. Check the Console.

Expected result: hosting succeeds, one local network player exists, and movement
does not produce project-owned errors.

## 4. Join and movement replication

1. Start a host.
2. Start a second instance using Unity Multiplayer Play Mode or a development
   build.
3. Join the same session.
4. Move each player in turn.
5. Confirm both instances see both players moving.
6. Check both Consoles or logs.

Expected result: both players join and movement is replicated consistently.

## 5. Table seats

1. With two connected players, request different free seats.
2. Confirm both players occupy the requested seats on both instances.
3. Have one player leave their seat.
4. Confirm the released seat becomes available.
5. Have both players request the same free seat as closely together as practical.

Expected result: state authority accepts only one conflicting request, and all
instances agree on occupancy.

## 6. Spectating

1. Join or switch to spectator mode using the available interface.
2. Confirm the spectator is not assigned a player seat.
3. Confirm public table membership and occupancy are visible.
4. Confirm no game-private information is exposed if any test data exists.

Expected result: spectators receive only public state.

## 7. Disconnect cleanup

1. Seat a connected client.
2. Close or disconnect that client.
3. Observe the host.
4. Confirm the player disappears and their seat becomes available.
5. Reconnect a client and claim the released seat.

Expected result: authority cleans up membership and the seat can be reused.

## 8. Ticket completion checklist

- Acceptance criteria were checked individually.
- Relevant automated tests or compilation checks passed.
- Required manual sections above were run and recorded.
- Project-owned warnings were resolved or explicitly documented.
- Out-of-scope findings were added to `KNOWN_ISSUES.md`.
- `CURRENT_STATE.md` contains observed results.
- `ARCHITECTURE.md` was updated if ownership or runtime flow changed.
