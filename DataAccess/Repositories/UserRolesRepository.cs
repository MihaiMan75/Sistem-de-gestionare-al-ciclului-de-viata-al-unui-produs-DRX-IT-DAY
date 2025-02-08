using DataAccess.Dtos;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRolesRepository : BaseRepository<UserRoles>
    {
        public UserRolesRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => throw new NotImplementedException();

        public override Task<int> AddAsync(UserRoles entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(UserRoles entity)
        {
            throw new NotImplementedException();
        }
    }
}
