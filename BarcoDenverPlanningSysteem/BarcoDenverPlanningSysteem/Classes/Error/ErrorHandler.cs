using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcoDenverPlanningSysteem.Classes.Error
{
    class ErrorHandler
    {
        public string CantFillPlanningMessage()
        {
            return "Oeps! Er is iets fout gegaan met het in laden van de huidige planning." + Environment.NewLine + "Controleer uw internet verbinding en herstart de applicatie" + Environment.NewLine + "wanneer dit probleem zich blijft voordoen contacteer de administrator voor verderen hulp";
        }

        public string ToManyCheckBoxesCheckedMessage()
        {
            return "Het totaal aantal aangevinkte vakjes mag het maximum aantal van 1 niet overschrijden";
        }

        public string WrongTimeMessage()
        {
            return "De begin tijd is kleiner dan of gelijk aan de eindtijd vul alstublieft een geldig tijdsbereik";
        }
        
        public string NoRowsSelectedMessage()
        {
            return "Selecteer alstublieft een of meerderen rijen in de tabel";
        }

        public string LoginErrorMessage()
        {
            return "Code bestaat niet of :::: Zorg ervoor dat er alleen getallen in het textvak staan. Controlleer hierna of u een internet connectie hebt. Wanneer dit beide het geval is en u krijgt dit bericht neem dan contact op met uw administrator";
        }

        public string NotEveryThingFilledInErrorMessage()
        {
            return "Vul aub alle velden in!";
        }

        public string NoCodeErrorMessage()
        {
            return "Geen code beschikbaar";
        }

        public void ShowCantConnectMessage(Exception e)
        {
            System.Windows.Forms.DialogResult clickedButton = System.Windows.Forms.MessageBox.Show("Kan geen verbinding maken met de database controleer uw internet verbinding en probeer opnieuw. Wilt u de volledige error message zien klik dan op JA", "Error", System.Windows.Forms.MessageBoxButtons.YesNo);

            if (clickedButton == System.Windows.Forms.DialogResult.Yes)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
        }

        public string SavedSucceeded()
        {
            return "Uw wijzigingen zijn opgeslagen.";
        }

        public string SomethingWentWrong()
        {
            return "Er is iets fout gegaan bij het doorvoeren van de wijziging probeer het later opnieuw. Als dit probleem zich voor blijft doen raadpleeg dan uw administrator.";
        }
    }
}
