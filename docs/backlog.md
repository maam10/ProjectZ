# Backlog

## Purpose

This backlog breaks Milestone 1 into small implementation tasks for the airport foundations phase.

Milestone 1 goal:

The player can place airport infrastructure on the map.

Milestone 1 success criteria:

- Build runways.
- Build terminals.
- Build gates.
- Infrastructure occupies hex tiles.
- Infrastructure information panel.
- Construction costs.

Out of scope for Milestone 1:

- Flights.
- Economy simulation.
- Passengers.
- Airline contracts.
- Namespace cleanup.
- Large architecture rewrites.

## Working Rules

- Each task should be independently implementable.
- Each task should take less than one day.
- Prefer reuse of existing map, command, input, UI, and rendering infrastructure.
- Keep simulation and rendering changes in separate tasks.
- Keep the game playable after every task.
- Avoid replacing unrelated Project Y systems until a task explicitly calls for it.

## Milestone 1 Tasks

### M1-01: Add Airport Domain Entity

Create a minimal airport domain entity to replace the role currently played by colony data for the airport MVP.

Scope:

- Add an `Airport` domain model.
- Include a name.
- Include a map position or origin tile.
- Include a collection for placed infrastructure.
- Do not remove `Colony` yet.

Acceptance criteria:

- The project builds.
- An airport can be created in code with a name and map position.
- The airport model does not reference MonoGame rendering classes beyond existing coordinate conventions already used in domain code.
- No rendering code is changed.

### M1-02: Add Infrastructure Types

Create the airport infrastructure type model for Milestone 1.

Scope:

- Add infrastructure types for runway, terminal, and gate.
- Add a minimal infrastructure entity.
- Keep the model small and focused on identity and type.
- Do not add rendering behavior.

Acceptance criteria:

- The project builds.
- Runway, terminal, and gate are represented by domain types.
- Infrastructure can be associated with a map tile or tile position.
- No UI or rendering code is changed.

### M1-03: Add Infrastructure Definitions

Create reusable infrastructure definitions for construction metadata.

Scope:

- Add display name.
- Add construction cost.
- Add basic placement requirements.
- Define runway, terminal, and gate entries.
- Keep costs as construction data only; do not add economy simulation.

Acceptance criteria:

- The project builds.
- Each Milestone 1 infrastructure type has a definition.
- Each definition includes a display name and construction cost.
- Placement rules are represented as data or simple validation inputs.
- Existing farm and wood camp definitions remain untouched unless a later task replaces their usage.

### M1-04: Add Airport To World State

Add airport state to the world without removing the existing colony state.

Scope:

- Add an airport collection or single airport reference to `World`.
- Keep existing map, visibility, selected tile, hovered tile, and command state intact.
- Do not update rendering.

Acceptance criteria:

- The project builds.
- The world can contain at least one airport.
- Existing colony-based startup still works.
- No presentation files are changed.

### M1-05: Create Starter Airport In World Factory

Create an initial airport during world creation.

Scope:

- Add a starter airport near the current starting area.
- Keep existing starter colony setup until replacement tasks are ready.
- Avoid changing JSON map format in this task.

Acceptance criteria:

- The project builds.
- Starting a new world creates one airport.
- Existing map loading behavior remains unchanged.
- No rendering code is changed.

### M1-06: Add Infrastructure Placement On Tiles

Allow a tile to hold airport infrastructure separately from old colony buildings.

Scope:

- Add the minimum tile state needed for airport infrastructure placement.
- Preserve existing `Building` behavior for current gameplay until replaced.
- Do not add construction commands yet.

Acceptance criteria:

- The project builds.
- A tile can report whether it has airport infrastructure.
- A tile can place airport infrastructure once.
- Existing building placement still behaves as before.
- No presentation files are changed.

### M1-07: Add Build Infrastructure Command

Create a command for placing airport infrastructure on a selected tile.

Scope:

- Reuse the existing `GameCommand` flow.
- Create a command that places runway, terminal, or gate infrastructure.
- Validate that the tile is empty of airport infrastructure.
- Validate basic placement requirements.
- Do not connect the command to the UI yet.

Acceptance criteria:

