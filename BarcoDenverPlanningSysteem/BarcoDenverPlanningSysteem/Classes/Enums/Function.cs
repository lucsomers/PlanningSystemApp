using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcoDenverPlanningSysteem
{
    public enum Function
    {
        Denver_Bar,
        Denver_Bar_Standby,
        Denver_Keuken,
        Barco_Bar,
        Barco_Bar_Standby,
        BarcoKeuken,
        DenverBarco_Keuken_Standby,
        Fiesta_Bar,
        Fiesta_Bar_Standby,
        Fiesta_Keuken,
        Fiesta_Keuken_Standby,
        Fiesta_Afwas,
        Fiesta_Afwas_Standby,
        Barco_Denver_Afwas,
        Barco_Denver_Afwas_Standby,
        NoFunctionDetected
    }
    public static class FunctionExtension
    {
        public static Workplace GetWorkplaceOfFunction(this Function me)
        {
            switch (me)
            {
                case Function.Denver_Bar:
                    return Workplace.Denver;
                case Function.Denver_Bar_Standby:
                    return Workplace.Denver;
                case Function.Denver_Keuken:
                    return Workplace.Keuken;
                case Function.Barco_Bar:
                    return Workplace.Barco;
                case Function.Barco_Bar_Standby:
                    return Workplace.Barco;
                case Function.BarcoKeuken:
                    return Workplace.Keuken;
                case Function.DenverBarco_Keuken_Standby:
                    return Workplace.Keuken;
                case Function.Fiesta_Bar:
                    return Workplace.Fiesta;
                case Function.Fiesta_Bar_Standby:
                    return Workplace.Fiesta;
                case Function.Fiesta_Keuken:
                    return Workplace.Fiesta;
                case Function.Fiesta_Keuken_Standby:
                    return Workplace.Fiesta;
                case Function.Fiesta_Afwas:
                    return Workplace.Fiesta;
                case Function.Fiesta_Afwas_Standby:
                    return Workplace.Fiesta;
                case Function.Barco_Denver_Afwas:
                    return Workplace.Keuken;
                case Function.Barco_Denver_Afwas_Standby:
                    return Workplace.Keuken;
                case Function.NoFunctionDetected:
                    return Workplace.NoFunctionDetected;
                default:
                    return Workplace.NoFunctionDetected;
            }
        }

        public static Function StringToEnum(string s)
        {
            switch (s.ToLower())
            {
                case "barco keuken":
                    return Function.BarcoKeuken;

                case "barco bar":
                    return Function.Barco_Bar;

                case "afwas denver en barco":
                    return Function.Barco_Denver_Afwas;

                case "denver bar":
                    return Function.Denver_Bar;

                case "denver keuken":
                    return Function.Denver_Keuken;

                case "fiesta afwas":
                    return Function.Fiesta_Afwas;

                case "fiesta bediening":
                    return Function.Fiesta_Bar;

                case "fiesta keuken":
                    return Function.Fiesta_Keuken;

                case "denver bar standby":
                    return Function.Denver_Bar_Standby;

                case "barco bar standby":
                    return Function.Barco_Bar_Standby;

                case "denverbarco keuken stanby":
                    return Function.DenverBarco_Keuken_Standby;

                case "fiesta bar standby":
                    return Function.Fiesta_Bar_Standby;

                case "fiesta keuken standby":
                    return Function.Fiesta_Keuken_Standby;

                case "fiesta afwas standby":
                    return Function.Fiesta_Afwas_Standby;

                case "barcoDenver afwas standby":
                    return Function.Barco_Denver_Afwas_Standby;

                default:
                    return Function.NoFunctionDetected;
            }
        }

        public static string ToFunctionString(this Function me)
        {
            switch (me)
            {
                case Function.NoFunctionDetected:
                    return "Geen functie beschikbaar";
                case Function.Denver_Bar:
                    return "Denver bar";
                case Function.Denver_Keuken:
                    return "Denver keuken";
                case Function.Barco_Bar:
                    return "Barco bar";
                case Function.BarcoKeuken:
                    return "Barco keuken";
                case Function.Fiesta_Bar:
                    return "Fiesta bediening";
                case Function.Fiesta_Keuken:
                    return "Fiesta keuken";
                case Function.Fiesta_Afwas:
                    return "Fiesta afwas";
                case Function.Barco_Denver_Afwas:
                    return "afwas Denver en Barco";
                case Function.Denver_Bar_Standby:
                    return "Denver Bar Standby";
                case Function.Barco_Bar_Standby:
                    return "Barco Bar Standby";
                case Function.DenverBarco_Keuken_Standby:
                    return "DenverBarco Keuken Stanby";
                case Function.Fiesta_Bar_Standby:
                    return "Fiesta Bar Standby";
                case Function.Fiesta_Keuken_Standby:
                    return "Fiesta Keuken Standby";
                case Function.Fiesta_Afwas_Standby:
                    return "Fiesta Afwas Standby";
                case Function.Barco_Denver_Afwas_Standby:
                    return "BarcoDenver Afwas Standby";
                default:
                    return "Geen functie beschikbaar";
            }
        }

        public static string ToPlanningString(this Function me)
        {
            switch (me)
            {

                case Function.NoFunctionDetected:
                    return "Geen functie beschikbaar";
                case Function.Denver_Bar:
                    return "Bediening";
                case Function.Denver_Keuken:
                    return "Denver keuken";
                case Function.Barco_Bar:
                    return "Bediening";
                case Function.BarcoKeuken:
                    return "Barco keuken";
                case Function.Fiesta_Bar:
                    return "Bediening";
                case Function.Fiesta_Keuken:
                    return "Keuken";
                case Function.Fiesta_Afwas:
                    return "Afwas";
                case Function.Barco_Denver_Afwas:
                    return "Afwas";
                case Function.Denver_Bar_Standby:
                    return "Standby";
                case Function.Barco_Bar_Standby:
                    return "Standby";
                case Function.DenverBarco_Keuken_Standby:
                    return "Standby";
                case Function.Fiesta_Bar_Standby:
                    return "Bediening Standby";
                case Function.Fiesta_Keuken_Standby:
                    return "Keuken Standby";
                case Function.Fiesta_Afwas_Standby:
                    return "Afwas Standby";
                case Function.Barco_Denver_Afwas_Standby:
                    return "Standby";
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
                case Function.Fiesta_Keuken_Standby:
                    return 9;
                case Function.Fiesta_Bar_Standby:
                    return 10;
                case Function.Denver_Bar_Standby:
                    return 11;
                case Function.Barco_Bar_Standby:
                    return 12;
                case Function.DenverBarco_Keuken_Standby:
                    return 13;
                case Function.Barco_Denver_Afwas_Standby:
                    return 14;
                case Function.Fiesta_Afwas_Standby:
                    return 15;
                case Function.NoFunctionDetected:
                    return 0;
                default:
                    return 0;
            }
        }
    }
}