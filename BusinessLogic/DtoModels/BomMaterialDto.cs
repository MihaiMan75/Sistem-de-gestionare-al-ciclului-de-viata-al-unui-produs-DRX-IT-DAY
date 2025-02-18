using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogic.DtoModels
{
    public class BomMaterialDto
    {
        public double Quantity { get; set; }
        public string UnitMeasureCode { get; set; } 
        public MaterialDto Material { get; set; }
        public int BomId { get; set; }
    }
}
