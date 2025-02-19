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
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => "users";

        public override async Task<int> AddAsync(User entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
        INSERT INTO {TableName} 
            (email,
             name,  
             PasswordHashed,
             phone_number)
        OUTPUT INSERTED id
        VALUES 
            (@email, 
             @name,
             @PasswordHashed,
             @phone_number);";

                return await connection.QuerySingleAsync<int>(sql, new
                {
                    entity.email,
                    entity.name,
                    entity.PasswordHashed,
                    entity.phone_number

                });
            }
        }

        public override async Task<bool> UpdateAsync(User entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
                UPDATE {TableName}
                SET
                    email = @email,
                    name = @name, 
                    PasswordHashed = @PasswordHashed,
                    phone_number = @phone_number
                WHERE id_user = @id_user";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    entity.id_user,
                    entity.email,
                    entity.name,
                    entity.PasswordHashed,
                    entity.phone_number
                });

                return rowsAffected > 0;
            }
        }

        public override async Task<bool> DeleteAsync(int id_user)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    $"DELETE FROM {TableName} WHERE id_user = @id_user",
                    new { id_user }
                    ) > 0;
            }
        }

        public override async Task<User> GetByIdAsync(int id_user)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<User>(
                    $"SELECT * FROM {TableName} WHERE id_user = @id_user",
                    new { id_user }
                );
            }
        }

        public override async Task<IEnumerable<User>> GetWithPaginationAsync(int pageNumber, int pageSize) // might be necesary to extract also the nr of records in the table.
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<User>(
                    $@"SELECT *
                      FROM {TableName}
                      ORDER BY id_user
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
        public override async Task<bool> ExistsAsync(int id_user)
        {
            using (var connection = _context.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(
                    $"SELECT COUNT(1) FROM {TableName} WHERE id_user = @id_user",
                    new { id_user }
                );
                return count > 0;
            }
        }
    }
}
