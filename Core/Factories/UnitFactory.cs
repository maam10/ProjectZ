using Microsoft.Xna.Framework;
using ProjectY.Domain;

namespace ProjectY.Core.Factories
{
    public static class UnitFactory
    {
        public static Unit CreateScout(Point position)
        {
            System.Console.WriteLine($"Creating Scout unit at position ({position.X}, {position.Y}).");
            return new Unit(
                name: "Scout",
                type: UnitType.Scout,
                position: position,
                movementPoints: 3,
                visionRange: 4
            );
        }

        public static Unit CreateScout(Tile tile)
            => CreateScout(new Point(tile.X, tile.Y));

        public static Unit CreateColonist(Point position)
        {
            System.Console.WriteLine($"Creating Colonist unit at position ({position.X}, {position.Y}).");
            return new Unit(
                name: "Colonist",
                type: UnitType.Colonist,
                position: position,
                movementPoints: 2,
                visionRange: 2
            );
        }

        public static Unit CreateColonist(Tile tile)
            => CreateColonist(new Point(tile.X, tile.Y));
    }
}