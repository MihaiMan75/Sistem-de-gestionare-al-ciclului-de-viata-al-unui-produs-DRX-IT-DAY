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
    public class UserRolesRepository : BaseRepository<UserRoles>
    {
        public UserRolesRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => "user_roles" ;

        public override async Task<int> AddAsync(UserRoles entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
        INSERT INTO {TableName} 
            (id_user, role_id)
        OUTPUT INSERTED.id_user
        VALUES 
            (@id_user, @role_id);";

                return await connection.QuerySingleAsync<int>(sql, new
                {
                    entity.id_user,
                    entity.role_id
                });
            }
        }

        public override async Task<bool> UpdateAsync(UserRoles entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
                UPDATE {TableName}
                SET 
                   role_id = @role_id
                WHERE id_user = @id_user AND role_id = @role_id";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    entity.id_user,
                    entity.role_id
                });

                return rowsAffected > 0;
            }
        }

        public async Task<UserRoles> GetByIdAsync(int id_user, int role_id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<UserRoles>(
                    $"SELECT * FROM {TableName} WHERE id_user = @id_user AND role_id=@role_id",
                    new { id_user,
                          role_id}
                );
            }
        }

        public async Task<bool> DeleteAsync(int id_user, int role_id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    $"DELETE FROM {TableName} WHERE id_user = @id_user AND role_id = @role_id",
                    new
                    {
                        id_user,
                        role_id
                    }
                    ) > 0;
            }
        }


        public override async Task<IEnumerable<UserRoles>> GetWithPaginationAsync(int pageNumber, int pageSize) 
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<UserRoles>(
                    $@"SELECT *
                      FROM {TableName}
                      ORDER BY id_user, role_id
                      OFFSET (@PageNumber - 1) * @PageSize ROWS
                      FETCH NEXT @PageSize ROWS ONLY",
                    new
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    }
                );
            }
        }
        public async Task<bool> ExistsAsync(int id_user, int role_id)
        {
            using (var connection = _context.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(
                    $"SELECT COUNT(1) FROM {TableName} WHERE id_user = @id_user AND role_id = @role_id",
                    new { id_user, 
                          role_id}
                );
                return count > 0;
            }
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(int id_user)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = @"
            SELECT r.*
            FROM roles r
            INNER JOIN user_roles ur ON r.id = ur.role_id
            WHERE ur.id_user = @id_user";

                var roles = await connection.QueryAsync<Role>(sql, new { id_user  });
                return roles;
            }
        }
    }
}
