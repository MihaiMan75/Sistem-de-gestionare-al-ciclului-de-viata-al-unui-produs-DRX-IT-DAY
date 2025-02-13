using BusinessLogic.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IStageService
    {
        Task<IEnumerable<StageDto>> GetAllStagesAsync();
        Task<StageDto> GetStageByIdAsync(int id);
        Task<int> CreateStageAsync(StageDto stage);
        Task<bool> UpdateStageAsync(StageDto stage);
        Task<bool> DeleteStageAsync(int id);
    }
}
