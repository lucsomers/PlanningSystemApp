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
                    return "Geen werkplek beschikbaar";
                default:
                    return "Geen werkplek beschikbaar";
            }
        }

        public static int ToID(this Workplace me)
        {
            switch (me)
            {
                case Workplace.Directie:
                    return 1;
                case Workplace.Keuken:
                    return 2;
                case Workplace.Barco:
                    return 3;
                case Workplace.Denver:
                    return 4;
                case Workplace.Fiesta:
                    return 5;
                case Workplace.NoFunctionDetected:
                    return 0;
                default:
                    return 0;
            }
        }
    }
}