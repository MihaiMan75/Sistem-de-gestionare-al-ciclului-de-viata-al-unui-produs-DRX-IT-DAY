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
        public static ProductStageHistoryDto ToDto(ProductStageHistory productStageHistory, Stage stage, UserDto user)
        {
            return new ProductStageHistoryDto
            {
                ProductStage = StageMapper.ToDto(stage),
                StartDate = productStageHistory.start_of_stage,
                EndDate = productStageHistory.end_of_stage,
                User = user

            };
        }
        public static List<ProductStageHistoryDto> ToDto(List<ProductStageHistory> productStageHistories, List<Stage> stages,List<UserDto> users)
        {
            var dtos = new List<ProductStageHistoryDto>();
            for (int i = 0; i < productStageHistories.Count; i++)
            {
                dtos.Add(ToDto(productStageHistories[i], stages[i], users[i]));
            }
            return dtos;
        }

        public static ProductStageHistory FromDto(ProductStageHistoryDto dto)
        {
            return new ProductStageHistory
            {
                stage_id = dto.ProductStage.Id, 
                start_of_stage = dto.StartDate,
                end_of_stage = dto.EndDate,
                id_user = dto.User.Id

            };
        }
        public static List<ProductStageHistory> FromDto(List<ProductStageHistoryDto> dtos)
        {
            var productStageHistories = new List<ProductStageHistory>();
            foreach (var dto in dtos)
            {
                productStageHistories.Add(FromDto(dto));
            }
            return productStageHistories;
        }
    }

}
