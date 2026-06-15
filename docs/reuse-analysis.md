# Reuse Analysis

## Purpose

This document identifies which parts of the current Project Z codebase can be reused for the airport management game and which inherited Project Y systems should be replaced.

The analysis follows the current architecture goals:

- Preserve reusable infrastructure where possible.
- Keep simulation independent from rendering.
- Avoid coupling UI to domain logic.
- Prefer small incremental replacements over broad rewrites.
- Do not modify rendering and simulation together in the same task.

## Current State Summary

The codebase still carries the original Project Y colony strategy model. The reusable foundation is strongest in map loading, turn orchestration, command execution, basic MonoGame rendering, input handling, and lightweight UI panels.

The least reusable areas are the colony, population, food, wood, farm, scout, and medieval-building concepts. These are game-specific and should be replaced gradually with airport concepts such as airports, runways, gates, terminals, flights, finances, and aggregate passenger demand.

One important technical note: most namespaces still use `ProjectZ`. That is not a blocker for reuse analysis, but it should eventually be cleaned up as a separate mechanical task.

## Reusable Infrastructure

### Strong Candidates To Keep

#### `Core.Map`

`Map` is a reusable tile-map container with width, height, and a two-dimensional tile array. The fixed map structure can support airport infrastructure placement without major changes.

Recommended reuse:

- Keep the map grid.
- Keep map dimensions and indexed tile access.
- Replace only terrain and tile contents as airport needs become clearer.

#### `Domain.Tile`

`Tile` is mostly reusable as the basic map cell. It currently stores terrain and a single building. The coordinate and terrain responsibilities are reusable; the single `Building` slot is only partially reusable.

Recommended reuse:

- Keep tile coordinates and terrain.
- Evolve the occupant model later if airport infrastructure needs multi-tile placement.
- Avoid turning `Tile` into a large airport object.

#### `Core.Maps.MapLoader` and `MapDefinition`

The JSON map loader is useful infrastructure. It validates dimensions and converts map text into terrain data. The colony and building sections are game-specific, but the loader pattern is reusable.

Recommended reuse:

- Keep JSON-backed starter maps.
- Keep validation logic.
- Replace `Colonies` and `Buildings` map sections with airport-specific initial state when Milestone 1 begins.

#### `Core.GameCommand`

The command pattern is a good fit for airport construction and player actions. The current base class is simple and easy to preserve.

Recommended reuse:

- Keep command execution through `GameManager.ExecuteCommand`.
- Rename or generalize `Cost` later because it currently assumes `ResourceCost`.
- Add airport commands incrementally, such as build runway, build gate, and build terminal.

#### `Core.GameManager`

`GameManager` is reusable as a coordinator for world state, commands, and simulation systems. The current registered systems are not reusable, but the orchestration pattern is.

Recommended reuse:

- Keep the manager role.
- Replace colony simulation systems with airport systems.
- Consider injecting the systems list later, but only when needed.

#### `Core.GameDate`

`GameDate` is reusable as a simple time model, although airport operations will likely need finer time than monthly turns.

Recommended reuse:

- Keep for early milestones if monthly turns are acceptable.
- Replace or extend with day/hour time when flight simulation begins.

#### `Core.TurnLog` and `TurnLogEntry`

The log is reusable for notifications, reports, and simulation feedback.

Recommended reuse:

- Keep the severity model.
- Reword messages for airport operations.
- Use it as a bridge toward the notification/log UX described in `docs/ui_ux.md`.

#### `Core.VisibilityMap` and `VisibilityState`

The visibility infrastructure is reusable if fog of war or map reveal remains in scope. For an airport management game, it may be less central than it was for a colony/exploration game.

Recommended reuse:

- Keep as optional map-state infrastructure.
- Do not prioritize it for airport MVP unless fog of war is intentionally retained.

## Reusable Rendering Systems

### Strong Candidates To Keep

#### `Presentation.MapRenderer`

`MapRenderer` contains reusable MonoGame map rendering logic: tile iteration, texture lookup, hex positioning, selection border drawing, and fog overlay drawing.

Reusable parts:

- Hex tile drawing.
- Hex pixel positioning.
- Selection border drawing.
- Fog overlay drawing.
- Texture fallback creation.

Game-specific parts to replace:

- Medieval terrain and building texture choices.
- Colony icon drawing.
- Farm and wood camp drawing.
- Building texture switch on `BuildingType`.

Recommended reuse:

- Keep `MapRenderer` as the tile renderer.
- Replace asset bindings with airport infrastructure textures.
- Split colony/building drawing into airport-focused rendering only when necessary.

