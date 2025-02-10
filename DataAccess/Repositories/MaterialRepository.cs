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

        protected override string TableName => "Material";

        public override async Task<int> AddAsync(Material entity)
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = @"
        INSERT INTO Material 
            (material_number, materialDescription, height, width, weight)
        OUTPUT INSERTED.material_number
        VALUES 
            (@material_number, @materialDescription, @height, @width, @weight);";

                return await connection.QuerySingleAsync<int>(sql, new
                {
                    entity.material_number,
                    entity.materialDescription,
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
                const string sql = @"
                UPDATE Material 
                SET 
                    materialDescription = @materialDescription,
                    height = @height,
                    width = @width,
                    weight = @weight
                WHERE material_number = @material_number";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    entity.material_number,
                    entity.materialDescription,
                    entity.height,
                    entity.width,
                    entity.weight
                });

                return rowsAffected > 0;
            }

        }

        public override async Task<Material> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Material>(
                    $"SELECT * FROM {TableName} WHERE material_number = @Id",
                    new { Id = id }
                );
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    $"DELETE FROM {TableName} WHERE material_number = @Id",
                    new { Id = id }
                    ) > 0;
            }
        }
    }

}