﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace BarcoDenverPlanningSysteem
{
    public class DatabaseRepository
    {
        MySqlConnection connection = new MySqlConnection();
        DatabaseUsers dbUsers = new DatabaseUsers();

        /// <summary>
        /// geeft de database zijn connectie een connectie string zodat er querrys uitgevoerd kunnen worden
        /// </summary>
        public void Connect()
        {
            connection.ConnectionString = "server=127.0.0.1;user id=root;password=123;database=marcelapp";
        }

        /// <summary>
        /// zoekt welke werkplek er bij de code hoort
        /// </summary>
        /// <param name="code">de inlogcode van een werkplek</param>
        /// <returns>geeft een werkplek terug die hoort bij de meegegeven code</returns>
        public Workplace CheckCodeForLogin(int code)
        {
            return dbUsers.CheckCodeForLogin(code, connection);
        }

        /// <summary>
        /// haalt alle werkplekken op die er zijn
        /// </summary>
        /// <returns>geeft een lijst met namen van werkplekken terug</returns>
        public string[] getAllWorkplaces()
        {
            return dbUsers.GetAllWorkplaces(connection);
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
    }
}