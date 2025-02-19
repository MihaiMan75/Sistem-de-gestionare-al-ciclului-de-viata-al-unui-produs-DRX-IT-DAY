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
    public class BomMaterialRepository : BaseRepository<BomMaterial>
    {
        public BomMaterialRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => "bom_materials";

        public override async Task<int> AddAsync(BomMaterial entity)
        {
            using (var connection = _context.CreateConnection())
            { 
                string sql = $@"
        INSERT INTO {TableName} 
            (material_number,
             bom_id,
             qty,
             unit_measure_code)
        OUTPUT INSERTED.material_number 
        VALUES 
            (@material_number,
             @bom_id,  
             @qty,
             @unit_measure_code);";


                return await connection.QuerySingleAsync<int>(sql, new
                {
                    entity.bom_id,
                    entity.material_number,
                    entity.qty,
                    entity.unit_measure_code,
                });
            }
        }

        public override async Task<bool> UpdateAsync(BomMaterial entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"UPDATE {TableName}
                SET
                    material_number = @material_number,
                    qty = @qty,
                    unit_measure_code = @unit_measure_code
                WHERE material_number = @material_number AND bom_id = @bom_id";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    entity.bom_id,
                    entity.material_number,
                    entity.qty,
                    entity.unit_measure_code,
                });
                return rowsAffected > 0;
            }
        }

        public async Task<BomMaterial> GetByIdAsync(int bom_id, int material_number )
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<BomMaterial>(
                    $"SELECT * FROM {TableName}" +
                    $" WHERE bom_id = @bom_id AND material_number = @material_number",
                    new { bom_id,
                          material_number  }
                );
            }
        }
        public  async Task<bool> DeleteAsync(int bom_id, int material_number)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    $"DELETE FROM {TableName} " +
                    $"WHERE  bom_id = @bom_id AND material_number = @material_number ",
                    new { bom_id, 
                          material_number }
                    ) > 0;
            }
        }

        public override async Task<IEnumerable<BomMaterial>> GetWithPaginationAsync(int pageNumber, int pageSize) 
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<BomMaterial>(
                    $@"SELECT *
                      FROM {TableName}
                      ORDER BY material_number, bom_id
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

        public async Task<bool> ExistsAsync(int material_number, int bom_id)
        {
            using (var connection = _context.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(
                    $"SELECT COUNT(1) FROM {TableName} WHERE material_number = @material_number AND bom_id = @bom_id",
                    new { material_number,
                          bom_id}
                );
                return count > 0;
            }
        }

        public async Task<IEnumerable<Material>> GetMaterialsByBomIdAsync(int bom_id)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = @"
            SELECT m.*
            FROM materials m
            INNER JOIN bom_material b ON m.material_number = b.material_number
            WHERE b.bom_id = @bom_id";

                var materials = await connection.QueryAsync<Material>(sql, new { bom_id });
                return materials;
            }
        }
    }
}