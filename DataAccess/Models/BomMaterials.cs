using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccess.Dtos
{
    public class BomMaterials
    {
        public int bomId {  get; set; }
        public int materialNumber { get; set; }
        public double qty { get; set; }
        //de vazut ce facem cu ucum
        public string unitMeasureCode { get; set; }
    }
}
