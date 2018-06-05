using BarcoDenverPlanningSysteem.Classes.Error;
using BarcoDenverPlanningSysteem.Classes.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BarcoDenverPlanningSysteem
{ 
    public class DatabaseUsers
    {
        ErrorHandler error = new ErrorHandler();
        
        public void AddStaffMember(StaffMember member, MySqlConnection connection)
        {
            string sql = @"INSERT INTO `staff`
                                  ( `earnings`, `name`,`multiple_workplaces`,`default_function_id`) 
                           VALUES (@earnings,@name,0,@defaultWorkplace)";

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            
            cmd.Parameters.AddWithValue("name", member.Name);
            cmd.Parameters.AddWithValue("earnings", member.Earnings);
            cmd.Parameters.AddWithValue("defaultWorkplace", member.DefaultFunction.ToID());

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                error.ShowCantConnectMessage(e);
            }

            connection.Close();
        }

        public void DeleteStaffMemberById(int id, MySqlConnection connection)
        {
            string sql = @"DELETE FROM `staff`
                           WHERE `id` = @id";

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                error.ShowCantConnectMessage(e);
            }

            connection.Close();
        }

        public string GetFunctionFromStaffmember(int staffmemberid, MySqlConnection connection)
        {
            string sql = @"SELECT `function`.`name` AS name
FROM `staff` 
INNER JOIN `function` 
ON `function`.`id` = `staff`.`default_function_id` 
WHERE `staff`.`id` = @id";

            string toReturn = "";

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("id", staffmemberid);

            try
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        toReturn = String.Format("{0}",reader["name"]);
                    }
                }
            }
            catch (Exception e)
            {
                error.ShowCantConnectMessage(e);
            }

            connection.Close();

            return toReturn;
        }

        public int getIdFromStaffMemberName(MySqlConnection connection, string name)
        {
            int id = -1;

            string sql = @"SELECT s.`id`
                          FROM `staff` AS s
                          WHERE s.`name` = @name";

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("name", name);

            try
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        id = int.Parse(reader["id"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                error.ShowCantConnectMessage(e);
            }

            connection.Close();

            return id;
        }

        public void FillViewWithAllUsers(DataGridView tableView, MySqlConnection connection)
        {
            string sql = @"SELECT `staff`.`id`,
`staff`.`earnings`,
`staff`.`name`,
`staff`.`multiple_workplaces`,
`function`.`name` AS function_name 
FROM `staff` 
LEFT JOIN `function` 
ON `function`.`id` = `staff`.`default_function_id`";
           
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
            try
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        DataTable temp = new DataTable();
                        temp.Load(reader);
                        tableView.DataSource = temp;
                    }
                }
            }
            catch (Exception e)
            {
                error.ShowCantConnectMessage(e);
            }

            connection.Close();
        }

        public bool UpdateStaffMemberFunctionId(int newFunctionid, int staffmemberid, MySqlConnection connection)
        {
            string sql = @"UPDATE `staff`
                           SET `staff`.`default_function_id`=@functionid
                           WHERE `staff`.`id` = @staffmemberid;";
            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@functionid", newFunctionid);
                cmd.Parameters.AddWithValue("@staffmemberid", staffmemberid);

                cmd.ExecuteNonQuery();

                connection.Close();
                return true;
            }
            catch (Exception)
            {
                
            }
            return false;
        }

        public Workplace CheckCodeForLogin(int code, MySqlConnection connection)
        {
            int iCode = code;
            Workplace toReturn = Workplace.NoFunctionDetected;

            string sql = @"SELECT `workplace`.`name`
                           FROM `inloggen`
                           INNER JOIN `workplace` ON `inloggen`.`werkplek`=`workplace`.`id`
                           WHERE `code` = @code;";

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@code", iCode);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string sToCheck = String.Format("{0}", reader["name"]);

                        switch (sToCheck)
                        {
                            case "Barco":
                                {
                                    toReturn = Workplace.Barco;
                                    break;
                                }

                            case "Denver":
                                {
                                    toReturn = Workplace.Denver;
                                    break;
                                }

                            case "Keuken":
                                {
                                    toReturn = Workplace.Keuken;
                                    break;
                                }

                            case "Directie":
                                {
                                    toReturn = Workplace.Directie;
                                    break;
                                }

                            case "Fiesta":
                                {
                                    toReturn = Workplace.Fiesta;
                                    break;
                                }

                            default:
                                toReturn = Workplace.NoFunctionDetected;
                                break;
                        }
                    }
                }

                connection.Close();
            }
            catch (Exception e)
            {
                error.ShowCantConnectMessage(e);
            } 

                
            return toReturn;
        }

        public string[] GetListOfAllStaffmMembers(MySqlConnection connection, Workplace currentUser)
        {
            List<string> lstToreturn = new List<string>();

            string sql = @"SELECT s.`name`
                          FROM `staff` AS s
                          INNER JOIN `workplace_staff` AS ws
                          ON ws.`workplace_id` = @workplaceID
                          WHERE s.`id` = ws.`staff_id`";

            if (currentUser == Workplace.Directie)
            {
                sql = @"SELECT s.`name` FROM `staff` AS s";
            }
            

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            //add parameters
            cmd.Parameters.AddWithValue("workplaceID", currentUser.ToID());

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    lstToreturn.Add(reader["name"].ToString());
                }
            }

            connection.Close();

            return lstToreturn.ToArray();
        }

        public string GetCodeFromFunction(string function, MySqlConnection connection)
        {
            string toReturn = "";
            string sql = @"SELECT `inloggen`.`code`
                          FROM `inloggen`
                          INNER JOIN `workplace` ON `workplace`.`id`=`inloggen`.`werkplek` 
                          WHERE `workplace`.`name`=@workplace";

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            
            cmd.Parameters.AddWithValue("@workplace", function);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                toReturn = reader["code"].ToString();
            }
            else
            {
                toReturn = error.NoCodeErrorMessage(); 
            }

            connection.Close();

            return toReturn;
        }

        public void EditCode(string functie, int newcode, MySqlConnection connection)
        {
            string sWerkplek = "";
            int iCode = newcode;

            sWerkplek = functie;

            if (sWerkplek != "")
            {
                //update on right position
                string sql = @"UPDATE `inloggen`
                           INNER JOIN `workplace` ON `inloggen`.`werkplek`=`workplace`.`id`
                           SET `inloggen`.`code`=@code
                           WHERE `workplace`.`name` = @werkplek;";

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@code", iCode);
                cmd.Parameters.AddWithValue("@werkplek", sWerkplek);

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public string[] GetAllWorkplaces(MySqlConnection connection, int[] exceptions = null)
        {
            string sql = @"SELECT * FROM `workplace`";

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            List<string> lstToReturn = new List<string>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lstToReturn.Add(reader["name"].ToString());
                }
            }
            connection.Close();

            if(exceptions != null)
            {
                foreach(int index in exceptions)
                {
                    lstToReturn.RemoveAt(index);
                }
            }

            return lstToReturn.ToArray();
        }
    }
}