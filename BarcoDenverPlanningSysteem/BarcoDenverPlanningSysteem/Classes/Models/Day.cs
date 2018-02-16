using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcoDenverPlanningSysteem.Classes.Models
{
    public class Day
    {
        private double expectedRevenue;
        private double revenue;

        
        private DateTime date;
        

        private string staffComment;
        private string adminComment;

        private List<StaffMember> staffMembers;
        
        /// <summary>
        /// telt het aantal personen die werken onder de meegegeven functie op deze dag
        /// </summary>
        /// <param name="function">geeft aan welke functie getelt moet worden</param>
        /// <returns>geeft een int terug die representateerd hoeveel mensen er werken onder de meegegeven functie</returns>
        public int AmountOfFunction(Function function)
        {
            int counter = 0;

            foreach (StaffMember staffmember in staffMembers)
            {
                if (staffmember.FunctionOfDay == function)
                {
                    counter++;
                }
            }

            return counter;
        }

        /// <summary>
        /// telt het totale loon van alle werknemers van deze dag bij elkaar op
        /// </summary>
        /// <param name="withPauseTime">geeft aan of het totaal met of zonder pauze moet worden berekent</param>
        /// <returns>geeft het totale loon terug als double</returns>
        public double TotalStaffCost(bool withPauseTime)
        {
            double tempDouble = 0;
            
                foreach (StaffMember member in staffMembers)
                {
                    tempDouble = member.Earnings * member.AmountOfWorkedHours(withPauseTime).Hour;
                }

            return tempDouble;
        }

        /// <summary>
        /// bereken de hoeveelheid loon een enkele werknemer krijgt met of zonder pauze
        /// </summary>
        /// <param name="withPauseTime">geeft aan of pauze meeberekent moet worden of niet</param>
        /// <param name="memberID">persoon van wie het loon moet worden berekent</param>
        /// <returns>geeft het loon terug dat de meegegeven werknemer moet krijgen</returns>
        public double EarningsOfStaffMember(bool withPauseTime, int memberID)
        {
            double tempDouble = 0;

                foreach (StaffMember member in staffMembers)
                {
                    if (member.Id == memberID)
                    {
                        tempDouble = member.Earnings * member.AmountOfWorkedHours(withPauseTime).Hour;
                    }
                }

            return tempDouble;
        }
        public double ExpectedRevenue { get => expectedRevenue; set => expectedRevenue = value; }
        public double Revenue { get => revenue; set => revenue = value; }
        public DateTime Date { get => date; set => date = value; }
        public string StaffComment { get => staffComment; set => staffComment = value; }
        public string AdminComment { get => adminComment; set => adminComment = value; }
        public void AddStaffMember(StaffMember member){ staffMembers.Add(member); }
    }
}
