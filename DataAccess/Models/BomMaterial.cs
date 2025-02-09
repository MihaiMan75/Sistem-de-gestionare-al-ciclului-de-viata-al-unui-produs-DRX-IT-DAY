using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class BomMaterial
    {
        public int bomId {  get; set; }
        public int materialNumber { get; set; }
        public double qty { get; set; }
        public string unitMeasureCode { get; set; }//de vazut cum tratam partea de ucum
    }
}
