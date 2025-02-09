using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DtoModels
{
    public class MaterialDto
    {
        public int MaterialNumber {  get; set; }
        public string MaterialDescription {  get; set; }
        public double Weight {  get; set; }
        public double Height {  get; set; }
        public double Width { get; set; }
    }
}
