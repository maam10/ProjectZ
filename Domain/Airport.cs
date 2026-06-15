using System;
using System.Collections.Generic;

namespace ProjectZ.Domain
{
    public class Airport
    {
        public Guid Id { get; }
        public string Name { get; }
        public List<AirportInfrastructure> Infrastructure { get; }

        public Airport(string name)
            : this(Guid.NewGuid(), name)
        {
        }

        public Airport(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Airport id cannot be empty.", nameof(id));
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Id = id;
            Name = name;
            Infrastructure = new List<AirportInfrastructure>();
        }
    }
}
