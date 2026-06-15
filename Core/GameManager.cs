using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ProjectZ.Simulation;

namespace ProjectZ.Core
{
    public class GameManager
    {
        public World World { get; private set; }
        private readonly List<ISimulationSystem> _systems;
        private readonly VisibilitySystem _visibilitySystem;

        public GameManager(World world)
        {
            World = world;
            _visibilitySystem = new VisibilitySystem();
            _systems = new List<ISimulationSystem>
        {
            new ProductionSystem(),
            new FoodConsumptionSystem(),
            new PopulationSystem(),
            _visibilitySystem
        };
            _visibilitySystem.Update(World);
        }

        public void AdvanceTurn()
        {
            Console.WriteLine($"Advancing turn from {World.Date}.");
            World.Log.Clear();  

            foreach (var system in _systems)
            {
                Console.WriteLine($"Running simulation system: {system.GetType().Name}");
                system.Update(World);
            }

            World.Date.Advance();
            Console.WriteLine($"Turn advanced to {World.Date}.");
        }

        public void ExecuteCommand(GameCommand command)
        {
            if (command.CanExecute(World))
            {
                Console.WriteLine($"Executing command: {command.Label}");
                command.Execute(World);
            }
            else
            {
                Console.WriteLine($"Command unavailable: {command.Label}");
            }
        }
    }

}
