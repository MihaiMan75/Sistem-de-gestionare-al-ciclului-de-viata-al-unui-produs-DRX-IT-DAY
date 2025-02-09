using DataAccess.Models;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class StageRepository : BaseRepository<Stage>
    {
        public StageRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => throw new NotImplementedException();

        public override Task<int> AddAsync(Stage entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(Stage entity)
        {
            throw new NotImplementedException();
        }
    }
}
