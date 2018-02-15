using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcoDenverPlanningSysteem
{
    public enum Workplace
    {
        Directie,
        Denver,
        Barco,
        Keuken,
        Fiesta,
        NoFunctionDetected
    }

    public static class WorkplaceExtension
    {
        public static string ToFriendlyString(this Workplace me)
        {
            switch (me)
            {
                case Workplace.Directie:
                    return "Directie";
                case Workplace.Denver:
                    return "Denver";
                case Workplace.Barco:
                    return "Barco";
                case Workplace.Keuken:
                    return "Keuken";
                case Workplace.Fiesta:
                    return "Fiesta";
                case Workplace.NoFunctionDetected:
                    return "Onbekende functie";
                default:
                    return "Geen functie beschikbaar";
            }
        }
    }
}