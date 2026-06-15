# Project Overview

Turn-based strategy game developed with MonoGame.

Architecture layers:
- Core
- Domain
- Simulation
- Presentation

The game uses:
- Tile-based world
- Colony system
- Resource production
- Command Pattern for player actions

Important rules:
- Keep simulation independent from rendering
- Avoid coupling UI with domain logic
- Prefer immutable command inputs
- Rendering must stay lightweight

Current implemented systems:
- Tile rendering
- Tile selection
- Colony hover info
- Farm construction
- Colony resources
- Action menu

Planned systems:
- Fog of war
- Roads
- Combat
- AI opponents
- Save/load
- Pathfinding

Coding conventions:
- Use small focused classes
- Avoid God objects
- Prefer composition over inheritance
- Keep MonoGame rendering code isolated
-Do not rewrite unrelated systems.
-Avoid changing architecture unless explicitly requested.
-Prefer minimal code changes.
-Never modify rendering and simulation together in the same task.