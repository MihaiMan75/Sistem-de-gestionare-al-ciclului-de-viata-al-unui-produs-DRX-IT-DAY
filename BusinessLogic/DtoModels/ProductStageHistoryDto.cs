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
                DateTime endPoint = EndDate ?? DateTime.Now;
                TimeSpan duration = endPoint - StartDate;

                // Future date (expected)
                if (EndDate > DateTime.Now)
                {
                    return FormatFutureDuration(duration);
                }

                return FormatDuration(duration, EndDate.HasValue);
            }
        }

        private string FormatFutureDuration(TimeSpan futureDuration)
        {
            
            var components = new List<string>();

            
            if (futureDuration.Days / 365 > 0)
            {
                int years = futureDuration.Days / 365;
                components.Add($"{years} year{(years != 1 ? "s" : "")}");
            }

            
            if ((futureDuration.Days % 365) / 30 > 0)
            {
                int months = (futureDuration.Days % 365) / 30;
                components.Add($"{months} month{(months != 1 ? "s" : "")}");
            }

            
            if (futureDuration.Days % 30 > 0)
            {
                int days = futureDuration.Days % 30;
                components.Add($"{days} day{(days != 1 ? "s" : "")}");
            }

            
            if (futureDuration.Hours > 0)
                components.Add($"{futureDuration.Hours} hour{(futureDuration.Hours != 1 ? "s" : "")}");

            
            string futureDurationString = components.Count > 0
                ? string.Join(", ", components)
                : "Less than a day";

            return $"Due in {futureDurationString}";
        }

        private string FormatDuration(TimeSpan duration, bool isCompleted)
        {
            
            var components = new List<string>();

           
            if (duration.Days > 0)
                components.Add($"{duration.Days} day{(duration.Days != 1 ? "s" : "")}");

            
            if (duration.Hours > 0)
                components.Add($"{duration.Hours} hour{(duration.Hours != 1 ? "s" : "")}");

           
            if (duration.Minutes > 0)
                components.Add($"{duration.Minutes} minute{(duration.Minutes != 1 ? "s" : "")}");

            
            if (duration.TotalHours < 1 && duration.Seconds > 0)
                components.Add($"{duration.Seconds} second{(duration.Seconds != 1 ? "s" : "")}");

            
            string durationString = components.Count > 0
                ? string.Join(", ", components)
                : "Less than a second";

            
            return isCompleted
                ? durationString
                : $"{durationString} (Active)";
        }
    
    }
    
}
