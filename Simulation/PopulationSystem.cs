using ProjectZ.Core;

namespace ProjectZ.Simulation
{
    public class PopulationSystem : ISimulationSystem
    {
        private const float BaseGrowthRate = 0.05f;
        private const int MaximumAgeYears = 80;

        public void Update(World world)
        {
            foreach (var colony in world.Colonies)
            {
                float growthRate = colony.Resources.Food > 0
                    ? BaseGrowthRate
                    : 0f;

                int births = colony.Population.Grow(growthRate);
                int deaths = colony.Population.AdvanceMonthAndRemoveDeaths(MaximumAgeYears);

                if (births > 0)
                {
                    world.Log.Add(
                        $"{colony.Name} population grew by {births} habitants. Growth rate: {growthRate:P0}.",
                        LogSeverity.Positive
                    );
                }
                else
                {
                    world.Log.Add(
                        $"{colony.Name} population did not grow. Growth rate: {growthRate:P0}.",
                        growthRate > 0 ? LogSeverity.Info : LogSeverity.Warning
                    );
                }

                if (deaths > 0)
                {
                    world.Log.Add(
                        $"{colony.Name} lost {deaths} habitants to old age.",
                        LogSeverity.Negative
                    );
                }

                world.Log.Add(
                    $"{colony.Name} population aged. Average age: {colony.Population.AverageAgeYears:0.0} years.",
                    LogSeverity.Info
                );
            }
        }
    }
}
