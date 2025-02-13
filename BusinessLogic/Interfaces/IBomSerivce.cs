using BusinessLogic.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IBomSerivce
    {
        Task<IEnumerable<BomDto>> GetAllBomsAsync();
        Task<BomDto> GetBomByIdAsync(int id);
        Task<int> CreateBomAsync(BomDto bom);
        Task<bool> UpdateBomAsync(BomDto bom);
        Task<bool> DeleteBomAsync(int id);
        Task<IEnumerable<BomDto>> GetBomsWithPaginationAsync(int pageNumber, int pageSize);
    }
}
