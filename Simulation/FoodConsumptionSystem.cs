using System;
using ProjectY.Core;

namespace ProjectY.Simulation
{
    public class FoodConsumptionSystem : ISimulationSystem
    {
        private const int FoodConsumedPerHabitant = 1;

        public void Update(World world)
        {
            foreach (var colony in world.Colonies)
            {
                int foodRequired = colony.Population.Total * FoodConsumedPerHabitant;
                int foodConsumed = Math.Min(colony.Resources.Food, foodRequired);

                colony.Resources.Food -= foodConsumed;

                if (foodConsumed == foodRequired)
                {
                    world.Log.Add(
                        $"{colony.Name} consumed {foodConsumed} food.",
                        LogSeverity.Info
                    );
                }
                else
                {
                    int shortage = foodRequired - foodConsumed;

                    world.Log.Add(
                        $"{colony.Name} consumed {foodConsumed} food and is short {shortage} food.",
                        LogSeverity.Warning
                    );
                }
            }
        }
    }
}
