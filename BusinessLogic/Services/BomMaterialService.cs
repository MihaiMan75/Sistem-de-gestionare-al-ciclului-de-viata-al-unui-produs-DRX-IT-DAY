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
    public class BomMaterialService : IBomMaterialService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly BomMaterialRepository _bomMaterialRepository;
        private readonly IRepository<Bom> _bomRepository;
        private readonly IRepository<Material> _materialRepository;


        public BomMaterialService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _bomMaterialRepository = repositoryFactory.CreateBomMaterialRepository();
            _bomRepository = repositoryFactory.CreateBomRepository();
            _materialRepository = repositoryFactory.CreateMaterialRepository();
        }

        public async Task<int> CreateBomMaterialAsync(BomMaterialDto bomMaterialDto)
        {

            await Validate(bomMaterialDto);

            var bomMaterial = BomMaterialMapper.FromDto(bomMaterialDto);
            return await _bomMaterialRepository.AddAsync(bomMaterial);
        }

        public async Task<bool> DeleteBomMaterialAsync(int boomId, int material_number)
        {
            return await _bomMaterialRepository.DeleteAsync(boomId, material_number);
        }

        public async Task<IEnumerable<BomMaterialDto>> GetAllBomMaterialsAsync()
        {
            var bomMaterials = await _bomMaterialRepository.GetAllAsync();
            List<Material> materials = new List<Material>();
            foreach (BomMaterial bomMaterial in bomMaterials)
            {
                materials.Add(await _materialRepository.GetByIdAsync(bomMaterial.material_number));
            }

            return BomMaterialMapper.ToDto(bomMaterials.ToList(), materials);
        }

        public async Task<BomMaterialDto> GetBomMaterialByIdAsync(int boomId, int material_number)
        {
            var material = await _materialRepository.GetByIdAsync(material_number);
            var bomMaterial = await _bomMaterialRepository.GetByIdAsync(boomId, material_number);
            if (material == null || bomMaterial == null)
            {
                return null;
            }
            return BomMaterialMapper.ToDto(bomMaterial, material);
        }

        public async Task<List<BomMaterialDto>> GetMaterialsByBomIdAsync(int boomId) 
        {
            var materials = await _bomMaterialRepository.GetMaterialsByBomIdAsync(boomId);
            var bomaterials= new List<BomMaterial>();
            foreach (Material material in materials)
            {
                bomaterials.Add(await _bomMaterialRepository.GetByIdAsync(boomId, material.material_number));
            }

            return BomMaterialMapper.ToDto(bomaterials, materials.ToList());
        }

        public async Task<IEnumerable<BomMaterialDto>> GetBomMaterialsWithPaginationAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number must be greater than 0");
            if (pageSize < 1) throw new ArgumentException("Page size must be greater than 0");

            var bomMaterials = await _bomMaterialRepository.GetWithPaginationAsync(pageNumber,pageSize);
            List<Material> materials = new List<Material>();
            foreach (BomMaterial bomMaterial in bomMaterials)
            {
                materials.Add(await _materialRepository.GetByIdAsync(bomMaterial.material_number));
            }

            return BomMaterialMapper.ToDto(bomMaterials.ToList(),materials);
        }

        public async Task<bool> UpdateBomMaterialAsync(BomMaterialDto bomMaterialDto)
        {
            if (bomMaterialDto == null)
                throw new ArgumentNullException(nameof(bomMaterialDto));

            await Validate(bomMaterialDto);

            var bomMaterial = BomMaterialMapper.FromDto(bomMaterialDto);
            return await _bomMaterialRepository.UpdateAsync(bomMaterial);
        }

        private async Task Validate(BomMaterialDto bomMaterial)
        {
            if (bomMaterial == null)
                throw new ArgumentNullException(nameof(bomMaterial));

            if (bomMaterial.Quantity <= 0)
                throw new ArgumentException("Bom Material Quantity cannot be less than 1");

            //if both bomId and material_number are exist in the db?
            if (!await _materialRepository.ExistsAsync(bomMaterial.Material.MaterialNumber))
                throw new ArgumentException("Material does not exist in the database");
            if(!await _bomRepository.ExistsAsync(bomMaterial.BomId))
                throw new ArgumentException("Bom does not exist in the database");
        }
    }
}