#### `Presentation.InputHandler`

The input handler is reusable because it handles mouse-to-hex conversion, map bounds checks, tile selection, hover state, and action-menu opening.

Recommended reuse:

- Keep hex hit-testing.
- Keep selected and hovered tile behavior.
- Replace direct use of world flags only if a cleaner selection/context model is introduced later.

#### Camera And Zoom Logic In `Game1`

The zoom, pan, and camera transform behavior in `Game1` is reusable for any tile-based management game.

Recommended reuse:

- Keep camera zoom and panning behavior.
- Later extract it into a small camera class if it starts to grow.
- Avoid mixing camera refactoring with simulation replacement.

#### `Presentation.UnitRenderer`

`UnitRenderer` is only partially reusable. Its hex positioning and sprite placement pattern are reusable, but the `Scout` unit concept is not aligned with the airport roadmap.

Recommended reuse:

- Reuse the positioning approach.
- Replace the unit concept with aircraft, service vehicles, or remove it from MVP if not needed.

## Reusable UI Components

### Strong Candidates To Keep

#### `Presentation.TileActionMenu`

The action menu is reusable as a contextual command surface. It already builds commands for the selected tile and executes them through `GameManager`.

Reusable parts:

- Context menu lifecycle.
- Command list rendering.
- Disabled-command display.
- Command execution flow.

Game-specific parts to replace:

- `BuildingCatalog.All`.
- `BuildBuildingCommand`.
- Farm and wood camp actions.
- Portuguese construction label text if the airport UI is standardized in English.

Recommended reuse:

- Keep the command menu pattern.
- Populate it with airport construction commands.
- Add costs and validation feedback once airport finances exist.

#### `Presentation.TileInfoPanel`

The panel is reusable as a selected-object information surface, but its contents are colony-specific.

Recommended reuse:

- Keep the panel structure.
- Replace displayed data with airport infrastructure details.
- Follow `docs/ui_ux.md` by using it as the bottom or context information panel.

#### `Presentation.BottomInfoBar`

The bottom hover info bar is useful for map inspection. It is currently heavily tied to colony population and resources.

Recommended reuse:

- Keep hover-driven status display.
- Replace colony/resource output with terrain, infrastructure, validity, capacity, or cost information.

#### `Presentation.UI.ActionBarRenderer` and `ActionButton`

The action bar is a reusable start for a construction toolbar. It currently draws generic buttons and hover feedback, but it does not yet connect to commands.

Recommended reuse:

- Keep panel and button rendering.
- Add explicit airport construction actions later.
- Consider tooltips because the UI guidelines require all buttons to explain their purpose.

#### Gum UI Assets

The Gum project and UI panel textures are reusable as a UI framework foundation, although the current code mostly uses hand-drawn SpriteBatch UI.

Recommended reuse:

- Keep Gum assets available.
- Choose either Gum or lightweight SpriteBatch UI per feature; avoid mixing both inside the same small UI surface unless there is a clear reason.

## Reusable Simulation Systems

### Strong Candidates To Keep

#### `Simulation.ISimulationSystem`

The interface is reusable and matches the architecture: simulation systems update the world without rendering concerns.

Recommended reuse:

- Keep `ISimulationSystem`.
- Use it for `TimeSystem`, `FlightSystem`, `EconomySystem`, and later airline or demand systems.

#### Simulation Update Pipeline

The `GameManager.AdvanceTurn` loop over `_systems` is reusable. The existing concrete systems are not aligned with the airport game, but the pipeline is a good fit.

Recommended reuse:

- Keep ordered system updates.
- Replace the concrete registered systems.
- Keep log clearing and simulation feedback if turn-based reporting remains desirable.

### Conditional Reuse

#### `Simulation.VisibilitySystem`

The visibility system is partially reusable if the airport game keeps fog of war or map reveal. Its current reveal sources are colonies, so the logic is not directly reusable.

Recommended reuse:

- Keep the idea of a visibility updater.
- Replace colony reveal sources with airport-owned tiles, radar coverage, explored land, or remove it from MVP.

## Game-Specific Systems That Should Be Replaced

### Colony Model

Files:

- `Domain.Colony`
- `World.Colonies`
- `World.GetColonyAt`
- colony creation in `WorldFactory`
- colony display in `MapRenderer`, `TileInfoPanel`, `BottomInfoBar`, and `Game1`

Reason:

Colonies are not part of the airport management vision. They should be replaced with an `Airport` aggregate or similar domain object.

Replacement direction:

- `Colony` -> `Airport`
- `MapPosition` -> airport origin or owned tiles
- `Buildings` -> airport infrastructure
- `Resources` -> finances

