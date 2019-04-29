using System.Collections.Generic;

namespace BrainNotFound.Paper.Services
{
    public static class SalutationType
    {

        public static string Mr => "Mr";
        public static string Mrs => "Mrs";
        public static string Ms => "Ms";
        public static string Miss => "Miss";
        public static string Dr => "Dr";

        public static List<string> All
        {
            get
            {
                return new List<string> { Mr, Mrs, Ms, Miss, Dr }; ;
            }
        }
    }
}