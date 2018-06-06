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

        public void AddStaffMemberToPlanning(DateTime datetimeToPlan, bool planning, StaffMember staffMemberToPlan, int workplace_id, MySqlConnection connection)
        {
            int dayId = getIdOfYearMonthOrDay(datetimeToPlan, DayMonthYear.Day, planning, connection);
            //check if day exists
            if (dayId == -1)
            {
                //create a new record for given date.
                addDay(datetimeToPlan, workplace_id, planning, connection);

                //update day id
                dayId = getIdOfYearMonthOrDay(datetimeToPlan, DayMonthYear.Day, planning, connection);
            }

            //add staffmember to day
            string sql = @"INSERT INTO `day_staff`(`staff_id`, `day_id`, `start_time`, `end_time`, `pause_time`, `function_id`) 
                                           VALUES (@staffmemberid, @dayid, @starttime, @endtime, @pause_time, @functionid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("staffmemberid", staffMemberToPlan.Id);
            cmd.Parameters.AddWithValue("dayid", dayId);
            cmd.Parameters.AddWithValue("starttime", staffMemberToPlan.StartTime.TimeOfDay);
            cmd.Parameters.AddWithValue("endtime", staffMemberToPlan.EndTime.TimeOfDay);
            cmd.Parameters.AddWithValue("pause_time", staffMemberToPlan.PauseTime.TimeOfDay);
            cmd.Parameters.AddWithValue("functionid", staffMemberToPlan.FunctionOfDay.ToID());

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();
        }

        private void addDay(DateTime dateOfDay, int workplace_id , bool planning, MySqlConnection connection)
        {
            int idOfMonth = getIdOfYearMonthOrDay(dateOfDay, DayMonthYear.Month, planning, connection);

            //check if month exists
            if (idOfMonth == -1)
            {
                //add month to db
                addMonth(dateOfDay, workplace_id,connection);

                //update month id
                idOfMonth = getIdOfYearMonthOrDay(dateOfDay, DayMonthYear.Month, planning, connection);
            }

            //add new day
            int idOfDay = addNewDay(dateOfDay, planning, connection);

            //add day to month
            ConnectDayToMonth(idOfDay, idOfMonth,connection);
        }

        private void ConnectDayToMonth(int dayId, int monthId, MySqlConnection connection)
        {
            string sql = @"INSERT INTO `day_month`(`month_id`,`day_id`) 
                                        VALUES (@monthid,@dayid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("monthid", monthId);
            cmd.Parameters.AddWithValue("dayid", dayId);

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();
        }

        private int addNewDay(DateTime dateOfDay, bool planning, MySqlConnection connection)
        {
            int newDayId = -1;

            string sql = @"INSERT INTO `day`(`date`, `planning`) 
                                        VALUES (@date, @planning)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("date", dateOfDay);
            cmd.Parameters.AddWithValue("planning", planning);

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();

            //update dayid
            newDayId = getIdOfYearMonthOrDay(dateOfDay, DayMonthYear.Day, planning, connection);

            return newDayId;
        }

        private void addMonth(DateTime dateOfMonth, int workplace_id, MySqlConnection connection)
        {
            int idOfYear = getIdOfYearMonthOrDay(dateOfMonth, DayMonthYear.Year, false, connection);
           
            //check if year exists
            if (idOfYear == -1)
            {
                //add year to db
                AddYear(dateOfMonth, workplace_id, connection);

                //update year id
                idOfYear = getIdOfYearMonthOrDay(dateOfMonth, DayMonthYear.Year, false, connection);
            }

            //create a new month
            int idOfMonth = AddNewMonth(dateOfMonth, connection);
            //add month to year
            ConnectMonthToYear(idOfMonth, idOfYear,connection);
        }

        private void ConnectMonthToYear(int idOfMonth, int idOfYear,MySqlConnection connection)
        {
            string sql = @"INSERT INTO `month_year`(`year_id`,`month_id`) 
                                        VALUES (@yearid,@monthid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("yearid", idOfYear);
            cmd.Parameters.AddWithValue("monthid", idOfMonth);

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();
        }

        private int AddNewMonth(DateTime date, MySqlConnection connection)
        {
            int monthid = -1;

            string sql = @"INSERT INTO `month`(`date`) 
                                        VALUES (@date)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("date", date);

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();

            monthid = getIdOfYearMonthOrDay(date, DayMonthYear.Month, false, connection);

            return monthid;
        }

        private void AddYear(DateTime dateOfYear, int workplace_id, MySqlConnection connection)
        {
            string sql = @"INSERT INTO `year`(`date`, `workplace_id`) 
                                       VALUES (@date,@workplace_id)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("date", dateOfYear);
            cmd.Parameters.AddWithValue("workplace_id", workplace_id);

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();
        }

        private int getIdOfYearMonthOrDay(DateTime dateOfDay, DayMonthYear dayMonth, bool planning, MySqlConnection connection)
        {
            int id = -1;
            string sql = "";
            MySqlCommand cmd = null;
            connection.Open();

            switch (dayMonth)
            {
                case DayMonthYear.Day:
                    sql = @"SELECT d.`id`
                           FROM `day` AS d
                           WHERE d.`date` = @date AND d.`planning` = @planning";
                    
                    cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("date", dateOfDay.Date.Date);
                    cmd.Parameters.AddWithValue("planning", planning);
                    break;
                case DayMonthYear.Month:
                    sql = @"SELECT m.`id`
                           FROM `month` AS m
                           INNER JOIN `year` AS y
                           ON YEAR(y.`date`) = @year
                           WHERE MONTH(m.`date`) = @month";

                    
                    cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("month", dateOfDay.Date.Month);
                    cmd.Parameters.AddWithValue("year", dateOfDay.Date.Year);
                    break;
                case DayMonthYear.Year:
                    sql = @"SELECT y.`id`
                           FROM `year` AS y
                           WHERE YEAR(y.`date`) = @year";


                    cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("year", dateOfDay.Date.Year);
                    break;
                default:
                    connection.Close();
                    return id;
            }

            //execute cmd 
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
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
                            reader.GetDateTime("pause_time"), reader.GetDateTime("start_time"), reader.GetDateTime("end_time"));

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
