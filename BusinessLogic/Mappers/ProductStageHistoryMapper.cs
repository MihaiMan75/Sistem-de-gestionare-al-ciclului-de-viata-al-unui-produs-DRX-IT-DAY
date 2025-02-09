using BusinessLogic.DtoModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    public static class ProductStageHistoryMapper
    {
        public static ProductStageHistoryDto ToDto(ProductStageHistory productStageHistory, Stage stage)
        {
            return new ProductStageHistoryDto
            {
                ProductStage = StageMapper.ToDto(stage), 
                StartDate = productStageHistory.startOfStage,
                EndDate = productStageHistory.endOfStage
            };
        }
        public static List<ProductStageHistoryDto> ToDto(List<ProductStageHistory> productStageHistories, Dictionary<int, Stage> stagesDict)
        {
            return productStageHistories.Select(psh =>
            {
                var stage = stagesDict.ContainsKey(psh.stageId) ? stagesDict[psh.stageId] : null;
                return ToDto(psh, stage);
            }).ToList();
        }

        public static ProductStageHistory FromDto(ProductStageHistoryDto dto)
        {
            return new ProductStageHistory
            {
                stageId = dto.ProductStage.Id, 
                startOfStage = dto.StartDate,
                endOfStage = dto.EndDate
            };
        }
        public static List<ProductStageHistory> FromDto(List<ProductStageHistoryDto> dtos)
        {
            return dtos.Select(FromDto).ToList();
        }
    }

}