- The project builds.
- The command can place infrastructure on a valid tile.
- The command refuses invalid terrain or occupied infrastructure tiles.
- The command writes a useful log entry after successful placement.
- No rendering code is changed.

### M1-08: Add Construction Cost Model For Airport Infrastructure

Add a construction cost concept suitable for airport infrastructure.

Scope:

- Represent money-based construction costs.
- Attach costs to infrastructure definitions.
- Keep this separate from economy simulation.
- Do not add revenue, expenses, or monthly reports.

Acceptance criteria:

- The project builds.
- Each Milestone 1 infrastructure definition has a money cost.
- Build command acceptance checks can read the cost.
- No recurring cost or income simulation is introduced.

### M1-09: Add Basic Airport Funds

Give the airport or world a simple construction budget for Milestone 1.

Scope:

- Add a current cash or funds value.
- Deduct construction cost when infrastructure is built.
- Prevent construction when funds are insufficient.
- Do not add economy simulation systems.

Acceptance criteria:

- The project builds.
- A starting budget exists.
- Successful construction deducts the correct cost.
- Construction is unavailable when funds are too low.
- No simulation system is added.
- No rendering code is changed.

### M1-10: Replace Action Menu Construction Entries

Update the tile action menu to offer airport infrastructure construction commands.

Scope:

- Replace or hide farm and wood camp actions in the tile action menu.
- Show runway, terminal, and gate construction actions.
- Keep the existing action menu structure.
- Do not change map rendering in this task.

Acceptance criteria:

- The project builds.
- Selecting a visible tile opens construction options for runway, terminal, and gate.
- Disabled actions appear unavailable when placement or funds are invalid.
- Choosing a valid action places infrastructure through `GameManager.ExecuteCommand`.
- Farm and wood camp construction are no longer presented in the airport construction menu.

### M1-11: Render Airport Infrastructure

Draw placed airport infrastructure on the map.

Scope:

- Add temporary or existing content textures for runway, terminal, and gate.
- Reuse `MapRenderer` tile iteration and hex positioning.
- Draw only infrastructure that exists on tiles.
- Do not change construction command logic.

Acceptance criteria:

- The project builds.
- Built runways, terminals, and gates are visible on the map.
- Existing tile rendering and selection still work.
- Rendering remains isolated in presentation code.
- No simulation or command files are changed.

### M1-12: Update Selected Tile Info Panel

Show airport infrastructure details in the selected tile information panel.

Scope:

- Display infrastructure type.
- Display construction cost or basic definition data.
- Keep terrain and coordinate display.
- Remove or de-emphasize colony details from the selected tile view if airport infrastructure exists.

Acceptance criteria:

- The project builds.
- Selecting a tile with runway, terminal, or gate shows its infrastructure type.
- Selecting an empty tile still shows coordinates and terrain.
- No simulation or command files are changed.

### M1-13: Update Bottom Hover Info

Update the bottom info bar for airport-focused hover information.

Scope:

- Show tile coordinates and terrain.
- Show airport infrastructure summary when hovering infrastructure.
- Show construction-relevant information if available.
- Avoid population, food, wood, and gold as primary hover information.

Acceptance criteria:

- The project builds.
- Hovering infrastructure shows airport-relevant details.
- Hovering empty known terrain still shows terrain information.
- Hidden or unknown tile behavior remains stable if visibility is still enabled.
- No simulation or command files are changed.

### M1-14: Update Top Status Bar For Airport Foundations

Replace colony resource status with airport foundation status.

Scope:

- Show date if still useful.
- Show available funds.
- Show counts for runways, terminals, and gates.
- Do not add flight, passenger, or economy metrics.

Acceptance criteria:

- The project builds.
- The top bar no longer depends on colony population, food, wood, or gold.
- Counts update after infrastructure construction.
- No simulation or command files are changed.

### M1-15: Add Infrastructure Placement Rules

Tighten placement validation for Milestone 1 infrastructure.

Scope:

- Define simple terrain restrictions.
- Prevent placing infrastructure on water.
- Prevent placing multiple infrastructure pieces on the same tile.
- Keep multi-tile runway placement out of scope unless already trivial.

Acceptance criteria:

