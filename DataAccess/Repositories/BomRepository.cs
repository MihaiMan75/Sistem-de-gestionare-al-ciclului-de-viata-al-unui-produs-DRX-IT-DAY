using DataAccess.Models;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class BomRepository : BaseRepository<Bom>
    {
        public BomRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => throw new NotImplementedException();

        public override Task<int> AddAsync(Bom entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(Bom entity)
        {
            throw new NotImplementedException();
        }
    }
}
