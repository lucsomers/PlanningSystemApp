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

        public string FillPlanningTableWithData(DataGridView tableToFill, MySqlConnection connection, Workplace currentUser,DateTime dateToFill, bool planning, int planningid, DatabaseUsers userdatabase, LogicalRepository repository)
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
                                int tempPlanningId = -1;

                                //planning id (hidden)
                                if (tempPlanningId == -1)
                                {
                                    //als planningid -1 is is deze niet meegegeven en dus moeten we deze even zoeken.
                                    tempPlanningId = GetIdOfStaffmemberAndDay(s.Id, d.Id, connection);
                                }
                                tableToFill.Rows[index].Cells[0].Value = tempPlanningId;

                                //staffmembername
                                //tableToFill.Rows[index].Cells[1].Value = s.Name;
                                //fill combobox with users for editing
                                DataGridViewComboBoxCell boxCell = (DataGridViewComboBoxCell)tableToFill.Rows[index].Cells[1];
                                boxCell.Items.AddRange(userdatabase.GetListOfAllStaffmMembers(connection, currentUser));
                                boxCell.Value = s.Name;

                                //starttime
                                tableToFill.Rows[index].Cells[2].Value = s.StartTime.ToShortTimeString();

                                //endtime
                                tableToFill.Rows[index].Cells[3].Value = s.EndTime.ToShortTimeString();

                                //planning boolean
                                //tableToFill.Rows[index].Cells[4].Value = s.FunctionOfDay.ToPlanningString();
                                //fill combobox with functions available to user for editing
                                DataGridViewComboBoxCell boxCell2 = (DataGridViewComboBoxCell)tableToFill.Rows[index].Cells[4];
                                boxCell2.Items.AddRange(repository.GetPlannableFunctionsAvailableToUser());
                                boxCell2.Value = s.FunctionOfDay.ToPlanningString();

                                //totaal gewerkte uren
                                TimeSpan span = s.AmountOfWorkedHours(planning);
                                tableToFill.Rows[index].Cells[5].Value = span.ToString(@"hh\:mm");

                                //pause_time
                                tableToFill.Rows[index].Cells[6].Value = s.PauseTime.ToShortTimeString();

                                //increment index with 1
                                index++;
                            }
                            

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

        public void SaveExpectedRevenue(bool planning, DateTime dateofday, int workplaceid, double expectedRevenue, MySqlConnection connection)
        {
            string sql = @"UPDATE `day` AS d
                           SET d.`expected_revenue`=@expectedrevenue
                           WHERE YEAR(d.`date`)=@year
                           AND MONTH(d.`date`)=@month                           
                           AND DAY(d.`date`)=@day                           
                           AND d.`planning`=@planning
                           AND d.`workplace_id`=@workplaceid";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("expectedrevenue", expectedRevenue);
            cmd.Parameters.AddWithValue("year", dateofday.Year);
            cmd.Parameters.AddWithValue("month", dateofday.Month);
            cmd.Parameters.AddWithValue("day", dateofday.Day);
            cmd.Parameters.AddWithValue("planning", planning);
            cmd.Parameters.AddWithValue("workplaceid", workplaceid);

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();
        }

        public void SaveChangesToPlanning(int planningid, StaffMember membertosave, MySqlConnection connection)
        {
            string sql = @"UPDATE `day_staff` AS ds
                           SET  ds.`staff_id`=@staffid,
                                ds.`start_time`=@starttime,
                                ds.`end_time`=@endtime,
                                ds.`pause_time`=@pausetime,
                                ds.`function_id`=@functionid 
                           WHERE ds.`id` = @planningid;
                           UPDATE `day` AS d
                           INNER JOIN `day_staff` AS ds
                           ON ds.`day_id` = d.`id`
                           SET d.`expected_revenue`=@expectedrevenue
                           WHERE ds.`id`=@planningid
                           AND ds.`day_id`=d.`id`";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("planningid", planningid);
            cmd.Parameters.AddWithValue("staffid", membertosave.Id);
            cmd.Parameters.AddWithValue("starttime", membertosave.StartTime);
            cmd.Parameters.AddWithValue("endtime", membertosave.EndTime);
            cmd.Parameters.AddWithValue("pausetime", membertosave.PauseTime);
            cmd.Parameters.AddWithValue("functionid", membertosave.FunctionOfDay.ToID());
            cmd.Parameters.AddWithValue("expectedrevenue", membertosave.Earnings);

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();
        }

        public void RemoveStaffmemberFromPlanningById(int planningid, MySqlConnection connection)
        {
            string sql = @"DELETE FROM `day_staff`
                           WHERE `day_staff`.`id` = @id";

            MySqlCommand cmd = new MySqlCommand(sql,connection);
            cmd.Parameters.AddWithValue("id", planningid);

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();
        }

        public int AddStaffMemberToPlanning(DateTime datetimeToPlan, bool planning, StaffMember staffMemberToPlan, int workplace_id, MySqlConnection connection)
        {
            int dayId = getIdOfYearMonthOrDay(datetimeToPlan, DayMonthYear.Day, planning, workplace_id, connection);
            //check if day exists
            if (dayId == -1)
            {
                //create a new record for given date.
                addDay(datetimeToPlan, workplace_id, planning, connection);

                //update day id
                dayId = getIdOfYearMonthOrDay(datetimeToPlan, DayMonthYear.Day, planning, workplace_id, connection);
            }

            //add staffmember to day
            string sql = @"INSERT INTO `day_staff`(`staff_id`, `day_id`, `start_time`, `end_time`, `pause_time`, `function_id`) 
                                           VALUES (@staffmemberid, @dayid, @starttime, @endtime, @pause_time, @functionid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("staffmemberid", staffMemberToPlan.Id);
            cmd.Parameters.AddWithValue("dayid", dayId);
            cmd.Parameters.AddWithValue("starttime", staffMemberToPlan.StartTime);
            cmd.Parameters.AddWithValue("endtime", staffMemberToPlan.EndTime);
            cmd.Parameters.AddWithValue("pause_time", staffMemberToPlan.PauseTime);
            cmd.Parameters.AddWithValue("functionid", staffMemberToPlan.FunctionOfDay.ToID());

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();

            return GetIdOfStaffmemberAndDay(staffMemberToPlan.Id, dayId,connection);
        }

        private int GetIdOfStaffmemberAndDay(int staffmemberid, int dayid, MySqlConnection connection)
        {
            int idToReturn = -1;
            string sql = @"SELECT ds.`id` 
                           FROM day_staff AS ds
                           WHERE ds.`staff_id` = @staffmemberid AND ds.`day_id` = @dayid";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("staffmemberid", staffmemberid);
            cmd.Parameters.AddWithValue("dayid", dayid);

            connection.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                idToReturn = int.Parse(reader["id"].ToString());
            }

            connection.Close();

            return idToReturn;
        }

        private void addDay(DateTime dateOfDay, int workplace_id , bool planning, MySqlConnection connection)
        {
            int idOfMonth = getIdOfYearMonthOrDay(dateOfDay, DayMonthYear.Month, planning, workplace_id, connection);

            //check if month exists
            if (idOfMonth == -1)
            {
                //add month to db
                addMonth(dateOfDay, workplace_id,connection);

                //update month id
                idOfMonth = getIdOfYearMonthOrDay(dateOfDay, DayMonthYear.Month, planning, workplace_id, connection);
            }

            //add new day
            int idOfDay = addNewDay(dateOfDay, planning, workplace_id, connection);

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

        private int addNewDay(DateTime dateOfDay, bool planning, int workplaceid, MySqlConnection connection)
        {
            int newDayId = -1;

            string sql = @"INSERT INTO `day`(`date`, `planning`, `workplace_id`) 
                                        VALUES (@date, @planning, @workplaceid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("date", dateOfDay);
            cmd.Parameters.AddWithValue("planning", planning);
            cmd.Parameters.AddWithValue("workplaceid", workplaceid);

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();

            //update dayid
            newDayId = getIdOfYearMonthOrDay(dateOfDay, DayMonthYear.Day, planning, workplaceid, connection);

            return newDayId;
        }

        private void addMonth(DateTime dateOfMonth, int workplace_id, MySqlConnection connection)
        {
            int idOfYear = getIdOfYearMonthOrDay(dateOfMonth, DayMonthYear.Year, false, workplace_id, connection);
           
            //check if year exists
            if (idOfYear == -1)
            {
                //add year to db
                AddYear(dateOfMonth, workplace_id, connection);

                //update year id
                idOfYear = getIdOfYearMonthOrDay(dateOfMonth, DayMonthYear.Year, false, workplace_id, connection);
            }

            //create a new month
            int idOfMonth = AddNewMonth(dateOfMonth, workplace_id, connection);
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

        private int AddNewMonth(DateTime date, int workplaceid, MySqlConnection connection)
        {
            int monthid = -1;

            string sql = @"INSERT INTO `month`(`date`,`workplace_id`) 
                                        VALUES (@date, @workplaceid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("date", date);
            cmd.Parameters.AddWithValue("workplaceid", workplaceid);

            connection.Open();

            cmd.ExecuteReader();

            connection.Close();

            monthid = getIdOfYearMonthOrDay(date, DayMonthYear.Month, false, workplaceid, connection);

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

        private int getIdOfYearMonthOrDay(DateTime dateOfDay, DayMonthYear dayMonth, bool planning, int workplaceid, MySqlConnection connection)
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
                           WHERE d.`date` = @date AND d.`planning` = @planning AND d.`workplace_id` = @workplaceid";
                    
                    cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("date", dateOfDay.Date.Date);
                    cmd.Parameters.AddWithValue("workplaceid", workplaceid);
                    cmd.Parameters.AddWithValue("planning", planning);
                    break;
                case DayMonthYear.Month:
                    sql = @"SELECT m.`id`
                           FROM `month` AS m
                           INNER JOIN `year` AS y
                           ON YEAR(y.`date`) = @year
                           WHERE MONTH(m.`date`) = @month AND m.`workplace_id` = @workplaceid";

                    
                    cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("month", dateOfDay.Date.Month);
                    cmd.Parameters.AddWithValue("workplaceid", workplaceid);
                    cmd.Parameters.AddWithValue("year", dateOfDay.Date.Year);
                    break;
                case DayMonthYear.Year:
                    sql = @"SELECT y.`id`
                           FROM `year` AS y
                           WHERE YEAR(y.`date`) = @year AND y.`workplace_id` = @workplaceid";


                    cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("year", dateOfDay.Date.Year);
                    cmd.Parameters.AddWithValue("workplaceid", workplaceid);
                    break;
                default:
                    connection.Close();
                    return id;
            }

            //execute cmd 
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                id = int.Parse(reader["id"].ToString());
            }
            connection.Close();

            return id;
        }

        private void LoadDaysOfMonth(List<Year> list, MySqlConnection connection, string workplaceToSearchFor, bool planning)
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

        private void LoadStaffmembersInDays(Month month, MySqlConnection connection, bool planning)
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
