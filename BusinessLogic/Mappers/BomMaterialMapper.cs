using BusinessLogic.DtoModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    static class BomMaterialMapper
    {
        public static BomMaterialDto ToDto(BomMaterial bomMaterial, List<Material> materials)
        {
            return new BomMaterialDto
            {
                BomId = bomMaterial.bomId,
                Quantity = bomMaterial.qty,
                UnitMeasureCode = bomMaterial.unitMeasureCode,
                Materials = MaterialMapper.ToDto(materials)
            };
        }

        public static BomMaterial FromDto(BomMaterialDto dto)
        {
            
            return new BomMaterial
            {
                bomId = dto.BomId,
                qty = dto.Quantity,
                unitMeasureCode = dto.UnitMeasureCode
            };
        }
    }
}
