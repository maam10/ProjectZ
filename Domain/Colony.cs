using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectY.Domain
{
    public class Colony
    {
        public string Name { get; }
        public Population Population { get; }
        public ResourceStock Resources { get; }
        public List<Building> Buildings { get; }
        public Point MapPosition { get; }   // posição no grid (tile)

        public Colony(string name, Point mapPosition)
        {
            Name = name;
            MapPosition = mapPosition;

            Population = new Population(10);
            Resources = new ResourceStock();
            Buildings = new List<Building>();
        }
    }

}
