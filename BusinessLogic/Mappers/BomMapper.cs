using BusinessLogic.DtoModels;
using DataAccess.Models;

namespace BusinessLogic.Mappers
{
    static class BomMapper
    {
        public static BomDto ToDto(Bom bom, BomMaterial bomMaterial,List<Material> materials)
        {
            return new BomDto
            {
                Id = bom.id,
                Name = bom.name,
                BomMaterial = BomMaterialMapper.ToDto(bomMaterial,materials)
            };
        }

        public static Bom FromDto(BomDto dto)
        {
            return new Bom
            {
                id = dto.Id,
                name = dto.Name
            };
        }

    }
}
