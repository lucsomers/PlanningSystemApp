using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcoDenverPlanningSysteem
{
    public enum Function
    {
        Afwas,
        StandBy,
        Bediening,
        Keuken,
        DenverBar,
        BarcoBar,
        NoFunctionDetected
        
    }
    public static class FunctionExtension
    {
        public static string ToFriendlyString(this Function me)
        {
            switch (me)
            {
                case Function.Afwas:
                    return "Afwas";
                case Function.StandBy:
                    return "Stand-By";
                case Function.Bediening:
                    return "Bediening";
                case Function.Keuken:
                    return "Keuken";
                case Function.BarcoBar:
                    return "Barco";
                case Function.DenverBar:
                    return "Denver";
                case Function.NoFunctionDetected:
                    return "NonFunctionDetected";
                default:
                    return "NonFunctionDetected";
            }
        }
    }
}