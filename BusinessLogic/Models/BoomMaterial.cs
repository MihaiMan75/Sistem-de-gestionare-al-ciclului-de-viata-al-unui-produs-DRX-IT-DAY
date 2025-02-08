using BusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogic.Models
{
    public class BoomMaterial
    {
        public int id { get; set; }
        public float quantity { get; set; }
        public UnitMeasure unit {  get; set; }
        public Material material { get; set; }


    }
}
