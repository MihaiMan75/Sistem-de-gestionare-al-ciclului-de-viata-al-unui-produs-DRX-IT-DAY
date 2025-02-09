using BusinessLogic.DtoModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(Product product, Bom bom, BomMaterial bomMaterial, List<Material> materials, List<ProductStageHistory> productStageHistories, Dictionary<int, Stage> stagesDict)
        {
            var lastStage = stagesDict[productStageHistories.MaxBy(psh => psh.endOfStage).stageId];
            return new ProductDto
            {
                Id = product.id,
                Name = product.name,
                Description = product.description,
                EstimatedHeight = product.estimated_height,
                EstimatedWidth = product.estimated_width,
                EstimatedWeight = product.estimated_weight,
                ProductBom = BomMapper.ToDto(bom, bomMaterial, materials),
                StageHistory = ProductStageHistoryMapper.ToDto(productStageHistories, stagesDict),
                Curentstage = StageMapper.ToDto(lastStage)
            };
        }

        public static Product FromDto(ProductDto dto)
        {
            return new Product
            {
                id = dto.Id,
                name = dto.Name,
                description = dto.Description,
                estimated_height = dto.EstimatedHeight,
                estimated_width = dto.EstimatedWidth,
                estimated_weight = dto.EstimatedWeight,
                bom_id=dto.ProductBom.Id
            };
        }
    }
}
