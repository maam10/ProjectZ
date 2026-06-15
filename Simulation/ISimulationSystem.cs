using ProjectY.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectY.Simulation
{
    public interface ISimulationSystem
    {
        void Update(World world);
    }
}
