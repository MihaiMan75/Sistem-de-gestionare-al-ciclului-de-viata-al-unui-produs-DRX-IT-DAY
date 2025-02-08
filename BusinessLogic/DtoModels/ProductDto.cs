using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float EstimatedHeight { get; set; }
        public float EstimatedWidth { get; set; }
        public float EstimatedWeight { get; set; }
        public BomDto ProductBom { get; set; }
        public List<ProductStageHistoryDto> StageHistory { get; set; } = new List<ProductStageHistoryDto>();
        public StageDto Curentstage {  get; set; }
    }
}
