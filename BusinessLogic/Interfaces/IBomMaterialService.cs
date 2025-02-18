using BusinessLogic.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IBomMaterialService
    {
        Task<IEnumerable<BomMaterialDto>> GetAllBomMaterialsAsync();
        Task<BomMaterialDto> GetBomMaterialByIdAsync(int boomId, int material_number);
        Task<int> CreateBomMaterialAsync(BomMaterialDto material);
        Task<bool> UpdateBomMaterialAsync(BomMaterialDto material);
        Task<bool> DeleteBomMaterialAsync(int boomId, int material_number);
        Task<List<BomMaterialDto>> GetMaterialsByBomIdAsync(int boomId);
        Task<IEnumerable<BomMaterialDto>> GetBomMaterialsWithPaginationAsync(int pageNumber, int pageSize);
    }
}
