using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcoDenverPlanningSysteem.Classes.Models
{
    public class StaffMember
    {
        private int id;

        private string name;

        private double earnings;

        private Function functionOfDay;
        private DateTime pauseTime;
        private DateTime startTime;
        private DateTime endTime;

        /// <summary>
        /// berekent het aantal gewerkte uren voor dit persoon op een dag
        /// </summary>
        /// <param name="startTime">begin tijd</param>
        /// <param name="endTime">eind tijd</param>
        /// <returns>geeft een datetime object terug met daarin het aantal gewerkte uren en het aantal gewerkte minuten</returns>
        public DateTime amountOfWorkedHours(bool withPauseTime)
        {
            if (!withPauseTime)
            {
                return countUpStartAndEndTime(startTime, endTime);
            }
            else
            {
                DateTime dateTime = countUpStartAndEndTime(startTime, endTime);

                int tempHours = (dateTime - pauseTime).Hours;
                int tempMinutes = (dateTime - pauseTime).Minutes;

                return new DateTime(0, 0, 0, tempHours, tempMinutes, 0);
            }
           
        }

        /// <summary>
        /// haalt twee tijden van elkaar af
        /// </summary>
        /// <param name="startTime">begintijd</param>
        /// <param name="endTime">eindtijd</param>
        /// <returns>het aantal gewerkte uren</returns>
        private DateTime countUpStartAndEndTime(DateTime startTime, DateTime endTime)
        {
            int tempHours = (endTime - startTime).Hours;
            int tempMinutes = (endTime - startTime).Minutes;

            return new DateTime(0, 0, 0, tempHours, tempMinutes, 0);
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public double Earnings { get => earnings; set => earnings = value; }
        public Function FunctionOfDay { get => functionOfDay; set => functionOfDay = value; }
    }
}
