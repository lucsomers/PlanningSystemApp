using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using BarcoDenverPlanningSysteem.Classes.Models;
using BarcoDenverPlanningSysteem.Classes.DataRepo;
using BarcoDenverPlanningSysteem.Classes.Error;

namespace BarcoDenverPlanningSysteem
{
    public class DatabaseRepository
    {
        MySqlConnection connection = new MySqlConnection();
        DatabaseUsers dbUsers = new DatabaseUsers();
        DatabasePlanning dbPlanning = new DatabasePlanning();
        ErrorHandler error = new ErrorHandler();

        /// <summary>
        /// geeft de database zijn connectie een connectie string zodat er querrys uitgevoerd kunnen worden
        /// </summary>
        public void Connect()
        {
            connection.ConnectionString = "server=127.0.0.1;user id=root;password=123;database=marcelapp";
        }

        public void DeleteStaffMemberById(int id) => dbUsers.DeleteStaffMemberById(id, connection);

        public void AddStaffMember(StaffMember member, bool idFilled = false)
        {
            if (!idFilled)
            {
                dbUsers.AddStaffMember(member, connection);
            }
            else
            {
                dbUsers.ConnectStaffmemberToWorkplace(member, connection);
            }
        }

        public void FillViewWithAllStaffMembers(DataGridView dataview) => dbUsers.FillViewWithAllUsers(dataview, connection);

        public string GetFunctionFromStaffmember(int staffmemberid) => dbUsers.GetFunctionFromStaffmember(staffmemberid, connection);


        /// <summary>
        /// zoekt welke werkplek er bij de code hoort
        /// </summary>
        /// <param name="code">de inlogcode van een werkplek</param>
        /// <returns>geeft een werkplek terug die hoort bij de meegegeven code</returns>
        public Workplace CheckCodeForLogin(int code) => dbUsers.CheckCodeForLogin(code, connection);

        public bool CheckForYear(DateTime datetimeToPlan)
        {
            return true;
        }

        public bool CheckForMonth(DateTime datetimeToPlan)
        {
            return true;
        }

        public bool CheckForDay(DateTime datetimeToPlan)
        {
            return true;
        }

        public int getIdFromStaffMemberName(string name)
        {
            return dbUsers.getIdFromStaffMemberName(connection, name);
        }

        public string FillPlanningTableWithData(DataGridView tableToFill, Workplace currentUser, DateTime dateToFill, bool planning)
        {
            if (tableToFill != null)
            {
                return dbPlanning.FillPlanningTableWithData(tableToFill, connection, currentUser, dateToFill, planning);
            }
            else
            {
                MessageBox.Show(error.CantFillPlanningMessage());
            }

            return "";
        }

        public void AddStaffMemberToPlanning(DateTime datetimeToPlan, bool reality, StaffMember staffMemberToPlan, int workplace_id)
        {
            dbPlanning.AddStaffMemberToPlanning(datetimeToPlan, reality, staffMemberToPlan, workplace_id,connection);
        }

        /// <summary>
        /// haalt alle werkplekken op die er zijn
        /// </summary>
        /// <returns>geeft een lijst met namen van werkplekken terug</returns>
        public string[] getAllWorkplaces(int[] exceptions = null)
        {
            return dbUsers.GetAllWorkplaces(connection,exceptions);
        }

        /// <summary>
        /// veranderd de inlogcode van een werkplek naar de meegegeven code
        /// </summary>
        /// <param name="newcode">de nieuwe code die de oude gaat vervangen</param>
        /// <param name="workplace">de werkplek waarvan de inlogcode moet worden veranderd</param>
        public void EditCode(string newcode, string workplace)
        {
            dbUsers.EditCode(workplace, int.Parse(newcode), connection);
        }

        /// <summary>
        /// haalt de huidige inlogcode van een werkplek op
        /// </summary>
        /// <param name="workplace">de werkplek waarvan de code moet worden opgehaalt</param>
        /// <returns>geeft de huidige inlogcode in string formaat terug</returns>
        public string GetCodeFromWorkplace(string workplace)
        {
            return dbUsers.GetCodeFromFunction(workplace,connection);
        }

        public string[] GetListOfStaffMembers(Workplace currentUser)
        {
            return dbUsers.GetListOfAllStaffmMembers(connection, currentUser);
        }

        public bool UpdateStaffMemberFunction(int newFunctionid, int staffmemberid)
        {
            return dbUsers.UpdateStaffMemberFunctionId(newFunctionid, staffmemberid, connection);
        }
    }
}