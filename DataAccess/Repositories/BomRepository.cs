using DataAccess.Models;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataAccess.Repositories
{
    public class BomRepository : BaseRepository<Bom>
    {
        public BomRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => "bom";

        public override async Task<int> AddAsync(Bom entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
        INSERT INTO {TableName} 
            (name)
        OUTPUT INSERTED.[id]
        VALUES 
            (@name);";

                return await connection.QuerySingleAsync<int>(sql, new
                {
                    entity.name
                });
            }
        }

        public override async Task<bool> UpdateAsync(Bom entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
                UPDATE {TableName}
                SET 
                   name = @name
                WHERE id = @id";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    entity.id,
                    entity.name
                });

                return rowsAffected > 0;
            }
        }
       
    }
}
