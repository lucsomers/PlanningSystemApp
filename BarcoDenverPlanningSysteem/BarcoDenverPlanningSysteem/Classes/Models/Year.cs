using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcoDenverPlanningSysteem.Classes.Models
{
    public class Year
    {
        private DateTime year;

        private List<Month> monthsInYear;

        public Year(DateTime year)
        {
            this.ThisYear = year;
            this.MonthsInYear = new List<Month>();
        }

        public DateTime ThisYear { get => year; private set => year = value; }
        public List<Month> MonthsInYear { get => monthsInYear; private set => monthsInYear = value; }
        public void AddDay(Day dateTime, StaffMember memberToAdd)
        {
            bool monthExists = false;
            Month mTemp = null;

            foreach (Month month in monthsInYear)
            {
                if(month.MonthDate.Month == dateTime.Date.Month)
                {
                    //When the month already exists we just add a day to the month
                    monthExists = true;
                    mTemp = month;
                    break;
                }
            }

            //When month does not exist add a new month and add the day to that month
            if (!monthExists)
            {
                mTemp = new Month(new DateTime(0, dateTime.Date.Month, 0));
                this.MonthsInYear.Add(mTemp);
            }

            mTemp.AddDayToMonth(dateTime, memberToAdd);
        }
    }
}
