using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IDbContext _context;
        private readonly IRepositoryFactory _repositoryFactory;

        public ServiceFactory(IDbContext context)
        {
            _context = context;
            _repositoryFactory = new RepositoryFactory(_context);
        }
        public IBomMaterialService GetBomMaterialService()
        {
            return new BomMaterialService(_repositoryFactory);
        }

        public IBomService GetBomService()
        {
            return new BomService(_repositoryFactory);
        }

        public IMaterialsService GetMaterialsService()
        {
            return new MaterialsService(_repositoryFactory);
        }

        public IProductService GetProductService()
        {
            return new ProductService(_repositoryFactory);
        }

        public IProductStageHistoryService GetProductStageHistoryService()
        {
            return new ProductStageHistoryService(_repositoryFactory);
        }

        public IRoleService GetRoleService()
        {
            return new RoleService(_repositoryFactory);
        }

        public IStageService GetStageService()
        {
            return new StageService(_repositoryFactory);
        }

        public IUserService GetUserService()
        {
            return new UserService(_repositoryFactory);
        }
    }
}
