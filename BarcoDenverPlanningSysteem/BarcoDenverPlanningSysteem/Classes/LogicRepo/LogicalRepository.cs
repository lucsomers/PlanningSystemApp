﻿using BarcoDenverPlanningSysteem.Classes.Models;
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
        private bool firstTimeClearOfCells = true;

        private Workplace currentUser = Workplace.NoFunctionDetected;
        
        /// <summary>
        /// constuctor
        /// </summary>
        public LogicalRepository()
        {
            database.Connect();
        }


        /// <summary>
        /// deselect all cells of the dgv's in the array except the first one in the array
        /// </summary>
        /// <param name="dataGridViews"> an array of dgv's the first one is the exception the rest is cleared of selected cells</param>
        public void DeselectCells(DataGridView[] dataGridViews)
        {
            //first one is always exeption
            DataGridView Exeption = dataGridViews[0];

            if (firstTimeClearOfCells)
            {
                firstTimeClearOfCells = false;
                for (int i = 0; i < dataGridViews.Length; i++)
                {
                    if (Exeption != dataGridViews[i])
                    {
                        dataGridViews[i].ClearSelection();
                    }
                }

                firstTimeClearOfCells = true;
            }
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
        /// zoek id bij naam van medewerker
        /// </summary>
        /// <param name="staffmember">the name of the staffmember</param>
        /// <param name="dgv">optinal dgv to search in if not given we use database</param>
        /// <returns>id that goes with the name of the staffmember</returns>
        public int getIdFromName(string staffmember, DataGridView dgv = null)
        {
            int id = 0;
            if (dgv != null)
            {
                //use dgv
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells["name"].Value.ToString() == staffmember)
                    {
                        id = int.Parse(row.Cells["id"].Value.ToString());
                        break;
                    }
                }

                return id;
            }
            else
            {
                //use db
                id = database.getIdFromStaffMemberName(name: staffmember);
                return id;
            }
        }

        /// <summary>
        /// geeft de functie van de werknemer afhankelijk van welk id is meegegeven.
        /// </summary>
        /// <param name="id">de id van de medewerker</param>
        /// <returns>geeft de functie terug van de medewerker</returns>
        private Function planningStringToFunction(string functionName)
        {
            Function toReturn = Function.NoFunctionDetected;

            switch (functionName)
            {
                case "Keuken":
                    switch (currentUser)
                    {
                        case Workplace.Fiesta:
                            toReturn = Function.Fiesta_Keuken;
                            break;
                        default:
                            toReturn = Function.NoFunctionDetected;
                            break;
                    }
                    return toReturn;
                case "Bar":
                    switch (currentUser)
                    {
                        case Workplace.Denver:
                            toReturn = Function.Denver_Bar;
                            break;
                        case Workplace.Barco:
                            toReturn = Function.Barco_Bar;
                            break;
                        case Workplace.Fiesta:
                            toReturn = Function.Fiesta_Bar;
                            break;
                        default:
                            toReturn = Function.NoFunctionDetected;
                            break;
                    }
                    return toReturn;
                case "Afwas":
                    switch (currentUser)
                    {
                        case Workplace.Keuken:
                            toReturn = Function.Barco_Denver_Afwas;
                            break;
                        case Workplace.Fiesta:
                            toReturn = Function.Fiesta_Afwas;
                            break;
                        default:
                            toReturn = Function.NoFunctionDetected;
                            break;
                    }
                    return toReturn;
                case "Bediening":
                    switch (currentUser)
                    {
                        case Workplace.Denver:
                            toReturn = Function.Denver_Bar;
                            break;
                        case Workplace.Barco:
                            toReturn = Function.Barco_Bar;
                            break;
                        case Workplace.Fiesta:
                            toReturn = Function.Fiesta_Bar;
                            break;
                        default:
                            toReturn = Function.NoFunctionDetected;
                            break;
                    }
                    return toReturn;
                case "StandBy":
                    switch (currentUser)
                    {
                        case Workplace.Denver:
                            toReturn = Function.Denver_Bar_Standby;
                            break;
                        case Workplace.Barco:
                            toReturn = Function.Barco_Bar_Standby;
                            break;
                        case Workplace.Keuken:
                            toReturn = Function.DenverBarco_Keuken_Standby;
                            break;
                        default:
                            toReturn = Function.NoFunctionDetected;
                            break;
                    }
                    return toReturn;
                case "Keuken StandBy":
                    switch (currentUser)
                    {
                        case Workplace.Fiesta:
                            toReturn = Function.Fiesta_Keuken_Standby;
                            break;
                        default:
                            toReturn = Function.NoFunctionDetected;
                            break;
                    }
                    return toReturn;
                case "Afwas StandBy":
                    switch (currentUser)
                    {
                        case Workplace.Fiesta:
                            toReturn = Function.Fiesta_Afwas_Standby;
                            break;
                    }
                    return toReturn;
                case "Bediening StandBy":
                    switch (currentUser)
                    {
                        case Workplace.Fiesta:
                            toReturn = Function.Fiesta_Bar_Standby;
                            break;
                        default:
                            toReturn = Function.NoFunctionDetected;
                            break;
                    }
                    return toReturn;
                case "Barco keuken":
                    switch (currentUser)
                    {
                        case Workplace.Keuken:
                            toReturn = Function.BarcoKeuken;
                            break;
                        default:
                            toReturn = Function.NoFunctionDetected;
                            break;
                    }
                    return toReturn;
                case "Denver keuken":
                    switch (currentUser)
                    {
                        case Workplace.Keuken:
                            toReturn = Function.Denver_Keuken;
                            break;
                        default:
                            toReturn = Function.NoFunctionDetected;
                            break;
                    }
                    return toReturn;
                default:
                    break;
            }

            return toReturn;
        }

        public void DeleteStaffmembersFromPlanningByID(int[] idOfStaffMembers)
        {
            for (int i = 0; i < idOfStaffMembers.Length; i++)
            {
                database.RemoveStaffMemberFromPlanningById(idOfStaffMembers[i]);
            }
        }

        public int[] GetIdOfSelectedRow(DataGridView[] dataGridViews)
        {
            List<int> lstids = new List<int>();

            for (int i = 0; i < dataGridViews.Length; i++)
            {
                if (dataGridViews[i].SelectedRows.Count > 0)
                {
                    for (int r = 0; r < dataGridViews[i].SelectedRows.Count; r++)
                    {
                        lstids.Add(int.Parse(dataGridViews[i].SelectedRows[r].Cells[0].Value.ToString()));
                    }

                    break;
                }
            }

            return lstids.ToArray();
        }

        /// <summary>
        /// Haalt de standaard werkplek van een werknemer op.
        /// </summary>
        /// <param name="idOfStaffmember">id van een werknemer</param>
        /// <returns>planning string van een werknemer zijn standaard werkplek</returns>
        public string GetDefaultFunctionById(int idOfStaffmember)
        {
            return FunctionExtension.ToPlanningString(FunctionExtension.StringToEnum(database.GetFunctionFromStaffmember(idOfStaffmember)));
        }

        /// <summary>
        /// Creeërd een medewerker van de gegeven naam en planed deze meteen in voor de huidige ingelogde werknemer
        /// </summary>
        /// <param name="datetimeToPlan">de te plannen datum</param>
        /// <param name="reality">geeft weer of het planning of werkelijk is</param>
        /// <param name="name">de naam van de medewerker</param>
        /// <param name="startTime">de begin tijd van de werknemer</param>
        /// <param name="endTime">de eindtijd van de werknemer</param>
        /// <param name="pauseTime">de pauze tijd van de werknemer</param>
        public int AddStaffMemberToPlanning(string functionname, DateTime datetimeToPlan, bool reality, string name, DateTime startTime, DateTime endTime, DateTime pauseTime = new DateTime())
        {
            //create staffmember from name
            int id = getIdFromName(name);
            Function function = planningStringToFunction(functionname);
            StaffMember staffMemberToPlan = new StaffMember(id, function, pauseTime, startTime, endTime);
            
            //add person to the day in db.
            return database.AddStaffMemberToPlanning(datetimeToPlan,reality,staffMemberToPlan, currentUser.ToID());
        }

        /// <summary>
        /// checks a checkedlistbox to return the string of the currently selected item.
        /// </summary>
        /// <param name="box">the checkedlistbox to search in</param>
        /// <returns>returns text value of the checked checkbox or null if something went wrong</returns>
        public string GetFunctionTextFromCurrentCheckedBox(CheckedListBox box)
        {
            for (int i = 0; i < box.Items.Count; i++)
            {
                if (box.GetItemChecked(i))
                {
                    return box.GetItemText(box.Items[i]);
                }
            }

            return "";
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
            StaffMember tempMember = new StaffMember(0, name, earnings, FunctionExtension.StringToEnum(defaultWorkplace));
            
            database.AddStaffMember(tempMember);
            tempMember.Id = getIdFromName(name);
            database.AddStaffMember(tempMember, true);
        }

        public void FillPlanningtableWithData(DataGridView tableToFill, DateTime dateToFill, bool planning, TextBox textBox, int planningid)
        {
            //sets commentbox
            textBox.Text = database.FillPlanningTableWithData(tableToFill, currentUser, dateToFill, planning, planningid);
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

        /// <summary>
        /// updates staffmembers default function by id
        /// </summary>
        /// <param name="text">new function name</param>
        /// <param name="id">id of staffmember to update</param>
        public bool UpdateStaffMemberDefaultFunction(string text, int id)
        {
            Function f = FunctionExtension.StringToEnum(text);
            return database.UpdateStaffMemberFunction(f.ToID(), id);
        }

        public Dictionary<string,int> CountNumbers(DataGridView dgv)
        {
            Dictionary<string, int> dir = new Dictionary<string, int>();
            List<string> lstCountable = new List<string>();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[3].Value != null)
                {
                    lstCountable.Add(row.Cells[3].Value.ToString().ToLower());
                }
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
                str = "standby";
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