- The project builds.
- Runways, terminals, and gates cannot be placed on water.
- Infrastructure cannot be placed on occupied infrastructure tiles.
- Invalid actions are disabled in the menu.
- No rendering code is changed.

### M1-16: Add Basic Infrastructure Counts

Add a small query or helper for counting placed airport infrastructure.

Scope:

- Count runways.
- Count terminals.
- Count gates.
- Keep the helper in domain or core, not presentation.

Acceptance criteria:

- The project builds.
- Counts are available without presentation code scanning tiles directly.
- Counts match infrastructure placed through commands.
- No rendering code is changed.

### M1-17: Update Starter Map Data For Airport Scenario

Update starter data so the initial world is framed as an airport site instead of a colony scenario.

Scope:

- Replace colony-specific starter setup with airport-specific starter setup.
- Keep terrain map loading.
- Keep this focused on data and loading only.

Acceptance criteria:

- The project builds.
- New game startup uses airport initial data.
- No farm or colony is required for the player to start building airport infrastructure.
- Existing terrain grid still loads successfully.
- No presentation files are changed.

### M1-18: Remove Milestone 1 Colony Construction Dependency

Remove remaining dependency on colony resources for airport construction.

Scope:

- Ensure airport construction does not call `world.Colonies.First()`.
- Ensure airport construction does not read food, wood, or gold.
- Leave old colony classes in place if still referenced elsewhere.

Acceptance criteria:

- The project builds.
- Runway, terminal, and gate construction works without a colony.
- Construction availability depends on airport placement rules and funds.
- No rendering code is changed.

### M1-19: Hide Or Disable Legacy Colony Simulation For Airport MVP

Stop old colony systems from affecting the airport MVP.

Scope:

- Remove or disable production, food consumption, and population systems from the active Milestone 1 simulation list.
- Keep `ISimulationSystem` and the update pipeline.
- Do not delete old classes in this task.

Acceptance criteria:

- The project builds.
- Advancing a turn no longer changes food, wood, gold, or colony population for MVP gameplay.
- The simulation pipeline still runs without errors.
- No rendering code is changed.

### M1-20: Add Build Verification Checklist

Create a lightweight manual verification checklist for Milestone 1.

Scope:

- Document startup verification.
- Document tile selection verification.
- Document each construction action.
- Document invalid placement cases.
- Document UI information checks.

Acceptance criteria:

- A checklist exists in docs or in this backlog.
- The checklist covers runway, terminal, and gate construction.
- The checklist covers insufficient funds and invalid terrain.
- No source code is changed.

## Suggested Implementation Order

1. M1-01: Add Airport Domain Entity.
2. M1-02: Add Infrastructure Types.
3. M1-03: Add Infrastructure Definitions.
4. M1-04: Add Airport To World State.
5. M1-05: Create Starter Airport In World Factory.
6. M1-06: Add Infrastructure Placement On Tiles.
7. M1-08: Add Construction Cost Model For Airport Infrastructure.
8. M1-09: Add Basic Airport Funds.
9. M1-07: Add Build Infrastructure Command.
10. M1-15: Add Infrastructure Placement Rules.
11. M1-10: Replace Action Menu Construction Entries.
12. M1-11: Render Airport Infrastructure.
13. M1-12: Update Selected Tile Info Panel.
14. M1-13: Update Bottom Hover Info.
15. M1-16: Add Basic Infrastructure Counts.
16. M1-14: Update Top Status Bar For Airport Foundations.
17. M1-18: Remove Milestone 1 Colony Construction Dependency.
18. M1-19: Hide Or Disable Legacy Colony Simulation For Airport MVP.
19. M1-17: Update Starter Map Data For Airport Scenario.
20. M1-20: Add Build Verification Checklist.

## Milestone 1 Completion Definition

Milestone 1 is complete when:

- The player can build runways, terminals, and gates from the UI.
- Built infrastructure occupies hex tiles.
- Invalid placement is prevented.
- Construction costs are visible or discoverable before building.
- Construction deducts from a simple airport budget.
- Selected and hovered infrastructure show airport-relevant information.
- Legacy colony resource systems no longer drive the airport MVP loop.
- The game remains playable from startup through multiple construction actions.