### Population And Person Simulation

Files:

- `Domain.Population`
- `Domain.Person`
- `Simulation.PopulationSystem`

Reason:

The roadmap explicitly says not to simulate individual passengers for Milestone 6. Current population simulates people and aging, which does not match aggregate passenger demand.

Replacement direction:

- `Population` -> `PassengerDemand` or capacity metrics.
- Remove individual `Person` modeling for airport MVP.
- Use aggregated passenger counts and satisfaction later.

### Food Consumption

Files:

- `Simulation.FoodConsumptionSystem`

Reason:

Food consumption is colony survival logic and has no direct airport equivalent.

Replacement direction:

- Replace with operating expenses, facility upkeep, or passenger service demand when economy work begins.

### Resource Production

Files:

- `Simulation.ProductionSystem`
- `Domain.ResourceStock`
- `Domain.ResourceCost`
- production fields in `BuildingDefinition`

Reason:

Food, wood, and gold production are colony/resource-game mechanics. Airport economy should track cash, revenue, expenses, passenger throughput, and flight income.

Replacement direction:

- `ResourceStock` -> financial account or balance model.
- `ResourceCost` -> money-based construction and operating costs.
- Production -> flight revenue and expense systems.

### Buildings, Farms, And Wood Camps

Files:

- `Domain.Building`
- `Domain.BuildingType`
- `Domain.BuildingDefinition`
- `Core.Buildings.BuildingCatalog`
- `Core.Commands.BuildBuildingCommand`

Reason:

The existing building system is specific to farms and wood camps. The general idea of placeable infrastructure is reusable, but the model should become airport infrastructure.

Replacement direction:

- `Building` -> `Infrastructure` or airport-specific classes.
- `Farm` -> `Runway`, `Gate`, `Terminal`.
- Terrain requirement -> placement validation rules.
- Single-tile building -> eventually support multi-tile infrastructure.

### Units, Scouts, And Colonists

Files:

- `Domain.Unit`
- `Domain.UnitType`
- `Core.Factories.UnitFactory`
- `Presentation.UnitRenderer`
- `World.Units`

Reason:

Scout and colonist units belong to exploration/colony gameplay, not the airport MVP.

Replacement direction:

- Remove for Milestone 1 if not needed.
- Later replace with aircraft, service vehicles, or abstract flight entities.

### Starter Island Data

Files:

- `Data/Maps/starter_island.json`

Reason:

The map file contains colony and farm setup. The terrain grid is reusable as a test map, but initial entities should be replaced.

Replacement direction:

- Keep terrain format.
- Replace `colonies` and `buildings` with airport initial state.
- Rename the map once the airport theme is introduced.

### Project Y Naming

Files:

- Most C# namespaces.
- Console startup text in `Game1`.
- README command path.

Reason:

The project identity still references Project Y. This is not gameplay logic, but it will cause confusion as Project Z grows.

Replacement direction:

- Rename namespaces and startup text in a separate mechanical cleanup.
- Avoid combining namespace cleanup with domain or simulation changes.

## Recommended Reuse Order

1. Keep the map, command, game manager, log, and simulation interface intact while replacing concrete colony systems.
2. Introduce airport domain objects before changing UI labels broadly.
3. Replace construction commands with airport infrastructure commands.
4. Update rendering assets and info panels for airport infrastructure.
5. Replace production/population/food systems with time, flights, and economy systems.
6. Do namespace cleanup as a separate task after the domain direction is stable.

## Replacement Map

| Current Concept | Airport Direction | Reuse Level |
| --- | --- | --- |
| `Map` | Airport site map | High |
| `Tile` | Buildable land cell | High |
| `MapLoader` | Scenario/map loader | High |
| `GameCommand` | Player actions | High |
| `GameManager` | Game coordinator | High |
| `ISimulationSystem` | Airport simulation systems | High |
| `VisibilityMap` | Optional fog/known map state | Medium |
| `MapRenderer` | Airport map renderer | Medium |
| `TileActionMenu` | Construction/context menu | Medium |
| `TileInfoPanel` | Selected object info | Medium |
| `BottomInfoBar` | Hover/selection summary | Medium |
| `UnitRenderer` | Aircraft/service rendering pattern | Low |
| `Colony` | `Airport` | Replace |
| `Population` / `Person` | Aggregate demand metrics | Replace |
| Food/wood/gold resources | Cash/revenue/expenses | Replace |
| Farms/wood camps | Runways/gates/terminals | Replace |
| Scouts/colonists | Flights/aircraft/service vehicles | Replace |

