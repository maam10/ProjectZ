using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectY.Domain;

namespace ProjectY.Core
{
    public class World
    {
        public Map Map { get; }
        public VisibilityMap Visibility { get; }
        public List<Colony> Colonies { get; } = new();
        public GameDate Date { get; }
        public TurnLog Log { get; } = new();
        public Tile SelectedTile { get; set; }
        public bool IsActionMenuOpen { get; set; }
        public Tile HoveredTile { get; set; }
        public List<Unit> Units { get; } = new();
        
        public World(GameDate date)
        {
            Date = date;
            Map = new Map(20, 15);
            Visibility = new VisibilityMap(Map.Width, Map.Height);
        }

        public World(GameDate date, Map map)
        {
            Date = date;
            Map = map;
            Visibility = new VisibilityMap(Map.Width, Map.Height);
        }
        public Colony GetColonyAt(int x, int y)
        {
            return Colonies.FirstOrDefault(c =>
                c.MapPosition.X == x &&
                c.MapPosition.Y == y);
        }
    }
}
