# UI / UX Guidelines

## Purpose

The user interface should support management gameplay by making operational information easy to access and understand.

The UI should feel professional, functional and data-driven rather than decorative.

Inspirations:

* Airport CEO
* SimAirport
* OpenTTD
* Transport Fever
* Cities Skylines

---

# Core Principles

1. Information first.
2. Minimize unnecessary clicks.
3. Keep the map visible.
4. Consistent interaction patterns.
5. Reveal complexity gradually.

---

# Screen Layout

## Main Layout

```text
+--------------------------------------------------+
| Top Status Bar                                   |
+--------------------------------------------------+
|                                                  |
|                                                  |
|                                                  |
|                 Map Area                         |
|                                                  |
|                                                  |
|                                                  |
+----------------------+---------------------------+
| Bottom Info Panel    | Right Context Panel       |
+----------------------+---------------------------+
```

The map should occupy most of the screen.

---

# Top Status Bar

Displays high-level information.

Example:

```text
Time: 08:30

Cash: $125,000

Flights Today: 24

Passengers: 1,450

Reputation: 72%
```

Rules:

* Always visible.
* Compact.
* Single-line layout.

---

# Bottom Info Panel

Displays information about the currently selected object.

Examples:

* Airport
* Runway
* Gate
* Terminal
* Flight

Should update immediately after selection.

---

# Right Context Panel

Displays actions available for the selected object.

Examples:

Runway:

* Rename
* Upgrade
* Close

Flight:

* Details
* Prioritize
* Cancel

Airport:

* Finances
* Expansion
* Statistics

---

# Construction Mode

Construction should follow a simple workflow:

```text
Select Tool
      ↓
Preview Placement
      ↓
Validate Placement
      ↓
Confirm Build
```

Rules:

* Green = valid placement
* Red = invalid placement
* Cost displayed before confirmation

---

# Selection Feedback

Selected tiles must always be obvious.

Requirements:

* Highlight border
* Persistent until deselected
* Distinct from hover state

Hover and selection must never use the same visual style.

---

# Tooltips

All buttons should provide tooltips.

Tooltips should answer:

* What is this?
* What does it do?
* Why would I use it?

Example:

```text
Runway

Allows aircraft to land and take off.

Capacity:
20 flights/day
```

---

# Information Hierarchy

Priority order:

1. Critical alerts
2. Financial information
3. Flight operations
4. Airport statistics
5. Historical data

Players should never search for critical information.

---

# Colors

Use color sparingly.

Recommended meanings:

Green

* Positive
* Profitable
* Available

Yellow

* Warning
* Near capacity

Red

* Error
* Congestion
* Negative balance

Blue

* Informational

Do not rely exclusively on color.

Always provide icons or text.

---

# Notifications

Notifications should be concise.

Good:

```text
Flight AZ123 delayed.
```

Bad:

```text
A delay has occurred due to operational constraints.
```

Notifications should disappear automatically.

Important notifications should remain accessible in a log.

---

# MVP UI Scope

Initial version should contain only:

* Top status bar
* Construction menu
* Tile selection
* Information panel
* Basic notifications

Avoid:

* Complex windows
* Nested menus
* Multiple dashboards
* Passenger detail screens

Keep the MVP focused and lightweight.

---

# AI Guidelines

When creating new screens:

* Reuse existing layout patterns.
* Keep the map visible whenever possible.
* Avoid modal dialogs.
* Favor side panels over popups.
* Maintain consistency with existing UI components.
* Prioritize usability over visual complexity.

```
```
