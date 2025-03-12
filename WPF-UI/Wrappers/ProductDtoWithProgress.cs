using BusinessLogic.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_UI.Wrappers
{
    public class ProductDtoWithProgress : ProductDto
    {
        public int StageProgressValue { get; set; }
    }
}
