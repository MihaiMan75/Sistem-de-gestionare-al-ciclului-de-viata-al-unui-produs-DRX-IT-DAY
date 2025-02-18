using BusinessLogic.DtoModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    static class StageMapper
    {
        public static StageDto ToDto(Stage stage)
        {
            return new StageDto
            {
                Id = stage.id,
                Name = stage.name,
                Description = stage.description
            };
        }
        public static List<StageDto> ToDto(List<Stage> stages)
        {
            List<StageDto> dtos = new List<StageDto>();
            foreach (Stage stage in stages)
            {
                dtos.Add(ToDto(stage));
            }
            return dtos;

        }

        public static Stage FromDto(StageDto dto)
        {
            return new Stage
            {
                id = dto.Id,
                name = dto.Name,
                description = dto.Description
            };
        }

        public static List<Stage> FromDto(List<StageDto> dtos)
        {
            List<Stage> stages = new List<Stage>();
            foreach (StageDto dto in dtos)
            {
                stages.Add(FromDto(dto));
            }
            return stages;
        }

    }
}
