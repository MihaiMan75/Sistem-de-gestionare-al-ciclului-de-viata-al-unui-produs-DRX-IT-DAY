using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogic.Models
{
    public class BomMaterialDto
    {
        public float Quantity { get; set; }
        public string UnitMeasureCode { get; set; } 
        public List<MaterialDto> Materials { get; set; } = new List<MaterialDto>();
        public int BomId { get; set; }
    }
}
