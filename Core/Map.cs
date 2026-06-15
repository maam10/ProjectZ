using ProjectZ.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZ.Core
{
    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        public Tile[,] Tiles { get; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new Tile[width, height];

            Generate();
        }

        public Map(TerrainType[,] terrain)
        {
            Width = terrain.GetLength(0);
            Height = terrain.GetLength(1);
            Tiles = new Tile[Width, Height];

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    Tiles[x, y] = new Tile(x, y, terrain[x, y]);
                }
        }

        private void Generate()
        {
            var random = new Random();

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    var terrain = (TerrainType)random.Next(0, 4);
                    Tiles[x, y] = new Tile(x, y, terrain);
                }
        }
    }
}
