using DataAccess.Interfaces;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RepositoryFactory
    {
        // RepositoryFactory.cs

        private readonly IDbContext _dbContext;

        // We can use a dictionary to cache repositories if needed

        public RepositoryFactory(IDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public BomMaterialRepository CreateBomMaterialRepository()
        {
            return new BomMaterialRepository(_dbContext);
        }

        public BomRepository CreateBomRepository()
        {
            return new BomRepository(_dbContext);
        }

        public MaterialRepository CreateMaterialRepository()
        {
            return new MaterialRepository(_dbContext);
        }

        public ProductRepository CreateProductRepository()
        {
            return new ProductRepository(_dbContext);
        }

        public ProductStageHistoryRepository CreateProductStageHistoryRepository()
        {
            return new ProductStageHistoryRepository(_dbContext);
        }

        public RoleRepository CreateRoleRepository()
        {
            return new RoleRepository(_dbContext);
        }

        public StageRepository CreateStageRepository()
        {
            return new StageRepository(_dbContext);
        }

        public UserRepository CreateUserRepository()
        {
            return new UserRepository(_dbContext);
        }

        public UserRolesRepository CreateUserRolesRepository()
        {
            return new UserRolesRepository(_dbContext);
        }
    }    
}
