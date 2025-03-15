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
        //progress bar color
        public string StageProgressColor
        {
            get
            {
                if (StageProgressValue < 5)
                {
                    return "Green";
                }
                else if (StageProgressValue == 5)
                {
                    return "Yellow";
                }
                else if (StageProgressValue == 6)
                {
                    return "Orange";
                }
                else if (StageProgressValue == 7)
                {
                    return "Red";
                }
                else
                {
                    return "Gray";
                }
            }
        }
    }
}
