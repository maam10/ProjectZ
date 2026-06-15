using System;

namespace ProjectZ.Domain
{
    public class AirportInfrastructure
    {
        public Guid Id { get; }

        public AirportInfrastructure()
            : this(Guid.NewGuid())
        {
        }

        public AirportInfrastructure(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Infrastructure id cannot be empty.", nameof(id));
            }

            Id = id;
        }
    }
}
