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
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => "roles";

        public override async Task<int> AddAsync(Role entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
        INSERT INTO {TableName} 
            (id, role_name)
        OUTPUT INSERTED.id
        VALUES 
            (@id, @role_name);";

                return await connection.QuerySingleAsync<int>(sql, new
                {   entity.id,
                    entity.role_name
                });
            }
        }

        public override async Task<bool> UpdateAsync(Role entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
                UPDATE {TableName}
                SET 
                   role_name = @role_name
                WHERE id = @id";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    entity.id,
                    entity.role_name
                });

                return rowsAffected > 0;
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    $"DELETE FROM {TableName} WHERE id = @id",
                    new { id }
                    ) > 0;
            }
        }

    }
}
