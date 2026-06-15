# Game Rules

## Turn Order

Each turn currently resolves in this order:

1. Food production
2. Food consumption
3. Population growth, aging, and old-age deaths
4. Visibility update

## Food

Food production is calculated from buildings placed on the map. Each building contributes the food production defined for its type in `BuildingCatalog`.

Each inhabitant consumes 1 food per turn. If a colony does not have enough food, its food stock drops to zero and the shortage is logged. Starvation effects are not implemented yet.

## Wood

Wood production is calculated from buildings placed on the map. A `Wood Camp` can be built on forest terrain and produces 5 wood per turn.

## Buildings

- `Farm`: can be built on grass terrain and produces food.
- `Wood Camp`: can be built on forest terrain and produces wood.

Building type rules are defined centrally in `BuildingCatalog`: display name, required terrain, construction cost, and production.

## Population

Each inhabitant is represented individually and stores age in months. Starting inhabitants are created with a random age between 5 and 30 years. Each turn may add new inhabitants, advances every inhabitant by one month, and removes inhabitants that die of old age when they reach 80 years.

The base growth rate is 5% per turn. Growth depends on colony conditions: if the colony has food available after food consumption, the growth rate is 5%; if the colony has no food available, the growth rate is 0%. When growth is above 0%, at least 1 new inhabitant is added.
