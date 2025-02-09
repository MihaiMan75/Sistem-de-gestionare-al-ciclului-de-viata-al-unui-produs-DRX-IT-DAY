using DataAccess.Models;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class MaterialRepository : BaseRepository<Material>
    {
        public MaterialRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => throw new NotImplementedException();

        public override Task<int> AddAsync(Material entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(Material entity)
        {
            throw new NotImplementedException();
        }
    }
}
