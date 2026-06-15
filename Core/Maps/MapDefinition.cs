using System.Collections.Generic;

namespace ProjectY.Core.Maps
{
    public class MapDefinition
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<string> Tiles { get; set; } = new();
        public List<ColonyDefinition> Colonies { get; set; } = new();
        public List<MapBuildingDefinition> Buildings { get; set; } = new();
    }

    public class ColonyDefinition
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MapBuildingDefinition
    {
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string ColonyName { get; set; }
    }
}
