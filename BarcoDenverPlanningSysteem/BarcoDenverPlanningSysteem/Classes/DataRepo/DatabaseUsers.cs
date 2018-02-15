using BarcoDenverPlanningSysteem.Classes.Error;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BarcoDenverPlanningSysteem
{
    public class DatabaseUsers
    {
        ErrorHandler error = new ErrorHandler();
        

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

        public string[] GetAllWorkplaces(MySqlConnection connection)
        {
            //update on right position
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

            return lstToReturn.ToArray();
        }
    }
}