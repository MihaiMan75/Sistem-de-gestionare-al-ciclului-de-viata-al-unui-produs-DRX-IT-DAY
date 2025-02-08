using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class Material
    {
        public int MaterialNumber {  get; set; }
        public string MaterialDescription {  get; set; }
        public float Weight {  get; set; }
        public float Height {  get; set; }
    }
}
