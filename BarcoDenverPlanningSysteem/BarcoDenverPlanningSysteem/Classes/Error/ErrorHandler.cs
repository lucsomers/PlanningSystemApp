﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcoDenverPlanningSysteem.Classes.Error
{
    class ErrorHandler
    {
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
    }
}
