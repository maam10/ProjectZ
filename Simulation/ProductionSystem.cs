using ProjectY.Core;
using ProjectY.Core.Buildings;

namespace ProjectY.Simulation
{
    public class ProductionSystem : ISimulationSystem
    {
        public void Update(World world)
        {
            foreach (var colony in world.Colonies)
            {
                var production = CalculateProduction(world);

                colony.Resources.Food += production.Food;
                if (production.Food > 0)
                {
                    world.Log.Add(
                        $"{colony.Name} produced {production.Food} food.",
                        LogSeverity.Positive
                    );
                }

                colony.Resources.Wood += production.Wood;
                if (production.Wood > 0)
                {
                    world.Log.Add(
                        $"{colony.Name} produced {production.Wood} wood.",
                        LogSeverity.Positive
                    );
                }
            }
        }

        private static ResourceProduction CalculateProduction(World world)
        {
            int foodProduced = 0;
            int woodProduced = 0;

            for (int x = 0; x < world.Map.Width; x++)
                for (int y = 0; y < world.Map.Height; y++)
                {
                    var tile = world.Map.Tiles[x, y];

                    if (!tile.HasBuilding)
                    {
                        continue;
                    }

                    var definition = BuildingCatalog.Get(tile.Building.Type);

                    foodProduced += definition.FoodProduction;
                    woodProduced += definition.WoodProduction;
                }

            return new ResourceProduction(foodProduced, woodProduced);
        }

        private readonly record struct ResourceProduction(int Food, int Wood);
    }
}
