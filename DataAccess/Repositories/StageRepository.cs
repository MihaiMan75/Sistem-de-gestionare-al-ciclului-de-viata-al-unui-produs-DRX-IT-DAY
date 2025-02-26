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
    public class StageRepository : BaseRepository<Stage>
    {
        public StageRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => "stages";

        public override async Task<int> AddAsync(Stage entity)
        {

            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
        INSERT INTO {TableName} 
            (id,
             name,
             description)
        OUTPUT INSERTED.id
        VALUES 
            (@id,
             @name, 
             @description);";

                return await connection.QuerySingleAsync<int>(sql, new
                {
                    entity.id,
                    entity.name,
                    entity.description
                });
            }
        }

        public override async Task<bool> UpdateAsync(Stage entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
                UPDATE {TableName}
                SET
                    name = @name, 
                    description = @description                    
                WHERE id = @id";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    entity.id, 
                    entity.name,               
                    entity.description
                });

                return rowsAffected > 0;
            }
        }
    }
}
