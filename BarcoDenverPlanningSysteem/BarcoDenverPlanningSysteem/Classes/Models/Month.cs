using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcoDenverPlanningSysteem.Classes.Models
{
    public class Month
    {
        private DateTime monthDate;
        private List<Day> daysInMonth;

        public Month(DateTime monthDate)
        {
            this.monthDate = monthDate;
            this.daysInMonth = new List<Day>();
        }

        public DateTime MonthDate { get => monthDate; set => monthDate = value; }
        public List<Day> DaysInMonth { get => daysInMonth; set => daysInMonth = value; }

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
                daysInMonth.Add(dateTime);
            }
        }
    }
}
