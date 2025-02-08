using BusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogic.Models
{
    public class BomMaterial
    {
        public int Id { get; set; }
        public float Quantity { get; set; }
        public string UnitMeasureCode { get; set; } 
        public List<Material> Materials { get; set; } = new List<Material>();
        public int BomId { get; set; }
        public Bom Bom { get; set; }
    }
}
