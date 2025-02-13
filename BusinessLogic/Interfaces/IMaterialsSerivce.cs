using BusinessLogic.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMaterialsService
    {
        Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync();
        Task<MaterialDto> GetMaterialByIdAsync(int id);
        Task<int> CreateMaterialAsync(MaterialDto material);
        Task<bool> UpdateMaterialAsync(MaterialDto material);
        Task<bool> DeleteMaterialAsync(int id);
        Task<IEnumerable<MaterialDto>> GetMaterialsWithPaginationAsync(int pageNumber, int pageSize);

    }
}
