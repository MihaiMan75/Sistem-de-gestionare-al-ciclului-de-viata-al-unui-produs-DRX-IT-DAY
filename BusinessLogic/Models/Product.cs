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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float EstimatedHeight { get; set; }
        public float EstimatedWidth { get; set; }
        public float EstimatedWeight { get; set; }
        public Bom ProductBom { get; set; }
        public List<ProductStageHistory> StageHistory { get; set; } = new List<ProductStageHistory>();
        public Stage Curentstage {  get; set; }
    }
}
