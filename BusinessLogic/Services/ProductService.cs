using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using BusinessLogic.DtoModels;
using BusinessLogic.Mappers;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class ProductService: IProductService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IRepository<Product> _productRepository;
        private readonly IBomService _bomService;
        private readonly IProductStageHistoryService _productStageHistoryService;


        public ProductService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _productRepository = repositoryFactory.CreateProductRepository();
            _bomService = new BomService(repositoryFactory);
            _productStageHistoryService = new ProductStageHistoryService(repositoryFactory);
        }

        public async Task<int> CreateProductAsync(ProductDto productDto)
        {
            await Validate(productDto);

             var bomId = await _bomService.CreateBomAsync(productDto.ProductBom);
                productDto.ProductBom.Id = bomId;
            //after creating the product id in the db we can create the product stage history
           
            var product = ProductMapper.FromDto(productDto);
            var productId = await _productRepository.AddAsync(product);
            foreach (var productStageHistory in productDto.StageHistory)
            {
                await _productStageHistoryService.CreateProductStageHistoryAsync(productStageHistory, productId);
            }
            return productId;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
          var products = await _productRepository.GetAllAsync();
            List<ProductDto> result = new List<ProductDto>();
            foreach (var product in products)
            {
                var bom = await _bomService.GetBomByIdAsync(product.bom_id); // also returns a DTO
                var productStageHistory = await _productStageHistoryService.GetProductStageHistoriesByProductIdAsync(product.id); //returns list of product stage history DTOs
                result.Add(ProductMapper.ToDto(product, bom, productStageHistory));
            }
            return result;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
           var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }
            //bom
            //product stage history
            //Curentstage
            var bom = await _bomService.GetBomByIdAsync(product.bom_id); // also returns a DTO
            var productStageHistory = await _productStageHistoryService.GetProductStageHistoriesByProductIdAsync(id); //returns list of product stage history DTOs
            if (bom == null || productStageHistory == null)
            {
                return null;
            }
            return ProductMapper.ToDto(product, bom, productStageHistory);

        }

        public async Task<bool> UpdateProductAsync(ProductDto productDto)
        {
            await Validate(productDto);
           
           await _bomService.UpdateBomAsync(productDto.ProductBom);
           
            var product = ProductMapper.FromDto(productDto);
            foreach (var productStageHistory in productDto.StageHistory)
            {
                await _productStageHistoryService.UpdateProductStageHistoryAsync(productStageHistory,productDto.Id);
            }
            return await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> AddProductStageAsync(ProductDto product, ProductStageHistoryDto newStage)
        {
            if (product == null)
                throw new ArgumentException($"Product with ID {product.Id} not found.");

            if (newStage == null || newStage.ProductStage == null)
                throw new ArgumentException("New stage cannot be null.");

            // Close the previous stage if it's still open
            var lastStage = product.StageHistory.FirstOrDefault(s => s.ProductStage == product.Curentstage);
            if (lastStage != null && lastStage.EndDate == lastStage.StartDate)
            {
                lastStage.EndDate = newStage.StartDate;
                await _productStageHistoryService.UpdateProductStageHistoryAsync(lastStage, product.Id);
            }

            product.Curentstage = newStage.ProductStage;

            return await _productStageHistoryService.CreateProductStageHistoryAsync(newStage, product.Id) > 0;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsWithPaginationAsync(int pageNumber, int pageSize)
        {

            if (pageNumber < 1) throw new ArgumentException("Page number must be greater than 0");
            if (pageSize < 1) throw new ArgumentException("Page size must be greater than 0");

            var products = await _productRepository.GetWithPaginationAsync(pageNumber,pageSize);
            List<ProductDto> result = new List<ProductDto>();
            foreach (var product in products)
            {
                var bom = await _bomService.GetBomByIdAsync(product.bom_id); // also returns a DTO
                var productStageHistory = await _productStageHistoryService.GetProductStageHistoriesByProductIdAsync(product.id); //returns list of product stage history DTOs
                result.Add(ProductMapper.ToDto(product, bom, productStageHistory));
            }

            return result;
        }

        private async Task Validate(ProductDto product) 
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(product.Description))
                throw new ArgumentException("Product Description cannot be empty.");

            if (product.EstimatedHeight <= 0)
                throw new ArgumentException("Estimated Height must be greater than zero.");

            if (product.EstimatedWidth <= 0)
                throw new ArgumentException("Estimated Width must be greater than zero.");

            if (product.EstimatedWeight <= 0)
                throw new ArgumentException("Estimated Weight must be greater than zero.");

            if (product.ProductBom == null)
                throw new ArgumentException("Product BOM cannot be null.");

            if (product.StageHistory == null || !product.StageHistory.Any())
                throw new ArgumentException("Product must have at least one stage in history.");

            var totalWeight = product.ProductBom.BomMaterials.Sum(bm => bm.Quantity * bm.Material.Weight);
            if (product.EstimatedWeight < totalWeight)
                throw new ArgumentException("Estimated Weight must be greater than the total weight of the materials in the BOM.");

            if (product.Curentstage == null)
                throw new ArgumentException("Current stage cannot be null.");
        }
    
    }
}
