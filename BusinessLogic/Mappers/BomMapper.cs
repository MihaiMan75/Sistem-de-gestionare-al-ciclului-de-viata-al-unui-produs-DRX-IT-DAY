using BusinessLogic.DtoModels;
using DataAccess.Models;

namespace BusinessLogic.Mappers
{
    static class BomMapper
    {
        public static BomDto ToDto(Bom bom, List<BomMaterialDto> bomMaterials)
        {
            return new BomDto
            {
                Id = bom.id,
                Name = bom.name,
                BomMaterials = bomMaterials
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
