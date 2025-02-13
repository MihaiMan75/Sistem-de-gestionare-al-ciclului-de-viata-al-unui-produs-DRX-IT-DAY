using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IRepositoryFactory
    {
        MaterialRepository CreateMaterialRepository();
        BomMaterialRepository CreateBomMaterialRepository();
        BomRepository CreateBomRepository();
        ProductRepository CreateProductRepository();
        ProductStageHistoryRepository CreateProductStageHistoryRepository();
        RoleRepository CreateRoleRepository();
        StageRepository CreateStageRepository();
        UserRepository CreateUserRepository();
        UserRolesRepository CreateUserRolesRepository();
    }
}
