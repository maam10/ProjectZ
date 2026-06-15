# ProjectY Architecture

ProjectY is a small MonoGame DesktopGL strategy-game prototype. The codebase is organized around a turn-based world model, a set of simulation systems that mutate that world, and presentation classes that translate MonoGame input and rendering into game interactions.

## Technology Stack

- .NET 8 application targeting `Exe`
- MonoGame DesktopGL for the game loop, rendering, input, and content pipeline
- MonoGame Content Builder for textures and the sprite font listed in `Content/Content.mgcb`

## High-Level Runtime Flow

```text
Program.cs
  -> Game1.Run()
      -> Initialize()
          -> create GameManager through GameBootstrap, plus renderers and UI/input helpers
      -> LoadContent()
          -> load sprite font and textures through MonoGame ContentManager
      -> Update()
          -> process keyboard and mouse input
          -> optionally advance the simulation by one turn
          -> update tile action menu commands
      -> Draw()
          -> render map, selection, buildings, UI, colony stats, and turn log
```

`Game1` owns the MonoGame lifecycle and presentation wiring. World/scenario bootstrapping lives in `GameBootstrap` and `WorldFactory`.

## Project Structure

```text
Core/
  Buildings/
  Commands/
  Maps/
  GameBootstrap.cs
  GameCommand.cs
  GameDate.cs
  GameManager.cs
  Map.cs
  TurnLog.cs
  TurnLogEntry.cs
  VisibilityMap.cs
  VisibilityState.cs
  World.cs

Domain/
  Building.cs
  BuildingDefinition.cs
  BuildingType.cs
  Colony.cs
  Person.cs
  Population.cs
  ResourceCost.cs
  ResourceStock.cs
  Tile.cs

Simulation/
  FoodConsumptionSystem.cs
  ISimulationSystem.cs
  PopulationSystem.cs
  ProductionSystem.cs
  VisibilitySystem.cs

Presentation/
  BottomInfoBar.cs
  InputHandler.cs
  MapRenderer.cs
  TileActionMenu.cs
  TileInfoPanel.cs

Content/
  Content.mgcb
  *.png
  Font.spritefont

Data/
  Maps/
  starter_island.json
```

## Layer Responsibilities

### Entry Point and Composition

- `Program.cs` creates `ProjectY.Game1` and starts the MonoGame run loop.
- `GameBootstrap` creates the initial `GameManager` from the starter world.
- `Game1.cs` owns MonoGame lifecycle methods:
  - `Initialize` creates rendering, UI/input helpers, and requests a ready `GameManager` from `GameBootstrap`.
  - `LoadContent` loads textures and fonts.
  - `Update` handles per-frame input and turn advancement.
  - `Draw` renders the game scene and UI.

World and scenario creation live outside `Game1`. `GameBootstrap` creates the `GameManager`, while `WorldFactory` creates the starter world from map data or fallback defaults.

### Core

The `Core` namespace contains application/game orchestration concepts.

- `World` is the central mutable game state. It owns:
  - the `Map`
  - the `VisibilityMap`
  - active `Colonies`
  - current `GameDate`
  - current-turn `TurnLog`
  - UI-facing transient state such as `SelectedTile`, `HoveredTile`, and `IsActionMenuOpen`
- `Map` owns the tile grid and currently generates a random `20 x 15` map with grass, forest, mountain, and water terrain.
- `WorldFactory` creates the starter world. It first tries to load `Data/Maps/starter_island.json`; if loading fails, it falls back to a random map.
- `Core/Maps` contains JSON map definitions and loading code.
- `GameManager` coordinates game actions:
  - `AdvanceTurn` clears the log, runs each simulation system, then advances the date.
  - `ExecuteCommand` validates and executes a `GameCommand`.
- `GameDate` tracks month and year.
- `TurnLog` and `TurnLogEntry` collect messages shown after each turn.
- `VisibilityMap` tracks whether each tile is hidden, explored, or currently visible.
- `GameCommand` is the base class for player actions.
- `BuildingCatalog` is the central list of buildable building definitions, including display name, required terrain, cost, and production.
- `BuildBuildingCommand` is the generic command implementation for constructing buildings from `BuildingDefinition` data. It checks terrain and resources, deducts cost, places the building on the tile, adds it to the colony, and logs the result.

### Domain

The `Domain` namespace contains game entities and value-like models.

- `Tile` represents a map cell with coordinates, terrain, and an optional building.
- `TerrainType` defines supported terrain: `Grass`, `Forest`, `Mountain`, and `Water`.
- `Colony` represents a settlement on the map and owns population, resources, buildings, and a map position.
- `BuildingType` defines supported building types.
- `BuildingDefinition` describes the rules and data for a building type.
- `Building` represents a placed building instance and stores its `BuildingType`.
- `Population` tracks individual inhabitants and their ages.
- `Person` represents one inhabitant and stores age in months.
- `ResourceStock` tracks food, wood, and gold quantities.
- `ResourceCost` describes resource costs for commands.

These classes are intentionally simple and mostly behavior-light. Simulation systems and commands currently contain most game rules.

### Simulation

The `Simulation` namespace contains turn-based systems. Each system implements:

```csharp
void Update(World world);
```

Current systems are registered directly in `GameManager`:

