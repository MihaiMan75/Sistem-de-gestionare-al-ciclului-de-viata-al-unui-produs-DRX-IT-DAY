using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DtoModels
{
    public class ProductStageHistoryDto
    {
        public StageDto ProductStage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public UserDto User { get; set; }

        public string Duration
        {
            get
            {
                if (StartDate != DateTime.MinValue)
                {
                    DateTime endPoint;
                    if (EndDate != StartDate)
                    {
                        endPoint = (DateTime)EndDate;
                        TimeSpan duration = endPoint - StartDate;
                        return $"{duration.Days} days, {duration.Hours} hours, {duration.Minutes} minutes";
                    }
                    else
                    {
                        endPoint = DateTime.Now;
                        TimeSpan duration = endPoint - StartDate;
                        return $"{duration.Days} days, {duration.Hours} hours, {duration.Minutes} minutes (Active)";
                    }

                }
                return string.Empty;
            }
        }
    }
    
}
