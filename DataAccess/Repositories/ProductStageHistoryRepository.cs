using DataAccess.Dtos;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductStageHistoryRepository : BaseRepository<ProductStageHistory>
    {
        public ProductStageHistoryRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => throw new NotImplementedException();

        public override Task<int> AddAsync(ProductStageHistory entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(ProductStageHistory entity)
        {
            throw new NotImplementedException();
        }
    }
}
