# Roadmap

## Vision

Build a long-term airport management simulation where the player transforms a small regional airfield into a major international airport.

The roadmap prioritizes playable milestones over technical completeness.

---

# Current Status

Inherited from Project Y:

* Hexagonal map
* Tile selection
* Camera controls
* Construction framework
* Command pattern
* Layered architecture
* Basic UI framework

---

# Milestone 1 - Airport Foundations

Goal:

The player can place airport infrastructure on the map.

Success Criteria:

* Build runways
* Build terminals
* Build gates
* Infrastructure occupies hex tiles
* Infrastructure information panel
* Construction costs

Features:

* Airport entity
* Runway entity
* Gate entity
* Terminal entity
* Construction commands

Out of Scope:

* Flights
* Economy simulation
* Passengers

---

# Milestone 2 - Time and Flights

Goal:

The airport becomes operational.

Success Criteria:

* Game time advances
* Flights are generated
* Flights arrive
* Flights use gates
* Flights depart

Features:

* TimeSystem
* Flight entity
* FlightSystem
* Flight scheduling

Flight States:

* Scheduled
* Approaching
* Landing
* Taxiing
* AtGate
* Boarding
* Departing
* Completed

Out of Scope:

* Passenger simulation
* Airline contracts

---

# Milestone 3 - Airport Economy

Goal:

The player can succeed or fail financially.

Success Criteria:

* Flights generate revenue
* Infrastructure generates costs
* Player balance is tracked
* Monthly reports

Features:

* EconomySystem
* Financial reports
* Construction costs
* Operating expenses

Metrics:

* Cash
* Revenue
* Expenses
* Profit

---

# Milestone 4 - Airport Growth

Goal:

The player can expand capacity.

Success Criteria:

* Larger airports handle more traffic
* Additional runways
* Additional gates
* Terminal expansion

Features:

* Airport reputation
* Capacity calculations
* Demand growth

Metrics:

* Passengers per year
* Flights per day
* Airport rating

---

# Milestone 5 - Airlines

Goal:

Airlines become strategic partners.

Success Criteria:

* Multiple airlines
* Different flight demands
* Airline relationships

Features:

* Airline entity
* Airline contracts
* Airline reputation

Examples:

* Regional airline
* Low-cost airline
* International airline

---

# Milestone 6 - Passenger Abstraction

Goal:

Introduce passenger demand without individual agents.

Success Criteria:

* Passenger demand exists
* Airports have capacity limits
* Passenger satisfaction affects growth

Features:

* PassengerDemand model
* Terminal capacity
* Service quality

Important:

Do not simulate individual passengers.

Use aggregated simulation.

---

# Milestone 7 - Cargo Operations

Goal:

Introduce cargo as a second business model.

Success Criteria:

* Cargo flights
* Cargo terminals
* Cargo revenue

Features:

* Cargo demand
* Cargo contracts
* Cargo facilities

---

# Milestone 8 - International Airport

Goal:

Transform the airport into a global hub.

Success Criteria:

* International flights
* Customs facilities
* Long-haul operations

Features:

* International terminals
* Border control
* Hub status

---

# Future Ideas

Possible future systems:

* Weather
* Maintenance
* Air traffic control
* Emergencies
* Passenger agents
* Ground services
* Historical progression
* Multiplayer

These systems should only be considered after Milestone 8.

---

# Development Rules

1. Keep the game playable at all times.
2. Prefer incremental improvements.
3. Avoid large rewrites.
4. Reuse Project Y infrastructure whenever possible.
5. Finish a milestone before starting the next.
6. Documentation should evolve together with the code.
