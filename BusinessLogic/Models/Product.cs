using BusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class Product
    {
        public int id { get; set; }
        List<BoomMaterial> materials { get; set; }
        public string description { get; set; }
        public float estimatedHeight { get; set; }
        public float estimatedWidth { get; set; }
        public float estimatedW { get; set; }
        public Dictionary<Stage, DateTime> stageHistory { get; set; } = new();
        public string name { get; set; }
    }
}
