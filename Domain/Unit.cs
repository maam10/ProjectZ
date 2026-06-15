using Microsoft.Xna.Framework;

namespace ProjectZ.Domain
{
    public class Unit
    {
        public string Name { get; }
        public UnitType Type { get; }

        public Point Position { get; private set; }

        public int MaxMovementPoints { get; }
        public int CurrentMovementPoints { get; private set; }

        public int VisionRange { get; }

        public Unit(
            string name,
            UnitType type,
            Point position,
            int movementPoints,
            int visionRange)
        {
            Name = name;
            Type = type;
            Position = position;

            MaxMovementPoints = movementPoints;
            CurrentMovementPoints = movementPoints;

            VisionRange = visionRange;
        }

        public void MoveTo(Point target)
        {
            Position = target;
            CurrentMovementPoints--;
        }

        public void ResetTurn()
        {
            CurrentMovementPoints = MaxMovementPoints;
        }
    }
}