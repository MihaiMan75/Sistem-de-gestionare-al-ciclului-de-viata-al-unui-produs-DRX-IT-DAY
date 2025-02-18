using BusinessLogic.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IProductStageHistoryService // Not Final Implementation, this may be a flawed design
    {
        Task<IEnumerable<ProductStageHistoryDto>> GetAllProductsHistoryAsync();
        Task<ProductStageHistoryDto> GetProductStageHistoryByIdAsync(int productId,int stageId);
        Task<int> CreateProductStageHistoryAsync(ProductStageHistoryDto productStageHistory);
        Task<bool> UpdateProductStageHistoryAsync(ProductStageHistoryDto productStageHistory);
        Task<bool> DeleteProductStageHistoryAsync(int productId, int stageId);
        Task<List<ProductStageHistoryDto>> GetProductStageHistoriesByProductIdAsync(int ProductId);
    }
}
