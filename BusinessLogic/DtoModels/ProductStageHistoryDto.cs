using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class ProductStageHistoryDto
    {
        public StageDto ProductStage { get; set; }
         public DateTime StartDate { get; set; }
         public DateTime EndDate { get; set; }
    }
}
