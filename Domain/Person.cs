namespace ProjectY.Domain
{
    public class Person
    {
        public int AgeMonths { get; private set; }

        public int AgeYears => AgeMonths / 12;

        public Person(int ageMonths)
        {
            AgeMonths = ageMonths;
        }

        public void AdvanceMonth()
        {
            AgeMonths++;
        }
    }
}
