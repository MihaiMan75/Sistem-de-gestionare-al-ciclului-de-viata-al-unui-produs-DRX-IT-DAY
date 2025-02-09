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

        public static Stage FromDto(StageDto dto)
        {
            return new Stage
            {
                id = dto.Id,
                name = dto.Name,
                description = dto.Description
            };
        }

    }
}
