using ProjectZ.Core;

namespace ProjectZ.Simulation
{
    public class VisibilitySystem : ISimulationSystem
    {
        private const int ColonyVisionRadius = 3;

        public void Update(World world)
        {
            world.Visibility.ResetVisibleToExplored();

            foreach (var colony in world.Colonies)
            {
                world.Visibility.RevealAround(
                    colony.MapPosition.X,
                    colony.MapPosition.Y,
                    ColonyVisionRadius
                );
            }
        }
    }
}
