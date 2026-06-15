using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZ.Domain
{
    
    public enum TerrainType
    {
        Grass,
        Forest,
        Mountain,
        Water
    }

    public class Tile
    {
        public int X { get; }
        public int Y { get; }
        public TerrainType Terrain { get; }

        public Building Building { get; private set; }

        public bool HasBuilding => Building != null;

        public Tile(int x, int y, TerrainType terrain)
        {
            X = x;
            Y = y;
            Terrain = terrain;
        }

        public void PlaceBuilding(Building building)
        {
            Building = building;
        }
    }


}
