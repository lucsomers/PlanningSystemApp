using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcoDenverPlanningSysteem
{
    public enum Function
    {
        Denver_Bar,
        Denver_Keuken,
        Barco_Bar,
        BarcoKeuken,
        Fiesta_Bar,
        Fiesta_Keuken,
        Fiesta_Afwas,
        Barco_Denver_Afwas,
        NoFunctionDetected
        
    }
    public static class FunctionExtension
    {
        public static string ToFriendlyString(this Function me)
        {
            switch (me)
            {

                case Function.NoFunctionDetected:
                    return "Geen functie beschikbaar";
                case Function.Denver_Bar:
                    return "Bar";
                case Function.Denver_Keuken:
                    return "Keuken";
                case Function.Barco_Bar:
                    return "Bar";
                case Function.BarcoKeuken:
                    return "Keuken";
                case Function.Fiesta_Bar:
                    return "Bar";
                case Function.Fiesta_Keuken:
                    return "Keuken";
                case Function.Fiesta_Afwas:
                    return "Afwas";
                case Function.Barco_Denver_Afwas:
                    return "Afwas";
                default:
                    return "Geen functie beschikbaar";
            }
        }

        public static int ToID(this Function me)
        {
            switch (me)
            {
                case Function.Denver_Bar:
                    return 1;
                case Function.Denver_Keuken:
                    return 2;
                case Function.Barco_Bar:
                    return 3;
                case Function.BarcoKeuken:
                    return 4;
                case Function.Fiesta_Bar:
                    return 5;
                case Function.Fiesta_Keuken:
                    return 6;
                case Function.Fiesta_Afwas:
                    return 7;
                case Function.Barco_Denver_Afwas:
                    return 8;
                case Function.NoFunctionDetected:
                    return 0;
                default:
                    return 0;
            }
        }
    }
}