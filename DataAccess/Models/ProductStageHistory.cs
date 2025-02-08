using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos
{
    public class ProductStageHistory
    {
        public int prductId {  get; set; }
        public int stageId { get; set; }
        public DateTime startOfStage { get; set; }
        public DateTime endOfStage { get; set; }
        public int userId { get; set; }
    }
}
