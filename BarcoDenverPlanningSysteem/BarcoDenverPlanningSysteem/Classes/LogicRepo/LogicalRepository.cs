using BarcoDenverPlanningSysteem.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BarcoDenverPlanningSysteem
{
    public class LogicalRepository
    {
        private DatabaseRepository database = new DatabaseRepository();
        private DatabaseUsers dbUsers = new DatabaseUsers();

        private Workplace currentUser = Workplace.NoFunctionDetected;
        
        /// <summary>
        /// constuctor
        /// </summary>
        public LogicalRepository()
        {
            database.Connect();
        }

        /// <summary>
        /// Delete a staffmember from the database
        /// </summary>
        /// <param name="id">the id of the staffmember that has to be deleted</param>
        public void DeleteStaffMemberByid(int id)
        {
            database.DeleteStaffMemberById(id);
        }

        /// <summary>
        /// add a staffmember to the database
        /// </summary>
        /// <param name="name">name of the staffmember</param>
        /// <param name="earnings">earnings of the staffmember</param>
        public void AddStaffMember(string name, double earnings)
        {
            StaffMember tempMember = new StaffMember();
            tempMember.Name = name;
            tempMember.Earnings = earnings;

            database.AddStaffMember(tempMember);
        }

        /// <summary>
        /// updates the given view to contain all the staffmembers currently in the database
        /// </summary>
        /// <param name="view">the DataGridView that has to be updated</param>
        public void FillViewWithAllStaffMembers(DataGridView view)
        {
            database.FillViewWithAllStaffMembers(view);
        }

        /// <summary>
        /// zoekt welke werkplek er bij de code hoort en vult de functie van de ingelogde gebruiker in.
        /// </summary>
        /// <param name="code">de inlogcode van een werkplek</param>
        /// <returns>geeft een werkplek terug die hoort bij de meegegeven code</returns>
        public Workplace CheckLoginCode(string code)
        {
            Workplace tempWorkplace = database.CheckCodeForLogin(int.Parse(code));

            if (tempWorkplace != Workplace.NoFunctionDetected)
            {
                currentUser = tempWorkplace;
            }

            return tempWorkplace;
        }

        /// <summary>
        /// geeft alle functies die de huidige user aankan
        /// </summary>
        /// <returns>alle functies die de user tot beschikking heeft</returns>
        public string[] GetFunctionsAvailableToUser()
        {
            List<string> availableFunctions = new List<string>();

            switch (currentUser)
            {
                case Workplace.Directie:
                    availableFunctions.Add(Function.Afwas.ToFriendlyString());
                    availableFunctions.Add(Function.Bediening.ToFriendlyString());
                    availableFunctions.Add(Function.Keuken.ToFriendlyString());
                    break;
                case Workplace.Denver:
                    availableFunctions.Add(Function.Bediening.ToFriendlyString());
                    break;
                case Workplace.Barco:
                    availableFunctions.Add(Function.Bediening.ToFriendlyString());
                    break;
                case Workplace.Keuken:
                    availableFunctions.Add(Function.Keuken.ToFriendlyString());
                    availableFunctions.Add(Function.Afwas.ToFriendlyString());
                    break;
                case Workplace.Fiesta:
                    availableFunctions.Add(Function.Afwas.ToFriendlyString());
                    availableFunctions.Add(Function.Bediening.ToFriendlyString());
                    availableFunctions.Add(Function.Keuken.ToFriendlyString());
                    break;
                case Workplace.NoFunctionDetected:
                    break;
            }

            //elke lijst krijgt deze objecten
            availableFunctions.Add(Function.StandBy.ToFriendlyString());

            return availableFunctions.ToArray();
        }

        /// <summary>
        /// get a list of all staffmembers in the database
        /// </summary>
        /// <returns>string array of all staffmembers</returns>
        public string[] GetListOfStaffMembers()
        {
            return database.GetListOfStaffMembers();
        }

        /// <summary>
        /// haalt alle werkplekken op die er zijn
        /// </summary>
        /// <returns>geeft een lijst met namen van werkplekken terug</returns>
        public string[] GetAllWorkplaces()
        {
            return database.getAllWorkplaces();
        }

        /// <summary>
        /// veranderd de inlogcode van een werkplek naar de meegegeven code
        /// </summary>
        /// <param name="newcode">de nieuwe code die de oude gaat vervangen</param>
        /// <param name="workplace">de werkplek waarvan de inlogcode moet worden veranderd</param>
        public void EditCode(string newCode, string werkplek)
        {
            database.EditCode(newCode, werkplek);
        }

        /// <summary>
        /// haalt de huidige inlogcode van een werkplek op
        /// </summary>
        /// <param name="workplace">de werkplek waarvan de code moet worden opgehaalt</param>
        /// <returns>geeft de huidige inlogcode in string formaat terug</returns>
        public string GetCodeFromFunction(string workplace)
        {
            return database.GetCodeFromWorkplace(workplace);
        }

        /// <summary>
        /// get the user that is logged in within this session
        /// </summary>
        /// <returns>the user of this session</returns>
        public Workplace GetCurrentUser()
        {
            return currentUser;
        }
    }
}