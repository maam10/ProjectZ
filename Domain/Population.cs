using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectZ.Domain
{
    public class Population
    {
        private const int InitialMinimumAgeYears = 5;
        private const int InitialMaximumAgeYears = 30;
        private readonly List<Person> _people = new();

        public IReadOnlyList<Person> People => _people;
        public int Total => _people.Count;
        public float GrowthRate { get; private set; }
        public double AverageAgeYears => Total == 0
            ? 0
            : _people.Average(person => person.AgeMonths) / 12.0;

        public Population(int initial)
        {
            for (int i = 0; i < initial; i++)
            {
                _people.Add(CreateInitialPerson());
            }
        }

        public int AdvanceMonthAndRemoveDeaths(int maximumAgeYears)
        {
            foreach (var person in _people)
            {
                person.AdvanceMonth();
            }

            int before = _people.Count;
            _people.RemoveAll(person => person.AgeYears >= maximumAgeYears);

            return before - _people.Count;
        }

        public int Grow(float growthRate)
        {
            GrowthRate = growthRate;

            if (growthRate <= 0 || Total == 0)
            {
                return 0;
            }

            int births = Math.Max(1, (int)(Total * growthRate));

            for (int i = 0; i < births; i++)
            {
                _people.Add(new Person(0));
            }

            return births;
        }

        private static Person CreateInitialPerson()
        {
            int ageYears = Random.Shared.Next(
                InitialMinimumAgeYears,
                InitialMaximumAgeYears + 1
            );
            int extraMonths = Random.Shared.Next(0, 12);

            return new Person((ageYears * 12) + extraMonths);
        }
    }
}
