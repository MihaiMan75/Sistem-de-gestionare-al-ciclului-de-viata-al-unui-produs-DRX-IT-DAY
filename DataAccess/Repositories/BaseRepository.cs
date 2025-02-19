using Dapper;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    
        public abstract class BaseRepository<T> : IRepository<T> where T : class
        {
            protected readonly IDbContext _context;
            protected abstract string TableName { get; }  // tabe name needs to be changed for each repository

            protected BaseRepository(IDbContext context) 
            {
                _context = context;
            }

            public abstract Task<int> AddAsync(T entity);
            public abstract Task<bool> UpdateAsync(T entity);

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

            public virtual async Task<IEnumerable<T>> GetAllAsync()
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<T>(
                        $"SELECT * FROM {TableName}"
                        );
                }
            }

            public virtual async Task<T> GetByIdAsync(int id)
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<T>(
                        $"SELECT * FROM {TableName} WHERE id = @id",
                        new { id }
                    );
                }
            }

            public virtual async Task<IEnumerable<T>> GetWithPaginationAsync(int pageNumber, int pageSize) // might be necesary to extract also the nr of records in the table.
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<T>(
                        $@"SELECT *
                      FROM {TableName}
                      ORDER BY id
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
            public virtual async Task<bool> ExistsAsync(int id)
            {
                using (var connection = _context.CreateConnection())
                {
                    var count = await connection.ExecuteScalarAsync<int>(
                        $"SELECT COUNT(1) FROM {TableName} WHERE id = @id",
                        new { id }
                    );
                    return count > 0;
                }
            }
    }

    
}
