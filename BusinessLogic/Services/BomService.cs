using DataAccess.Interfaces;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using BusinessLogic.Interfaces;
using BusinessLogic.DtoModels;
using BusinessLogic.Mappers;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class BomService: IBomService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IRepository<Bom> _bomRepository;
        private readonly IBomMaterialService _bomMaterialService;

        public BomService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _bomRepository = repositoryFactory.CreateBomRepository();
            _bomMaterialService = new BomMaterialService(repositoryFactory);
        }

        public async Task<int> CreateBomAsync(BomDto bomDto)
        {
            await Validate(bomDto);
            

            var bom = BomMapper.FromDto(bomDto);
            bom.id = await _bomRepository.AddAsync(bom);
            //list of bomMaterials -> create new BomMaterials
            foreach (var bomMaterial in bomDto.BomMaterials)
            {
                bomMaterial.BomId = bom.id;
                await _bomMaterialService.CreateBomMaterialAsync(bomMaterial); // need to create the links between bom and materials
            }
            return bom.id;
        }

        public async Task<bool> DeleteBomAsync(int id)
        {
            return await _bomRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<BomDto>> GetAllBomsAsync()
        {
            var Boms = await _bomRepository.GetAllAsync();
            List<BomDto> result = new List<BomDto>();
            foreach (var bom in Boms)
            {
                var BomMaterials = await _bomMaterialService.GetMaterialsByBomIdAsync(bom.id);
                result.Add(BomMapper.ToDto(bom, BomMaterials));
            }
            return result;
        }

        public async Task<BomDto> GetBomByIdAsync(int id)
        {
           var Bom = await _bomRepository.GetByIdAsync(id);
           var BomMaterials = await _bomMaterialService.GetMaterialsByBomIdAsync(id);
            if (Bom == null || BomMaterials == null)
            {
                return null;
            }
            return BomMapper.ToDto(Bom, BomMaterials);

        }

        public async Task<IEnumerable<BomDto>> GetBomsWithPaginationAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number must be greater than 0");
            if (pageSize < 1) throw new ArgumentException("Page size must be greater than 0");

            var boms = await _bomRepository.GetWithPaginationAsync(pageNumber, pageSize);
            List<BomDto> result = new List<BomDto>();

            foreach (var bom in boms)
            {
                var bomMaterials = await _bomMaterialService.GetMaterialsByBomIdAsync(bom.id);
                result.Add(BomMapper.ToDto(bom, bomMaterials));
            }

            return result;
        }

        public async Task<bool> UpdateBomAsync(BomDto bomDto)
        {
            if (bomDto == null)
                throw new ArgumentNullException(nameof(bomDto));

            await Validate(bomDto);

            //update aslo user roles
            foreach (BomMaterialDto bomMaterial in bomDto.BomMaterials)
            {
                await _bomMaterialService.UpdateBomMaterialAsync(bomMaterial);
            }

            var bom = BomMapper.FromDto(bomDto);
            return await _bomRepository.UpdateAsync(bom);
        }

        private async Task Validate(BomDto bomDto)
        {
            if (bomDto == null)
                throw new ArgumentNullException(nameof(bomDto));

            if (String.IsNullOrEmpty(bomDto.Name))
                throw new Exception("Bom must have a Name");

            if (bomDto.BomMaterials.Count == 0)
                throw new Exception("BOm must have at least one BomMaterial");
        }
    }
}
