using BusinessLogic.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DtoModels;
using BusinessLogic.Mappers;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class StageService: IStageService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IRepository<Stage> _stageRepository;

        public StageService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _stageRepository = repositoryFactory.CreateStageRepository();
        }

        public async Task<int> CreateStageAsync(StageDto stageDto)
        {
            await Validate(stageDto);

            var stage = StageMapper.FromDto(stageDto);
            return await _stageRepository.AddAsync(stage);
        }

        public async Task<bool> DeleteStageAsync(int id)
        {
            return await _stageRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<StageDto>> GetAllStagesAsync()
        {
            var stages = await _stageRepository.GetAllAsync();
            return StageMapper.ToDto(stages.ToList());
        }

        public async Task<StageDto> GetStageByIdAsync(int id)
        {
            var stage = await _stageRepository.GetByIdAsync(id);
            if(stage == null)
            {
                return null;
            }
            return StageMapper.ToDto(stage);
        }

        public async Task<bool> UpdateStageAsync(StageDto stageDto)
        {
            await Validate(stageDto);

            var stage = StageMapper.FromDto(stageDto);
            return await _stageRepository.UpdateAsync(stage);
        }

        private async Task Validate(StageDto stage)
        {
            if (stage == null)
                throw new ArgumentNullException(nameof(stage));
            if (string.IsNullOrWhiteSpace(stage.Name))
                throw new ArgumentException("Stage Name cannot be empty");
            if (string.IsNullOrWhiteSpace(stage.Description))
                throw new ArgumentException("Stage Description cannot be empty");
        }
    }
}
