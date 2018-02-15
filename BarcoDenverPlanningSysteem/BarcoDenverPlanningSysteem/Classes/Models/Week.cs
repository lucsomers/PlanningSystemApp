using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcoDenverPlanningSysteem.Classes.Models
{
    class Week
    {
        private int id;

        private List<Day> daysInWeek;

        private int weekNumber;

        public double ExpectedRevenue()
        {
            double tempDouble = 0;

            foreach (Day day in daysInWeek)
            {
                tempDouble += day.ExpectedRevenue;
            }

            return tempDouble;
        }

        public double TotalStaffCost(bool withPauseTime)
        {
            double tempDouble = 0;

            foreach (Day day in daysInWeek)
            {
                tempDouble += day.TotalStaffCost(withPauseTime);
            }

            return tempDouble;
        }
        
        public int PercentageTotalStaffCostToExpectedRevenue(bool withPauseTime)
        {
            return (int)Math.Round((TotalStaffCost(withPauseTime) / ExpectedRevenue()) * 100, 0);
        }
    }
}
