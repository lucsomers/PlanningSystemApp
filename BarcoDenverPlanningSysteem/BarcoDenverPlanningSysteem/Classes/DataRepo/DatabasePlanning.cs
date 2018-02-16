using BarcoDenverPlanningSysteem.Classes.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcoDenverPlanningSysteem.Classes.DataRepo
{
    public class DatabasePlanning
    {
        public List<Year> LoadYearOfCurrentUser(MySqlConnection connection, Workplace currentUser)
        {
            List<Year> toReturn = new List<Year>();

            LoadYears(toReturn, connection, getWorkplaceDatabaseString(currentUser));
            LoadMonthsOfYear(toReturn, connection, getWorkplaceDatabaseString(currentUser));
            LoadDaysOfMonth(toReturn, connection, getWorkplaceDatabaseString(currentUser));

            return toReturn;
        }

        private string getWorkplaceDatabaseString(Workplace currentUser)
        {
            return currentUser.ToFriendlyString();
        }

        private void LoadYears(List<Year> list, MySqlConnection connection, string workplaceToSearchFor)
        {
            //get all years and create a list of them
            string sql = @"SELECT * FROM `workplace`";

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            List<string> lstToReturn = new List<string>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //TODO:get all years and create a list of them database
                }
            }
            connection.Close();
        }

        private void LoadMonthsOfYear(List<Year> list, MySqlConnection connection, string workplaceToSearchFor)
        {
            //get all months for each year and then add them to the corresponding year 
            string sql = @"SELECT * FROM `workplace`";

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            List<string> lstToReturn = new List<string>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //TODO:load months database
                }
            }
            connection.Close();
        }

        private void LoadDaysOfMonth(List<Year> list, MySqlConnection connection, string workplaceToSearchFor)
        {
            //get all days for each month and then add them to the corresponding month
            string sql = @"SELECT * FROM `workplace`";

            connection.Open();

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            List<string> lstToReturn = new List<string>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //TODO: load days database
                }
            }
            connection.Close();
        }
    }
}
