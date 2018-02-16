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
        [Obsolete("Function is obsolent because we fill the datagridview in this class now")]
        public List<Year> LoadYearOfCurrentUser(MySqlConnection connection, Workplace currentUser)
        {
            List<Year> toReturn = new List<Year>();

            LoadYears(toReturn, connection, currentUser);
            LoadMonthsOfYear(toReturn, connection, getWorkplaceDatabaseString(currentUser));
            LoadDaysOfMonth(toReturn, connection, getWorkplaceDatabaseString(currentUser));

            return toReturn;
        }

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
                               FROM month AS m
                               INNER JOIN year_month AS ym
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

        public void FillPlanningTableWithData(DataGridView tableToFill, MySqlConnection connection, Workplace currentUser)
        {
            List<Year> lstYear = new List<Year>();

            LoadYears(lstYear, connection, currentUser);
            LoadMonthsOfYear(lstYear, connection, getWorkplaceDatabaseString(currentUser));
            LoadDaysOfMonth(lstYear, connection, getWorkplaceDatabaseString(currentUser));
        }

        private void LoadDaysOfMonth(List<Year> list, MySqlConnection connection, string workplaceToSearchFor)
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
                                new List<StaffMember>());
                        }
                    }
                    connection.Close();

                    //add staffmembers to day
                    LoadStaffmembersInDays(m);
                    //add days to month
                }
            }
        }

        private void LoadStaffmembersInDays(Month month)
        {
            //TODO:A HIGH write complex sql string to get staff from day in month
            string sql = @"SELECT s.``"
        }
    }
}
