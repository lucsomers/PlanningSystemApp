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
        private List<Year> mainYearList = new List<Year>();

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
        /// clears the box and fills the box again with the staffmembers needed checked boxes
        /// </summary>
        /// <param name="staffmembername">the name of the staffmember</param>
        /// <param name="box">the box that needs checked boxes</param>
        public void GetCheckedItemsFromStaffmemberName(int staffmemberid, CheckedListBox box)
        {
            for (int i = 0; i < box.Items.Count; i++)
            {
                box.SetItemChecked(i, false);
            }

            string staffmemberCurrentFunction = database.GetFunctionFromStaffmember(staffmemberid);
            int index = 0;

            foreach (Object item in box.Items)
            {
                string checkboxText = box.GetItemText(item);
                
                if (staffmemberCurrentFunction.ToLower() == checkboxText.ToLower())
                {
                    box.SetItemChecked(index, true);
                    break;
                }
                index++;
            }
        }

        /// <summary>
        /// add a staffmember to the database
        /// </summary>
        /// <param name="name">name of the staffmember</param>
        /// <param name="earnings">earnings of the staffmember</param>
        public void AddStaffMember(string name, double earnings, string defaultWorkplace)
        {
            StaffMember tempMember = new StaffMember(0, name, earnings, FunctionExtension.stringToEnum(defaultWorkplace));
            
            database.AddStaffMember(tempMember);
        }

        public void FillPlanningtableWithData(DataGridView tableToFill, DateTime dateToFill, int planning, TextBox textBox)
        {
            //sets commentbox
            textBox.Text = database.FillPlanningTableWithData(tableToFill, currentUser, dateToFill, planning);
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
        public string[] GetPlannableFunctionsAvailableToUser()
        {
            List<string> availableFunctionsString = new List<string>();
            List<Function> availableFunctions = new List<Function>();

            switch (currentUser)
            {
                case Workplace.Directie:
                    availableFunctionsString.Add(Function.BarcoKeuken.ToFunctionString());
                    availableFunctionsString.Add(Function.Barco_Bar.ToFunctionString());
                    availableFunctionsString.Add(Function.Barco_Denver_Afwas.ToFunctionString());
                    availableFunctionsString.Add(Function.Denver_Bar.ToFunctionString());
                    availableFunctionsString.Add(Function.Denver_Keuken.ToFunctionString());
                    availableFunctionsString.Add(Function.Fiesta_Afwas.ToFunctionString());
                    availableFunctionsString.Add(Function.Fiesta_Bar.ToFunctionString());
                    availableFunctionsString.Add(Function.Fiesta_Keuken.ToFunctionString());
                    availableFunctionsString.Add(PlannableFunction.StandBy.ToFriendlyString());
                    break;
                case Workplace.Denver:
                    availableFunctionsString.Add(Function.Denver_Bar.ToPlanningString());
                    availableFunctionsString.Add(PlannableFunction.StandBy.ToFriendlyString());
                    break;
                case Workplace.Barco:
                    availableFunctionsString.Add(Function.Denver_Bar.ToPlanningString());
                    availableFunctionsString.Add(PlannableFunction.StandBy.ToFriendlyString());
                    break;
                case Workplace.Keuken:
                    availableFunctionsString.Add(Function.Denver_Keuken.ToPlanningString());
                    availableFunctionsString.Add(Function.Barco_Denver_Afwas.ToPlanningString());
                    availableFunctionsString.Add(Function.BarcoKeuken.ToPlanningString());
                    availableFunctionsString.Add(PlannableFunction.StandBy.ToFriendlyString());
                    break;
                case Workplace.Fiesta:
                    availableFunctionsString.Add(Function.Fiesta_Afwas.ToPlanningString());
                    availableFunctionsString.Add(Function.Fiesta_Bar.ToPlanningString());
                    availableFunctionsString.Add(Function.Fiesta_Keuken.ToPlanningString());
                    availableFunctionsString.Add(PlannableFunction.StandBy.ToFriendlyString());
                    break;
                case Workplace.NoFunctionDetected:
                    break;
            }

            return availableFunctionsString.ToArray();
        }

        public Dictionary<string,int> countNumbers(DataGridView dgv)
        {
            Dictionary<string, int> dir = new Dictionary<string, int>();
            List<string> lstCountable = new List<string>();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                lstCountable.Add(row.Cells[3].Value.ToString().ToLower());
            }

            if (lstCountable.Count >= 1)
            {
                string str = "bediening";
                dir.Add(str, lstCountable.Where(s => s == str).Count());
                str = "denver keuken";
                dir.Add(str, lstCountable.Where(s => s == str).Count());
                str = "barco keuken";
                dir.Add(str, lstCountable.Where(s => s == str).Count());
                str = "keuken";
                dir.Add(str, lstCountable.Where(s => s == str).Count());
                str = "afwas";
                dir.Add(str, lstCountable.Where(s => s == str).Count());
                str = "stand-by";
                dir.Add(str, lstCountable.Where(s => s == str).Count());
            }
          
            return dir;
        }

        /// <summary>
        /// get a list of all staffmembers in the database
        /// </summary>
        /// <returns>string array of all staffmembers</returns>
        public string[] GetListOfStaffMembers()
        {
            return database.GetListOfStaffMembers(currentUser);
        }

        /// <summary>
        /// haalt alle werkplekken op die er zijn
        /// </summary>
        /// <returns>geeft een lijst met namen van werkplekken terug</returns>
        public string[] GetAllWorkplaces(int[] exceptions = null)
        {
            return database.getAllWorkplaces(exceptions);
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

        public void AddDayToYear(DateTime givenDate)
        {
            StaffMember memberToAdd = null;
            Year tempYear = null;
            bool yearExists = false;

            Classes.Models.Day dayToAdd = new Classes.Models.Day
            {
                Date = givenDate
            };

            //check if the year exists if it does add the day and member to the already existing year
            foreach (Year year in mainYearList)
            {
                if (givenDate.Year == year.ThisYear.Year)
                {
                    yearExists = true;
                    tempYear = year;
                }
            }

            //if the year does not exist we create a new year and add te day and member to the new year
            if (!yearExists)
            {
                tempYear = new Year(new DateTime(givenDate.Year, 0, 0));
                mainYearList.Add(tempYear);
            }

            tempYear.AddDay(dayToAdd, memberToAdd);
        }
    }
}