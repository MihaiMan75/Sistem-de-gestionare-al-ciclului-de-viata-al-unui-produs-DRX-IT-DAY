using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class BomMaterial
    {
        public int bom_id {  get; set; }
        public int material_number { get; set; }
        public double qty { get; set; }
        public string unit_measure_code { get; set; }
    }
}
