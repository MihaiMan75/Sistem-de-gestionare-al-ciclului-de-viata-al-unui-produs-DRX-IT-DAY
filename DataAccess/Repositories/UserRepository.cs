using DataAccess.Models;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => throw new NotImplementedException();

        public override Task<int> AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
