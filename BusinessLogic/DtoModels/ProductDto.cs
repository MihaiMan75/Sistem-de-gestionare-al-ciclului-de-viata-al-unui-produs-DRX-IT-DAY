using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DtoModels
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double EstimatedHeight { get; set; }
        public double EstimatedWidth { get; set; }
        public double EstimatedWeight { get; set; }
        public BomDto ProductBom { get; set; }
        public List<ProductStageHistoryDto> StageHistory { get; set; } = new List<ProductStageHistoryDto>();
        public StageDto Curentstage {  get; set; }
    }
}
