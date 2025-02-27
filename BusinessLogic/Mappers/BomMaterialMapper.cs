using BusinessLogic.DtoModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    static class BomMaterialMapper // 1 material to 1 bom
    {
        public static BomMaterialDto ToDto(BomMaterial bomMaterial, Material material)
        {
            return new BomMaterialDto
            {
                BomId = bomMaterial.bom_id,
                Quantity = bomMaterial.qty,
                UnitMeasureCode = bomMaterial.unit_measure_code,
                Material = MaterialMapper.ToDto(material)
            };
        }
        
        public static List<BomMaterialDto> ToDto(List<BomMaterial> bomMaterials, List<Material> materials)
        {
            List<BomMaterialDto> dtos = new List<BomMaterialDto>();
            for (int i = 0; i < bomMaterials.Count; i++)
            {
                dtos.Add(ToDto(bomMaterials[i], materials[i]));
            }
            return dtos;
        }

        

        public static BomMaterial FromDto(BomMaterialDto dto)
        {
            
            return new BomMaterial
            {
                bom_id = dto.BomId,
                qty = dto.Quantity,
                material_number = dto.Material.MaterialNumber,
                unit_measure_code = dto.UnitMeasureCode
            };
        }

    }
}
