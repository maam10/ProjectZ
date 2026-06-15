using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZ.Domain
{
    public class Building
    {
        public BuildingType Type { get; }

        public Building(BuildingType type)
        {
            Type = type;
        }
    }

}
