using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Material
    {
        public int material_number {  get; set; }
        public string material_description { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public double weight { get; set; }
    }
}
