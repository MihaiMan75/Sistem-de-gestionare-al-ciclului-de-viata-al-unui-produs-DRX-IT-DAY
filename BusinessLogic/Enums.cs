using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class Enums
    {
        //Maybe populate them from the db
        public enum Stage
        {
            Design = 1,
            Prototype = 2,
            Production = 3,
            Testing = 4,
            Completed = 5
        };

        public enum Roles
        {
            Admin = 1,
            User = 2
        };

        public enum UCUM
        {
            // Quantity
            Piece = 1,
            // Length
            Millimeter = 2,
            // Weight
            Grams = 3
        }

    }
}
