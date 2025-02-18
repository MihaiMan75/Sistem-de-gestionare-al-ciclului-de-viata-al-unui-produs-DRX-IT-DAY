using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using BusinessLogic.Mappers;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class MaterialsService : IMaterialsService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IRepository<Material> _materialRepository;

        public MaterialsService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _materialRepository = repositoryFactory.CreateMaterialRepository();
        }


        public async Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync()
        {
            var materials = await _materialRepository.GetAllAsync();
            return MaterialMapper.ToDto(materials.ToList());
        }

        public async Task<MaterialDto> GetMaterialByIdAsync(int id)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            return MaterialMapper.ToDto(material);
        }

        public async Task<int> CreateMaterialAsync(MaterialDto materialDto)
        {
            await Validate(materialDto);

            var material = MaterialMapper.FromDto(materialDto);
            return await _materialRepository.AddAsync(material);
        }

        public async Task<bool> UpdateMaterialAsync(MaterialDto materialDto)
        {
            if (materialDto == null)
                throw new ArgumentNullException(nameof(materialDto));

            await Validate(materialDto);

            var material = MaterialMapper.FromDto(materialDto);
            return await _materialRepository.UpdateAsync(material);
        }

        public async Task<bool> DeleteMaterialAsync(int id)
        {
            return await _materialRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<MaterialDto>> GetMaterialsWithPaginationAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number must be greater than 0");
            if (pageSize < 1) throw new ArgumentException("Page size must be greater than 0");

            var materials = await _materialRepository.GetWithPaginationAsync(pageNumber, pageSize);
            return MaterialMapper.ToDto(materials.ToList());
        }

        private async Task Validate(MaterialDto material)
        {

            if (material == null)
                throw new ArgumentNullException(nameof(material));

            if (string.IsNullOrWhiteSpace(material.MaterialDescription))
                throw new ArgumentException("Material description cannot be empty");

            if (material.Weight <= 0)
                throw new ArgumentException("Weight must be greater than 0");

            if (material.Height <= 0)
                throw new ArgumentException("Height must be greater than 0");

            if (material.Width <= 0)
                throw new ArgumentException("Width must be greater than 0");
        }
    }
}
