using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZ.Core
{
    public class GameDate
    {
        public int Year { get; private set; }
        public int Month { get; private set; }

        public GameDate(int startYear, int startMonth = 1)
        {
            Year = startYear;
            Month = startMonth;
        }

        public void Advance()
        {
            Month++;
            if (Month > 12)
            {
                Month = 1;
                Year++;
            }
        }

        public override string ToString()
            => $"{Month:00}/{Year}";
    }

}
