using DataAccess.Models;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => throw new NotImplementedException();

        public override Task<int> AddAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}
