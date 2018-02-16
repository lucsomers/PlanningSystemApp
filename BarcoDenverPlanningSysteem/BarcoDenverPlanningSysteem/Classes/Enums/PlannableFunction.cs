using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcoDenverPlanningSysteem
{
    public enum PlannableFunction
    {
        Keuken,
        Bar,
        Afwas,
        Bediening,
        StandBy,
        NoFunctionDetected
    }
    public static class PlannableFunctionExtension
    {
        public static string ToFriendlyString(this PlannableFunction me)
        {
            switch (me)
            {
                case PlannableFunction.Keuken:
                    return "Keuken";
                case PlannableFunction.Bar:
                    return "Bar";
                case PlannableFunction.Afwas:
                    return "Afwas";
                case PlannableFunction.Bediening:
                    return "Bediening";
                case PlannableFunction.StandBy:
                    return "StandBy";
                case PlannableFunction.NoFunctionDetected:
                    return "Geen functie beschikbaar";
                default:
                    return "Geen functie beschikbaar";
            }
        }
    }
}