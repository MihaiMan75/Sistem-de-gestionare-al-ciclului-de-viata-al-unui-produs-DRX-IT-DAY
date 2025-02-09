using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DtoModels
{
    public class BomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BomMaterialDto BomMaterial { get; set; }
    }
}
