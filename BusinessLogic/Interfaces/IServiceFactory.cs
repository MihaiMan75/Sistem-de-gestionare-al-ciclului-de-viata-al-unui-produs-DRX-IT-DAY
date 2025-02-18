using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IServiceFactory
    {
        IBomMaterialService GetBomMaterialService();
        IBomService GetBomService();
        IMaterialsService GetMaterialsService();
        IProductService GetProductService();
        IProductStageHistoryService GetProductStageHistoryService();
        IRoleService GetRoleService();
        IStageService GetStageService();
        IUserService GetUserService();

    }
}
