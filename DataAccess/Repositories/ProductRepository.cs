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
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(IDbContext context) : base(context)
        {
        }

        protected override string TableName => "products";

        public override async Task<int> AddAsync(Product entity)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = $@"
        INSERT INTO {TableName} 
                     (
                      bom_id,
                      description,  
                      estimated_height,
                      estimated_width,  
                      estimated_weight,
                      name)
        OUTPUT INSERTED.id
        VALUES 
             (
              @bom_id,
              @description,  
              @estimated_height,
              @estimated_width,  
              @estimated_weight,
              @name);";

                return await connection.QuerySingleAsync<int>(sql, new
                {
                    entity.bom_id,
                    entity.description,
                    entity.estimated_height,
                    entity.estimated_width,
                    entity.estimated_weight,
                    entity.name
                });
            }
        }

        public override async Task<bool> UpdateAsync(Product entity)
        {
            using (var connection = _context.CreateConnection())
            { 
                string sql = $@"
                UPDATE {TableName}
                SET 
                    description = @description,  
                    estimated_height = @estimated_height,
                    estimated_width = @estimated_width,  
                    estimated_weight = @estimated_weight,
                    name = @name 
                    
                WHERE id = @id";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    entity.id,
                    entity.description,
                    entity.estimated_height,
                    entity.estimated_width,
                    entity.estimated_weight,
                    entity.name
                });

                return rowsAffected > 0;
            }

        }
        
    }
}
