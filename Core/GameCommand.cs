using ProjectZ.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZ.Core
{
    public abstract class GameCommand
    {
        public abstract string Label { get; }
        public abstract ResourceCost Cost { get; }

        public abstract bool CanExecute(World world);
        public abstract void Execute(World world);
    }
}
