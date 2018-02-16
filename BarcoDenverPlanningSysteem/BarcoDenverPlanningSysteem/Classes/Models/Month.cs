using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcoDenverPlanningSysteem.Classes.Models
{
    public class Month
    {
        private int id;
        private DateTime monthDate;
        private List<Day> realityDaysInMonth;
        private List<Day> planningDaysInMonth;

        public Month(DateTime monthDate, int i = 0)
        {
            this.monthDate = monthDate;
            this.realityDaysInMonth = new List<Day>();
            this.planningDaysInMonth = new List<Day>();
            this.Id = i;
        }

        public DateTime MonthDate { get => monthDate; set => monthDate = value; }
        public List<Day> DaysInMonth { get => realityDaysInMonth; set => realityDaysInMonth = value; }
        public int Id { get => id; private set => id = value; }

        public void AddDayToMonth(Day dateTime, StaffMember memberToAdd)
        {
            bool memberDateExists = false;

            foreach (Day d in DaysInMonth)
            {
                if (d.Date.Day == dateTime.Date.Day)
                {
                    //when the day already exist just add the member to the existing date
                    d.AddStaffMember(memberToAdd);
                    memberDateExists = true;
                    break;
                }
            }

            //When day does not exist add a new day to this month
            if (!memberDateExists)
            {
                dateTime.AddStaffMember(memberToAdd);
                realityDaysInMonth.Add(dateTime);
            }
        }
    }
}
