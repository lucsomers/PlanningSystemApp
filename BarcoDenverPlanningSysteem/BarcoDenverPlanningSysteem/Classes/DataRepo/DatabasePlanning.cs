using BarcoDenverPlanningSysteem.Classes.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcoDenverPlanningSysteem.Classes.DataRepo
{
    public class DatabasePlanning
    {
        private string getWorkplaceDatabaseString(Workplace currentUser)
        {
            return currentUser.ToFriendlyString();
        }

        private void LoadYears(List<Year> list, MySqlConnection connection, Workplace workplaceToSearchFor)
        {
            //get all years and create a list of them
            string sql = @"SELECT 
                           y.`id`,
                           y.`date`
                           FROM `year` AS y
                           INNER JOIN `workplace` AS w
                           ON y.`workplace_ID` = w.`id`
                           WHERE @workplaceID = y.`workplace_id`";

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("workplaceID", workplaceToSearchFor.ToID());

            MySqlDataReader reader = cmd.ExecuteReader();

            List<string> lstToReturn = new List<string>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Year tempYear = new Year(reader.GetDateTime("date"), reader.GetInt32("id"));
                    list.Add(tempYear);
                }
            }
            connection.Close();
        }

        private void LoadMonthsOfYear(List<Year> list, MySqlConnection connection, string workplaceToSearchFor)
        {
            foreach (Year y in list)
            {
                string sql = @"SELECT m.`id`, m.`date`
                               FROM `month` AS m
                               INNER JOIN `month_year` AS ym
                               ON ym.`year_id` = @yearID
                               WHERE ym.`month_id` = m.`id`";
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("yearID", y.Id);

                MySqlDataReader reader = cmd.ExecuteReader();

                List<string> lstToReturn = new List<string>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Month tempMonth = new Month(reader.GetDateTime("date"), reader.GetInt32("id"));
                        y.MonthsInYear.Add(tempMonth);
                    }
                }
                connection.Close();
            }
        }

        public string FillPlanningTableWithData(DataGridView tableToFill, MySqlConnection connection, Workplace currentUser,DateTime dateToFill, int planning)
        {
            List<Year> lstYear = new List<Year>();

            string toReturn = "";

            LoadYears(lstYear, connection, currentUser);
            LoadMonthsOfYear(lstYear, connection, getWorkplaceDatabaseString(currentUser));
            LoadDaysOfMonth(lstYear, connection, getWorkplaceDatabaseString(currentUser), planning);

            foreach (Year y in lstYear)
            {
                foreach (Month m in y.MonthsInYear)
                {
                    foreach (Models.Day d in m.DaysInMonth)
                    {
                        if (d.Date == dateToFill.Date)
                        {
                            int index = 0;
                            
                            foreach (StaffMember s in d.StaffMembers)
                            {
                                tableToFill.Rows.Add(new DataGridViewRow());
                                tableToFill.Rows[index].Cells[0].Value = s.Name;
                                tableToFill.Rows[index].Cells[1].Value = s.StartTime.ToString();
                                tableToFill.Rows[index].Cells[2].Value = s.EndTime.ToString();
                                tableToFill.Rows[index].Cells[3].Value = s.FunctionOfDay.ToPlanningString();
                                tableToFill.Rows[index].Cells[4].Value = s.AmountOfWorkedHours(false);
                            }
                            index++;

                            if (d.StaffMembers.Count >= 1)
                            {
                                toReturn = d.DayComment;
                            }
                        }
                    }
                }
            }

            return toReturn;
        }

        public void AddStaffMemberToPlanning(DateTime datetimeToPlan, bool reality, StaffMember staffMemberToPlan, MySqlConnection connection)
        {
            int dayId = getIdOfDay(datetimeToPlan, connection);
            //add staff member to planning 
        }

        private int getIdOfDay(DateTime date, MySqlConnection connection)
        {
            int id = -1;
            //fill in the querry to get id from date
            string sql = "";

            connection.Open();
            MySqlCommand cmd = new MySqlCommand(sql, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {

                }
            }
            connection.Close();

            return id;
        }

        private void LoadDaysOfMonth(List<Year> list, MySqlConnection connection, string workplaceToSearchFor, int planning)
        {
            string sql = @"SELECT d.`id`, d.`date`, d.`admin_comment`, d.`day_comment`, 
                                          d.`staff_comment`, d.`expected_revenue`, d.`kitchen_revenue`, 
                                          d.`bar_revenue`, d.`planning`
                                          FROM `day` AS d
                                          INNER JOIN `day_month` AS dm
                                          ON dm.`month_id` = @monthID
                                          WHERE d.`id` = dm.`day_id`";
            foreach (Year y in list)
            {
                foreach (Month m in y.MonthsInYear)
                {
                    //write querry

                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    //add parameters
                    cmd.Parameters.AddWithValue("monthID", m.Id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    List<string> lstToReturn = new List<string>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //create day from data
                            Models.Day tempDay = new Models.Day(reader.GetDouble("expected_revenue"),
                                0,
                                reader.GetInt32("id"),
                                reader.GetDateTime("date"),
                                reader.GetString("staff_comment"),
                                reader.GetString("admin_comment"),
                                reader.GetString("day_comment"),
                                new List<StaffMember>());
                            m.DaysInMonth.Add(tempDay);
                        }
                    }
                    connection.Close();

                    //add staffmembers to day
                    LoadStaffmembersInDays(m, connection, planning);
                    //add days to month
                }
            }
        }

        private void LoadStaffmembersInDays(Month month, MySqlConnection connection, int planning)
        {
            string sql = @"SELECT s.`id`, s.`earnings`, s.`name` AS staffMemberName, s.`default_function_id`,
                                  f.`name` AS functionName, ds.`start_time`, ds.`end_time`, ds.`pause_time`
                                  FROM `staff` AS s
                                  INNER JOIN `day_staff` AS ds
                                  ON s.`id` = ds.`staff_id`
                                  INNER JOIN `function` AS f
                                  ON ds.`function_id` = f.`id`
                                  INNER JOIN `day` AS d
                                  ON d.`id` = ds.`day_id`
                                  WHERE ds.`day_id` = @dayID
                                  AND d.`planning` = @b";

            foreach (Models.Day d in month.DaysInMonth)
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                //add parameters
                cmd.Parameters.AddWithValue("dayID", d.Id);
                cmd.Parameters.AddWithValue("b",planning);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //create staffmember from data
                        StaffMember tempMember = new StaffMember(reader.GetInt32("id"), reader.GetString("staffMemberName"), reader.GetDouble("earnings"),
                            Function.NoFunctionDetected, Function.NoFunctionDetected,
                            reader.GetTimeSpan("pause_time"), reader.GetTimeSpan("start_time"), reader.GetTimeSpan("end_time"));

                        //change string function to enum function
                        tempMember.FunctionOfDay = FunctionExtension.StringToEnum(reader.GetString("functionName"));

                        //add staffmember to day
                        d.AddStaffMember(tempMember);
                    }
                }
                connection.Close();
            }
        }
    }
}
