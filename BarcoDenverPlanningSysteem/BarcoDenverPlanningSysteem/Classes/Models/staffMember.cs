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

        private Function defaultFunction;

        private Function functionOfDay;
        private PlannableFunction plannableFunctionOfDay;
        private DateTime pauseTime;
        private DateTime startTime;
        private DateTime endTime;

        public StaffMember(int id, PlannableFunction plannableFunction, DateTime pauseTime, DateTime startTime, DateTime endTime)
        {
            this.plannableFunctionOfDay = plannableFunction;
            this.pauseTime = pauseTime;
            this.startTime = startTime;
            this.endTime = endTime;
            this.id = id;
        }

        public StaffMember(int id, Function functionOfDay, DateTime pauseTime, DateTime startTime, DateTime endTime)
        {
            this.functionOfDay = functionOfDay;
            this.pauseTime = pauseTime;
            this.startTime = startTime;
            this.endTime = endTime;
            this.id = id;
        }

        public StaffMember(int id, string name, double earnings, Function defaultFunction)
        {
            this.id = id;
            this.name = name;
            this.earnings = earnings;
            this.defaultFunction = defaultFunction;
        }

        public StaffMember(int id, string name, double earnings, Function defaultFunction, Function functionOfDay, DateTime pauseTime, DateTime startTime, DateTime endTime) : this(id, name, earnings, defaultFunction)
        {
            this.functionOfDay = functionOfDay;
            this.pauseTime = pauseTime;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        /// <summary>
        /// berekent het aantal gewerkte uren voor dit persoon op een dag
        /// </summary>
        /// <param name="startTime">begin tijd</param>
        /// <param name="endTime">eind tijd</param>
        /// <returns>geeft een TimeSpan object terug met daarin het aantal gewerkte uren en het aantal gewerkte minuten</returns>
        public TimeSpan AmountOfWorkedHours(bool withPauseTime)
        {
            if (!withPauseTime)
            {
                return countUpStartAndEndTime(startTime.TimeOfDay, endTime.TimeOfDay);
            }
            else
            {
                TimeSpan dateTime = countUpStartAndEndTime(startTime.TimeOfDay, endTime.TimeOfDay);

                int tempHours = (dateTime - pauseTime.TimeOfDay).Hours;
                int tempMinutes = (dateTime - pauseTime.TimeOfDay).Minutes;

                return new TimeSpan(tempHours, tempMinutes, 0);
            }
           
        }

        /// <summary>
        /// haalt twee tijden van elkaar af
        /// </summary>
        /// <param name="startTime">begintijd</param>
        /// <param name="endTime">eindtijd</param>
        /// <returns>het aantal gewerkte uren</returns>
        private TimeSpan countUpStartAndEndTime(TimeSpan startTime, TimeSpan endTime)
        {
            int tempHours = (endTime - startTime).Hours;
            int tempMinutes = (endTime - startTime).Minutes;

            return new TimeSpan(tempHours, tempMinutes, 0);
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public double Earnings { get => earnings; set => earnings = value; }
        public Function FunctionOfDay { get => functionOfDay; set => functionOfDay = value; }
        public Function DefaultFunction { get => defaultFunction; set => defaultFunction = value; }
        public Function FunctionOfDay1 { get => functionOfDay; set => functionOfDay = value; }
        public DateTime PauseTime { get => pauseTime; set => pauseTime = value; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }
        public PlannableFunction PlannableFunctionOfDay { get => plannableFunctionOfDay; set => plannableFunctionOfDay = value; }
    }
}