- `ProductionSystem` sums resource production from buildings placed on the map and adds it to colony resources.
- `FoodConsumptionSystem` subtracts 1 food per inhabitant from each colony every turn. If the colony does not have enough food, food drops to zero and a warning is logged.
- `PopulationSystem` calculates colony growth from current conditions, adds new inhabitants, advances every inhabitant by one month, and removes inhabitants that reach old age. The base growth rate is 5%, it drops to 0% when the colony has no food available, starting inhabitants are randomly aged between 5 and 30 years, and the current old-age threshold is 80 years.
- `VisibilitySystem` updates fog of war. Currently, each colony reveals tiles in a radius of 3. Previously visible tiles become explored when they are no longer in sight.

Systems run sequentially in the order registered by `GameManager`: production, food consumption, population growth/aging, then visibility. If future systems depend on each other, this ordering becomes part of the game rules and should be documented near the registration point.

### Presentation

The `Presentation` namespace contains MonoGame-facing input and rendering.

- `InputHandler` reads mouse and keyboard state:
  - left click selects a tile and closes the action menu
  - right click selects a tile and opens the action menu
  - hover updates `World.HoveredTile`
  - Escape closes the action menu
- `MapRenderer` draws terrain, selection highlight, colonies, buildings, and fog-of-war overlays. It only draws colonies, buildings, and selection highlights on currently visible tiles.
- `TileActionMenu` builds available commands for the selected tile and executes clicked commands through `GameManager`.
- `BottomInfoBar` displays hovered tile or colony information.
- `TileInfoPanel` can display selected tile details, but its draw call is currently commented out in `Game1.Draw`.

Presentation classes currently read and write some transient UI state directly on `World`. That keeps the prototype small, but it also mixes persistent game state with UI interaction state.

Hidden tiles do not expose terrain details in the bottom info bar and cannot be selected for actions. Explored tiles show terrain, but not current colony information.

## Content Pipeline

Runtime assets are declared in `Content/Content.mgcb` and loaded by name through MonoGame's `ContentManager`.

Current assets include:

- terrain textures: `grass`, `forest`, `mountain`, `water`
- overlays/icons: `highlight`, `colony`, `farm`
- font: `Font`

`MapRenderer`, `TileActionMenu`, `TileInfoPanel`, and `BottomInfoBar` each load the assets they need in their `LoadContent` methods.

## Map Data

Starter map data lives in `Data/Maps/starter_island.json` and is copied to the build output. The map uses one string per row, with one character per tile:

- `G` = grass
- `F` = forest
- `M` = mountain
- `W` = water

The same file can also define starting colonies and building placements. Building entries reference a building type, while construction costs and production values come from `BuildingCatalog`. `WorldFactory.CreateStarterWorld` loads this file at startup and writes a terminal message indicating whether the map loaded successfully or the game fell back to random generation.

## Turn Lifecycle

The main turn advancement is triggered in `Game1.Update` when Space is down:

```text
Space key pressed
  -> GameManager.AdvanceTurn()
      -> World.Log.Clear()
      -> ProductionSystem.Update(world)
      -> FoodConsumptionSystem.Update(world)
      -> PopulationSystem.Update(world)
      -> VisibilitySystem.Update(world)
      -> World.Date.Advance()
```

`Game1` tracks the previous keyboard state, so Space advances the turn only on the transition from released to pressed.

## Command Flow

The current player action flow is:

```text
Right-click tile
  -> InputHandler sets World.SelectedTile and World.IsActionMenuOpen
  -> TileActionMenu.BuildCommands creates commands for selected tile
  -> player clicks command
  -> TileActionMenu calls GameManager.ExecuteCommand(command)
  -> GameManager validates command.CanExecute(world)
  -> command.Execute(world) mutates world and writes to turn log
```

`BuildBuildingCommand` handles construction for all entries in `BuildingCatalog`. New building types usually do not need a new command class.

## Key Design Decisions

- The world model is mutable and shared across systems, commands, and presentation.
- Turn rules are modeled as small systems behind `ISimulationSystem`.
- Player actions are modeled as `GameCommand` objects with explicit validation and execution.
- Rendering is separated from domain entities; textures are selected in `MapRenderer`.
- The first implementation favors direct wiring over dependency injection or a scene framework.

## Current Constraints and Risks

- `BuildBuildingCommand` uses `world.Colonies.First()`, so construction commands assume a single-colony MVP.
- `World` contains both persistent game state and UI state.
- Fallback map generation is random and unseeded, making fallback runs non-deterministic.
- The action menu rebuilds command objects each update while open.
- Building construction adds a building to both the selected tile and the colony's building list, but production is calculated from map tiles.

## Extension Points

To add a new turn rule:

1. Create a class in `Simulation/` that implements `ISimulationSystem`.
2. Add it to `_systems` in `GameManager`.
3. Document ordering requirements if it depends on another system.

To add a new player action:

1. Create a `GameCommand` subclass in `Core/Commands/`.
2. Implement `Label`, `Cost`, `CanExecute`, and `Execute`.
3. Add the command to `TileActionMenu.BuildCommands`.
4. Add any needed art to `Content/Content.mgcb` and rendering support to `MapRenderer`.

For standard building construction, prefer adding a `BuildingDefinition` to `BuildingCatalog` instead of creating a new command class.

To add a new terrain or building type:

1. Add the building enum value to `BuildingType`.
2. Add a `BuildingDefinition` entry to `BuildingCatalog`.
3. Add or update content assets if the building needs unique art.
4. Update texture selection in `MapRenderer`.
5. Update specialized simulation rules only if the building does something beyond standard resource production.

## Recommended Next Architecture Improvements

- Split UI interaction state out of `World` once the UI becomes more complex.
- Add deterministic map generation using an explicit seed.
- Add tests around simulation systems and command validation once rules become more complex.
