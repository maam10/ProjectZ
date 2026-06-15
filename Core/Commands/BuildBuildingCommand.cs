using ProjectY.Domain;
using System.Linq;

namespace ProjectY.Core.Commands
{
    public class BuildBuildingCommand : GameCommand
    {
        private readonly Tile _tile;
        private readonly BuildingDefinition _definition;

        public BuildBuildingCommand(Tile tile, BuildingDefinition definition)
        {
            _tile = tile;
            _definition = definition;
        }

        public override string Label => $"Construir {_definition.DisplayName}";

        public override ResourceCost Cost => _definition.Cost;

        public override bool CanExecute(World world)
        {
            var colony = world.Colonies.First(); // MVP

            bool terrainOk = _tile.Terrain == _definition.RequiredTerrain && !_tile.HasBuilding;
            bool hasResources =
                colony.Resources.Food >= Cost.Food &&
                colony.Resources.Wood >= Cost.Wood &&
                colony.Resources.Gold >= Cost.Gold;

            return terrainOk && hasResources;
        }

        public override void Execute(World world)
        {
            var colony = world.Colonies.First();

            colony.Resources.Food -= Cost.Food;
            colony.Resources.Wood -= Cost.Wood;
            colony.Resources.Gold -= Cost.Gold;

            var building = new Building(_definition.Type);
            _tile.PlaceBuilding(building);
            colony.Buildings.Add(building);

            world.Log.Add(
                $"{_definition.DisplayName} construido (-{Cost.Wood} madeira, -{Cost.Gold} ouro).",
                LogSeverity.Positive
            );
        }
    }
}
