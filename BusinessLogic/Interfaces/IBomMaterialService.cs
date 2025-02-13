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
        Task<BomMaterialDto> GetBomMaterialByIdAsync(int id);
        Task<int> CreateBomMaterialAsync(BomMaterialDto material);
        Task<bool> UpdateBomMaterialAsync(BomMaterialDto material);
        Task<bool> DeleteBomMaterialAsync(int id);
        Task<IEnumerable<BomMaterialDto>> GetBomMaterialsWithPaginationAsync(int pageNumber, int pageSize);
    }
}
