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
    public class ProductStageHistoryService: IProductStageHistoryService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ProductStageHistoryRepository _productStageHitoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Stage> _stageRepository;
        private readonly IUserService _userService;

        public ProductStageHistoryService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _productStageHitoryRepository = repositoryFactory.CreateProductStageHistoryRepository();
            _userRepository = repositoryFactory.CreateUserRepository();
            _stageRepository = repositoryFactory.CreateStageRepository();
            _userService = new UserService(repositoryFactory);
        }

        public async Task<int> CreateProductStageHistoryAsync(ProductStageHistoryDto productStageHistoryDto,int productId)
        {
            await Validate(productStageHistoryDto);

            var productStageHistory = ProductStageHistoryMapper.FromDto(productStageHistoryDto,productId);
            return await _productStageHitoryRepository.AddAsync(productStageHistory);
        }

        public async Task<bool> DeleteProductStageHistoryAsync(int productId, int stageId)
        {
           return await _productStageHitoryRepository.DeleteAsync(productId);   
        }

        public async Task<IEnumerable<ProductStageHistoryDto>> GetAllProductsHistoryAsync()
        {
            var productStageHisotory = await _productStageHitoryRepository.GetAllAsync();
            var stages = new List<Stage>();
            var users = new List<UserDto>();
            foreach (var productStage in productStageHisotory)
            {
                stages.Add(await _stageRepository.GetByIdAsync(productStage.stage_id));
                users.Add(await _userService.GetUserByIdAsync(productStage.id_user));
            }
        
            return ProductStageHistoryMapper.ToDto(productStageHisotory.ToList(), stages,users); //Hope it works
        }

        public async Task<List<ProductStageHistoryDto>> GetProductStageHistoriesByProductIdAsync(int ProductId)
        {
            var productStageHistories = await _productStageHitoryRepository.GetProductStageHistoriesByProductIdAsync(ProductId);
            var stages = new List<Stage>();
            var users = new List<UserDto>();
            foreach (var productStage in productStageHistories)
            {
                stages.Add(await _stageRepository.GetByIdAsync(productStage.stage_id));
                users.Add(await _userService.GetUserByIdAsync(productStage.id_user));
            }
            return ProductStageHistoryMapper.ToDto(productStageHistories.ToList(), stages, users);
        }

        public async Task<ProductStageHistoryDto> GetProductStageHistoryByIdAsync(int productId, int stageId)
        {
            var productStageHisotory= await _productStageHitoryRepository.GetByIdAsync(productId);
            var stage = await _stageRepository.GetByIdAsync(stageId);
            var user = await _userService.GetUserByIdAsync(productStageHisotory.id_user);
            return ProductStageHistoryMapper.ToDto(productStageHisotory, stage, user);
        }

        public async Task<bool> UpdateProductStageHistoryAsync(ProductStageHistoryDto productStageHistoryDto,int productId)
        {
            await Validate(productStageHistoryDto);

            var productStageHistory = ProductStageHistoryMapper.FromDto(productStageHistoryDto,productId);
            return await _productStageHitoryRepository.UpdateAsync(productStageHistory);
        }

        private async Task Validate(ProductStageHistoryDto productStageHistory)
        {
            if (productStageHistory == null)
                throw new ArgumentNullException(nameof(productStageHistory));

            if (productStageHistory.EndDate < productStageHistory.StartDate && productStageHistory.EndDate > DateTime.MinValue)
                throw new Exception(productStageHistory.EndDate + " must be greater than " + productStageHistory.StartDate);

            try
            {
                await _userRepository.ExistsAsync(productStageHistory.User.Id);
            }
            catch(Exception ex)
            {
                throw new Exception("You need to be logged in");
            } 
            if(productStageHistory.ProductStage == null && !await _stageRepository.ExistsAsync(productStageHistory.ProductStage.Id))
                throw new Exception("Stage must Exist in the database");
        }
    }
}
