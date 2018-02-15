using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcoDenverPlanningSysteem
{
    public class LogicalRepository
    {
        private DatabaseRepository database = new DatabaseRepository();
        private DatabaseUsers dbUsers = new DatabaseUsers();

        private Workplace currentUser = Workplace.NoFunctionDetected;
        
        public LogicalRepository()
        {
            database.Connect();
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
        public string[] GetFunctionsAvailableToUser()
        {
            List<string> availableFunctions = new List<string>();

            switch (currentUser)
            {
                case Workplace.Directie:
                    availableFunctions.Add(Function.Afwas.ToFriendlyString());
                    availableFunctions.Add(Function.Bediening.ToFriendlyString());
                    availableFunctions.Add(Function.Keuken.ToFriendlyString());
                    break;
                case Workplace.Denver:
                    availableFunctions.Add(Function.Bediening.ToFriendlyString());
                    break;
                case Workplace.Barco:
                    availableFunctions.Add(Function.Bediening.ToFriendlyString());
                    break;
                case Workplace.Keuken:
                    availableFunctions.Add(Function.Keuken.ToFriendlyString());
                    availableFunctions.Add(Function.Afwas.ToFriendlyString());
                    break;
                case Workplace.Fiesta:
                    availableFunctions.Add(Function.Afwas.ToFriendlyString());
                    availableFunctions.Add(Function.Bediening.ToFriendlyString());
                    availableFunctions.Add(Function.Keuken.ToFriendlyString());
                    break;
                case Workplace.NoFunctionDetected:
                    break;
            }

            //elke lijst krijgt deze objecten
            availableFunctions.Add(Function.StandBy.ToFriendlyString());

            return availableFunctions.ToArray();
        }

        /// <summary>
        /// haalt alle werkplekken op die er zijn
        /// </summary>
        /// <returns>geeft een lijst met namen van werkplekken terug</returns>
        public string[] GetAllWorkplaces()
        {
            return database.getAllWorkplaces();
            //return readWriter.GetAllWorkplaces();
        }

        /// <summary>
        /// veranderd de inlogcode van een werkplek naar de meegegeven code
        /// </summary>
        /// <param name="newcode">de nieuwe code die de oude gaat vervangen</param>
        /// <param name="workplace">de werkplek waarvan de inlogcode moet worden veranderd</param>
        public void EditCode(string newCode, string werkplek)
        {
            //readWriter.EditCode(werkplek, newCode);
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
    }
}