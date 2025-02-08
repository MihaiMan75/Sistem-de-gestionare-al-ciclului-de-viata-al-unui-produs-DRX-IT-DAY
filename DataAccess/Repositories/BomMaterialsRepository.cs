using DataAccess.Dtos;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class BomMaterialsRepository : BaseRepository<BomMaterials>
    {
        public BomMaterialsRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => throw new NotImplementedException();

        public override Task<int> AddAsync(BomMaterials entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(BomMaterials entity)
        {
            throw new NotImplementedException();
        }
    }
}
