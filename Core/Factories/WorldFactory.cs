using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using ProjectY.Core.Buildings;
using ProjectY.Core.Factories;
using ProjectY.Core.Maps;
using ProjectY.Domain;

namespace ProjectY.Core
{
    public static class WorldFactory
    {
        private const int StartYear = 1500;

        public static World CreateStarterWorld()
        {
            var mapPath = Path.Combine(AppContext.BaseDirectory, "Data", "Maps", "starter_island.json");

            try
            {
                
                var definition = MapLoader.LoadDefinition(mapPath);
                var world = new World(new GameDate(StartYear), MapLoader.CreateMap(definition));
                Console.WriteLine($"Loading map '{definition.Name}'.");
                foreach (var colonyDefinition in definition.Colonies)
                {
                    world.Colonies.Add(new Colony(
                        colonyDefinition.Name,
                        new Point(colonyDefinition.X, colonyDefinition.Y)
                    ));
                }

                foreach (var buildingDefinition in definition.Buildings)
                {
                    var buildingType = BuildingCatalog.ParseType(buildingDefinition.Type);
                    var building = new Building(buildingType);

                    world.Map.Tiles[buildingDefinition.X, buildingDefinition.Y].PlaceBuilding(building);

                    var colony = FindColonyForBuilding(world, buildingDefinition);
                    colony?.Buildings.Add(building);
                }

                Console.WriteLine($"Loaded map '{definition.Name}' from {mapPath}.");

                CreateStarterUnits(world);
                return world;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not load starter map. Using random map. Reason: {ex.Message}");
                return CreateFallbackWorld();
            }
        }

        private static World CreateFallbackWorld()
        {
            var world = new World(new GameDate(StartYear));
            var colony = new Colony("New Hope", new Point(10, 7));

            colony.Buildings.Add(new Building(BuildingType.Farm));
            world.Colonies.Add(colony);

            return world;
        }

        private static Colony FindColonyForBuilding(World world, MapBuildingDefinition buildingDefinition)
        {
            if (!string.IsNullOrWhiteSpace(buildingDefinition.ColonyName))
            {
                return world.Colonies.FirstOrDefault(c => c.Name == buildingDefinition.ColonyName);
            }

            return world.GetColonyAt(buildingDefinition.X, buildingDefinition.Y);
        }
        private static void CreateStarterUnits(World world)
        {
            world.Units.Add(
                UnitFactory.CreateScout(
                    new Point(11, 7)
                )
            );
        }
    }
}
