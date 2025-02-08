using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos
{
    public class Product
    {
        public int id { get; set; }
        public int bom_id {  get; set; }
        public string description { get; set; }
        public double estimated_height { get; set; }
        public double estimated_width { get; set; }
        public double estimated_weight { get; set; }
        public string name { get; set; }
    }
}
