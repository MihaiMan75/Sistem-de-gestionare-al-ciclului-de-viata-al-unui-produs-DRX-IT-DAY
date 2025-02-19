using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class ProductStageHistory
    {
        public int product_id{  get; set; }
        public int stage_id { get; set; }
        public DateTime start_of_stage { get; set; }
        public DateTime end_of_stage { get; set; }
        public int id_user { get; set; }
    }
}
