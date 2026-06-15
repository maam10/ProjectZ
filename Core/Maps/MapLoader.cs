using System;
using System.IO;
using System.Text.Json;
using ProjectZ.Domain;

namespace ProjectZ.Core.Maps
{
    public static class MapLoader
    {
        public static MapDefinition LoadDefinition(string path)
        {
            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var definition = JsonSerializer.Deserialize<MapDefinition>(json, options);

            if (definition == null)
            {
                throw new InvalidOperationException($"Map file '{path}' is empty or invalid.");
            }

            Validate(definition);
            return definition;
        }

        public static Map CreateMap(MapDefinition definition)
        {
            var terrain = new TerrainType[definition.Width, definition.Height];

            for (int y = 0; y < definition.Height; y++)
                for (int x = 0; x < definition.Width; x++)
                {
                    terrain[x, y] = ParseTerrain(definition.Tiles[y][x]);
                }

            return new Map(terrain);
        }

        private static void Validate(MapDefinition definition)
        {
            if (definition.Width <= 0 || definition.Height <= 0)
            {
                throw new InvalidOperationException("Map width and height must be greater than zero.");
            }

            if (definition.Tiles.Count != definition.Height)
            {
                throw new InvalidOperationException(
                    $"Map '{definition.Name}' has {definition.Tiles.Count} tile rows but height is {definition.Height}.");
            }

            for (int y = 0; y < definition.Tiles.Count; y++)
            {
                if (definition.Tiles[y].Length != definition.Width)
                {
                    throw new InvalidOperationException(
                        $"Map '{definition.Name}' row {y} has {definition.Tiles[y].Length} columns but width is {definition.Width}.");
                }
            }
        }

        private static TerrainType ParseTerrain(char value)
        {
            return char.ToUpperInvariant(value) switch
            {
                'G' => TerrainType.Grass,
                'F' => TerrainType.Forest,
                'M' => TerrainType.Mountain,
                'W' => TerrainType.Water,
                _ => throw new InvalidOperationException($"Unknown terrain code '{value}'.")
            };
        }
    }
}
