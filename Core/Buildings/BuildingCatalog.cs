using System;
using System.Collections.Generic;
using System.Linq;
using ProjectY.Domain;

namespace ProjectY.Core.Buildings
{
    public static class BuildingCatalog
    {
        private static readonly IReadOnlyList<BuildingDefinition> _all = new List<BuildingDefinition>
        {
            new()
            {
                Type = BuildingType.Farm,
                DisplayName = "Farm",
                RequiredTerrain = TerrainType.Grass,
                Cost = new ResourceCost { Wood = 10, Gold = 5 },
                FoodProduction = 5
            },
            new()
            {
                Type = BuildingType.WoodCamp,
                DisplayName = "Wood Camp",
                RequiredTerrain = TerrainType.Forest,
                Cost = new ResourceCost { Wood = 5, Gold = 3 },
                WoodProduction = 5
            }
        };

        public static IReadOnlyList<BuildingDefinition> All => _all;

        public static BuildingDefinition Get(BuildingType type)
        {
            return _all.First(definition => definition.Type == type);
        }

        public static BuildingType ParseType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException("Building type cannot be empty.");
            }

            var normalized = value.Replace(" ", string.Empty);

            if (Enum.TryParse(normalized, ignoreCase: true, out BuildingType type))
            {
                return type;
            }

            throw new InvalidOperationException($"Unknown building type '{value}'.");
        }
    }
}
