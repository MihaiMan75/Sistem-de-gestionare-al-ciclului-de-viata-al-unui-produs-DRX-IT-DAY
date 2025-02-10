using BusinessLogic.DtoModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    static class MaterialMapper
    {
        public static MaterialDto ToDto(Material material)
        {
            return new MaterialDto
            {
                MaterialNumber = material.material_number,
                MaterialDescription = material.material_description,
                Weight = material.weight,
                Height = material.height,
                Width = material.width
            };
        }

        public static List<MaterialDto> ToDto(List<Material> materials)
        {
            List<MaterialDto> materialsList = new List<MaterialDto>();
            foreach (var material in materials)
            {
                materialsList.Add(ToDto(material));
            }
            
            return materialsList;
        }

        public static Material FromDto(MaterialDto dto)
        {
            return new Material
            {
                material_number = dto.MaterialNumber,
                material_description = dto.MaterialDescription,
                weight = dto.Weight,
                height = dto.Height,
                width = dto.Width
            };
        }

        public static List<Material> FromDto(List<MaterialDto> dtos)
        {
            List<Material> materialsList = new List<Material>();
            foreach (var dto in dtos)
            {
                materialsList.Add(FromDto(dto));
            }
            return materialsList;
        }
    }
}
