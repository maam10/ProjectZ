namespace ProjectY.Domain
{
    public class BuildingDefinition
    {
        public BuildingType Type { get; init; }
        public string DisplayName { get; init; }
        public TerrainType RequiredTerrain { get; init; }
        public ResourceCost Cost { get; init; }
        public int FoodProduction { get; init; }
        public int WoodProduction { get; init; }
    }
}
