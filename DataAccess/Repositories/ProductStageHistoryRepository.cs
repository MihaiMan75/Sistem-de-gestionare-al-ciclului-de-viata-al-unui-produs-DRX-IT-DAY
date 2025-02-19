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
    public class ProductStageHistoryRepository : BaseRepository<ProductStageHistory>
    {
        public ProductStageHistoryRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => "product_stage_history";

        public override async Task<int> AddAsync(ProductStageHistory entity)
        {
            using (var connection = _context.CreateConnection())
            { 
                string sql = $@"
        INSERT INTO {TableName} 
            (stage_id,
             product_id, 
             start_of_stage, 
             id_user, 
             end_of_stage)
        OUTPUT INSERTED.stage_id
        VALUES 
            (@stage_id,
             @product_id, 
             @start_of_stage, 
             @id_user, 
             @end_of_stage);";


                return await connection.QuerySingleAsync<int>(sql, new
                {
                    entity.stage_id,
                    entity.product_id,
                    entity.start_of_stage,
                    entity.id_user,
                    entity.end_of_stage,
                    
                });
            }
        }

        public override async Task<bool> UpdateAsync(ProductStageHistory entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"UPDATE {TableName}
                SET 
                   id_user = @id_user,
                   end_of_stage = @end_of_stage,
                   start_of_stage = @start_of_stage
                WHERE stage_id = @stage_id 
                AND  product_id = @product_id";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    entity.stage_id,
                    entity.product_id,
                    entity.start_of_stage,
                    entity.id_user,
                    entity.end_of_stage
                });

                return rowsAffected > 0;
            }
        }
        public async Task<BomMaterial> GetByIdAsync(int stage_id, int product_id, DateTime start_of_stage)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<BomMaterial>(
                    $"SELECT * FROM {TableName}" +
                    $" WHERE stage_id = @stage_id AND product_id = @product_id",
                    new
                    {
                        stage_id,
                        product_id,
                        start_of_stage
                    }
                );
            }
        }

        public async Task<bool> DeleteAsync(int stage_id, int product_id, DateTime start_of_stage)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    $"DELETE FROM {TableName} " +
                    $" WHERE stage_id = @stage_id" +
                    $" AND product_id = @product_id ",
                    new
                    {
                        stage_id,
                        product_id,
                    }
                    ) > 0;
            }
        }

        public override async Task<IEnumerable<ProductStageHistory>> GetWithPaginationAsync(int pageNumber, int pageSize) 
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<ProductStageHistory>(
                    $@"SELECT *
                      FROM {TableName}
                      ORDER BY stage_id, product_id
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
        public  async Task<bool> ExistsAsync(int stage_id, int product_id, DateTime start_of_stage)
        {
            using (var connection = _context.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(
                    $"SELECT COUNT(1) FROM {TableName}  WHERE stage_id = @stage_id" +
                    $" AND product_id = @product_id ",
                    new { stage_id, product_id, start_of_stage }
                );
                return count > 0;
            }
        }
        public async Task<IEnumerable<ProductStageHistory>> GetProductStageHistoriesByProductIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = @"
            SELECT psh.product_id, 
                   psh.stage_id, 
                   psh.start_of_stage, 
                   psh.end_of_stage, 
                   psh.id_user
            FROM product_stage_history psh
            INNER JOIN stages s ON psh.stage_id = s.id
            WHERE psh.product_id = @id
            ORDER BY psh.start_of_stage DESC ";

                var productHistory = await connection.QueryAsync<ProductStageHistory>( query, new { id } );
                return productHistory;
               
            }
        }
       
    }
}

