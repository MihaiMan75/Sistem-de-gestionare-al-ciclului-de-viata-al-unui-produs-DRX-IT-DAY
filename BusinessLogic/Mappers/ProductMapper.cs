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
        public static ProductDto ToDto(Product product, BomDto bomDto, List<ProductStageHistoryDto> productStageHistoryDtos)
        {
            var currentDate= DateTime.Now;
            return new ProductDto
            {
                Id = product.id,
                Name = product.name,
                Description = product.description,
                EstimatedHeight = product.estimated_height,
                EstimatedWidth = product.estimated_width,
                EstimatedWeight = product.estimated_weight,
                ProductBom = bomDto,
                StageHistory = productStageHistoryDtos,
                Curentstage = productStageHistoryDtos
                   .Where(stage => stage.StartDate <= currentDate)
                    .MaxBy(stage => stage.StartDate)?.ProductStage
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
