using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class Enums
    {
        //Hardcoded values for the stages,roles and units of measure
            public enum Stages
            {
                Concept = 1,
                Fezabilitate = 2,
                Proiectare = 3,
                Productie = 4,
                Retragere = 5,
                StandBy = 6,
                Cancel = 7
            };

        public enum Roles
        {
            Creator_Concept =1,
            Engineer = 2,
            Designer = 3,
            Production_Manager = 4,
            Portfolio_Manager = 5,
            Admin = 6
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
