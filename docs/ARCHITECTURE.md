# Architecture

## Purpose

Project Z is a long-term airport management and simulation game developed in C# using MonoGame.

The project is built on top of the architecture originally created for Project Y. The goal is to reuse as much infrastructure as possible while replacing game-specific simulation concepts.

The architecture follows a layered design intended to support incremental development and AI-assisted coding.

---

# Technology Stack

* Language: C#
* Framework: MonoGame
* IDE: Visual Studio Code
* Version Control: Git
* AI Assistance: Codex / ChatGPT

---

# Architectural Principles

1. Domain contains business rules only.
2. Simulation updates the game world.
3. Presentation renders information and handles user interaction.
4. Core contains reusable infrastructure.
5. Game logic must not depend on rendering.
6. MonoGame types should remain inside Presentation whenever possible.
7. Prefer extending existing systems over rewriting them.
8. Favor simple simulation models before introducing detailed agent simulations.

---

# Project Structure

```text
ProjectZ
│
├── Core
├── Domain
├── Simulation
├── Presentation
├── Data
├── Content
└── Docs
```

---

# Layer Responsibilities

## Core

Contains reusable infrastructure.

Examples:

* Hex grid
* Map
* Tile
* Commands
* Input handling
* Utilities
* Shared interfaces

Core should remain generic and reusable.

---

## Domain

Contains business entities and rules.

Examples:

* Airport
* Runway
* Gate
* Terminal
* Flight
* Airline

Domain should not contain rendering logic.

---

## Simulation

Contains systems that evolve the game state.

Examples:

* TimeSystem
* FlightSystem
* EconomySystem
* AirlineSystem
* InfrastructureSystem

Simulation updates Domain objects.

---

## Presentation

Responsible for rendering and user interaction.

Examples:

* MapRenderer
* AirportRenderer
* UI Panels
* Menus
* Tooltips
* Construction UI

Presentation must not contain business rules.

---

# High-Level Runtime Flow

```text
Input
  ↓
Commands
  ↓
Domain Updates
  ↓
Simulation Systems
  ↓
World State Changes
  ↓
Rendering
```

---

# World Model

The world is represented by a hexagonal tile map.

Each tile may contain:

* Terrain
* Airport infrastructure
* Visibility information

Example:

```text
World
 ├─ Tiles
 ├─ Airports
 ├─ Flights
 ├─ Economy
 └─ Time
```

---

# Airport Model

```text
Airport
 ├─ Runways
 ├─ Taxiways
 ├─ Gates
 ├─ Terminals
 ├─ Finances
 └─ Reputation
```

Airport infrastructure occupies one or more hex tiles.

---

# Flight Lifecycle

Initial target flow:

```text
Scheduled
    ↓
Approaching
    ↓
Landing
    ↓
Taxiing
    ↓
AtGate
    ↓
Boarding
    ↓
Departing
    ↓
Completed
```

Flight state transitions are managed by Simulation systems.

---

# Reuse Strategy

The following components should be preserved whenever possible:

* HexGrid
* Tile
* World
* Camera
* InputHandler
* Command Pattern
* Selection System
* Map Rendering
* Visibility System
* UI Framework

The following concepts are expected to evolve:

```text
Colony           → Airport
Population       → Passenger Demand
Production       → Airport Revenue
Buildings        → Airport Infrastructure
Resources        → Airport Finances
```

The following concepts may eventually be removed:

* Food systems
* Colonist simulation
* Population aging
* Colony-specific mechanics

---

# Future Expansion

Potential future systems:

* Passenger simulation
* Cargo operations
* Airline contracts
* International flights
* Airport reputation
* Weather
* Air traffic control
* Maintenance systems

These features should be added incrementally and should not influence MVP architecture decisions.

---

# AI Development Guidelines

When modifying the codebase:

* Prefer small incremental changes.
* Avoid large-scale rewrites.
* Preserve the layered architecture.
* Reuse existing infrastructure whenever possible.
* Keep code testable.
* Update documentation when introducing architectural changes.
* Do not introduce unnecessary dependencies.
* Keep MVP scope under control.

```
```
