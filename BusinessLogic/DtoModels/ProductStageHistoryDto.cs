using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DtoModels
{
    public partial class ProductStageHistoryDto 
    {
        public StageDto ProductStage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public UserDto User { get; set; }

        //public string Duration
        //{
        //    get
        //    {
        //        DateTime endPoint = EndDate ?? DateTime.Now;
        //        TimeSpan duration = endPoint - StartDate;
        //        return EndDate > DateTime.Now ? FormatFutureDuration(duration) : FormatDuration(duration, EndDate.HasValue);
        //    }
        //}

        //private string CalculateDuration()
        //{
        //    DateTime endPoint = EndDate ?? DateTime.Now;
        //    TimeSpan duration = endPoint - StartDate;

        //    if (EndDate > DateTime.Now)
        //        return FormatFutureDuration(duration);

        //    return FormatDuration(duration, EndDate.HasValue);
        //}

        //private string FormatFutureDuration(TimeSpan futureDuration)
        //{
        //    var components = new List<string>();

        //    if (futureDuration.Days / 365 > 0)
        //        components.Add($"{futureDuration.Days / 365} year{(futureDuration.Days / 365 != 1 ? "s" : "")}");

        //    if ((futureDuration.Days % 365) / 30 > 0)
        //        components.Add($"{(futureDuration.Days % 365) / 30} month{((futureDuration.Days % 365) / 30 != 1 ? "s" : "")}");

        //    if (futureDuration.Days % 30 > 0)
        //        components.Add($"{futureDuration.Days % 30} day{(futureDuration.Days % 30 != 1 ? "s" : "")}");

        //    if (futureDuration.Hours > 0)
        //        components.Add($"{futureDuration.Hours} hour{(futureDuration.Hours != 1 ? "s" : "")}");

        //    return $"Due in {(components.Count > 0 ? string.Join(", ", components) : "Less than a day")}";
        //}

        //private string FormatDuration(TimeSpan duration, bool isCompleted)
        //{
        //    var components = new List<string>();

        //    if (duration.Days > 0)
        //        components.Add($"{duration.Days} day{(duration.Days != 1 ? "s" : "")}");

        //    if (duration.Hours > 0)
        //        components.Add($"{duration.Hours} hour{(duration.Hours != 1 ? "s" : "")}");

        //    if (duration.Minutes > 0)
        //        components.Add($"{duration.Minutes} minute{(duration.Minutes != 1 ? "s" : "")}");

        //    if (duration.TotalHours < 1 && duration.Seconds > 0)
        //        components.Add($"{duration.Seconds} second{(duration.Seconds != 1 ? "s" : "")}");

        //    return isCompleted
        //        ? (components.Count > 0 ? string.Join(", ", components) : "Active")
        //        : $"{(components.Count > 0 ? string.Join(", ", components) : "Less than a second")} (Active)";
        //}
    }

}
