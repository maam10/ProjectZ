# UI / UX Guidelines

# General Principles

- Keep the interface minimal and readable.
- Avoid excessive UI windows.
- Prioritize map visibility.
- Information should appear contextually.
- Mouse interactions must feel responsive.

# Visual Style

- Clean strategy-game inspired interface.
- Neutral dark panels with light text.
- Minimal animations.
- Pixel-perfect rendering when possible.
- UI should not obstruct gameplay area.

# Tile Interaction

## Hover

When hovering a tile:
- Highlight the hovered tile.
- Display tile information in the bottom hover info bar.
- Show:
  - terrain type
  - colony ownership
  - available resources
  - buildings

Hover must NOT change game state.

## Selection

Left click:
- Select tile.
- Open contextual action menu when the tile is visible.
- Persist selection until another tile is selected.

Selection should:
- Have stronger highlight than hover.
- Open contextual information panel.

# Colony UX

When hovering or selecting a colony:
- Show colony name.
- Show stored resources.
- Show population.
- Show production summary.

Resource information should be easy to scan quickly.

# Contextual Menus

Right click drags/pans the map.

Left click opens action menu on visible tiles.

Action menu rules:
- Only show valid actions.
- Disabled actions should explain why unavailable.
- Keep menu compact.
- Close automatically after action selection.

# HUD

HUD areas:
- Top bar:
  - turn number
  - global resources
- Bottom bar:
  - hover tile information
- Side panel:
  - selected tile/colony details

# Feedback

Player actions must always provide feedback:
- visual highlight
- sound effect (future)
- tooltip or message when invalid

# Camera

- Smooth camera movement.
- Mouse wheel zoom (future).
- Camera boundaries should prevent showing outside map.

# Performance Rules

- UI rendering must remain lightweight.
- Avoid allocating objects during rendering.
- Avoid recalculating static UI every frame.

# Accessibility

- Text must remain readable at all zoom levels.
- Avoid color-only indicators.
- Important selections should have shape/border indicators.

# Future UX Features

Planned:
- Minimap
- Production overview screen
- Colony management screen
- Trade route visualization
- Fog of war visual feedback
- Unit movement preview
