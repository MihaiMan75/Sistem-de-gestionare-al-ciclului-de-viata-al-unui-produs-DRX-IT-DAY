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
    public class MaterialRepository : BaseRepository<Material>
    {
        public MaterialRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => "materials";

        public override async Task<int> AddAsync(Material entity)
        {
            using (var connection = _context.CreateConnection())
            { 
                 string sql = $@"
        INSERT INTO {TableName} 
            (
             material_description,  
             weight, 
             width,     
             height)
        OUTPUT INSERTED.material_number
        VALUES 
            (
             @material_description,
             @height,
             @width, 
             @weight);";

                return await connection.QuerySingleAsync<int>(sql, new
                {
                    
                    entity.material_description,
                    entity.height,
                    entity.width,
                    entity.weight
                });
            }
        }

        public override async Task<bool> UpdateAsync(Material entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
                UPDATE {TableName}
                SET 
                    height = @height,
                    width = @width,
                    material_description = @material_description,
                    weight = @weight
                WHERE material_number = @material_number";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    entity.material_number,
                    entity.material_description,
                    entity.height,
                    entity.width,
                    entity.weight
                });

                return rowsAffected > 0;
            }

        }

        public override async Task<Material> GetByIdAsync(int material_number)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Material>(
                    $"SELECT * FROM {TableName} WHERE material_number = @material_number",
                    new { material_number }
                );
            }
        }

        public override async Task<bool> DeleteAsync(int material_number)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    $"DELETE FROM {TableName} WHERE material_number = @material_number",
                    new { material_number}
                    ) > 0;
            }
        }

        public override async Task<IEnumerable<Material>> GetWithPaginationAsync(int pageNumber, int pageSize) 
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Material>(
                    $@"SELECT *
                      FROM {TableName}
                      ORDER BY material_number
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

        public override async Task<bool> ExistsAsync(int material_number)
        {
            using (var connection = _context.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(
                    $"SELECT COUNT(1) FROM {TableName} WHERE material_number = @material_number",
                    new { material_number}
                );
                return count > 0;
            }
        }
    }

}