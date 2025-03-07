using BusinessLogic.DtoModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_UI.Wrappers
{
    public class ProductStageHistoryViewModel : ObservableObject
    {
        private ProductStageHistoryDto _model;

        public ProductStageHistoryViewModel(ProductStageHistoryDto model)
        {
            _model = model;
        }

        public StageDto ProductStage
        {
            get => _model.ProductStage;
            set
            {
                _model.ProductStage = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _model.StartDate;
            set
            {
                _model.StartDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
            }
        }

        public DateTime? EndDate
        {
            get => _model.EndDate;
            set
            {
                _model.EndDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
            }
        }

        public UserDto User
        {
            get => _model.User;
            set
            {
                _model.User = value;
                OnPropertyChanged();
            }
        }

        public string Duration => CalculateDuration();

        // if the dto is neeeded
        public ProductStageHistoryDto GetDto() => _model;

        private string CalculateDuration()
        {
            DateTime endPoint = EndDate ?? DateTime.Now;
            TimeSpan duration = endPoint - StartDate;
            if (EndDate > DateTime.Now)
                return FormatFutureDuration(duration);
            return FormatDuration(duration, EndDate.HasValue);
        }

        private string FormatFutureDuration(TimeSpan futureDuration)
        {
            var components = new List<string>();
            if (futureDuration.Days / 365 > 0)
                components.Add($"{futureDuration.Days / 365} year{(futureDuration.Days / 365 != 1 ? "s" : "")}");
            if ((futureDuration.Days % 365) / 30 > 0)
                components.Add($"{(futureDuration.Days % 365) / 30} month{((futureDuration.Days % 365) / 30 != 1 ? "s" : "")}");
            if (futureDuration.Days % 30 > 0)
                components.Add($"{futureDuration.Days % 30} day{(futureDuration.Days % 30 != 1 ? "s" : "")}");
            if (futureDuration.Hours > 0)
                components.Add($"{futureDuration.Hours} hour{(futureDuration.Hours != 1 ? "s" : "")}");
            return $"Due in {(components.Count > 0 ? string.Join(", ", components) : "Less than a day")}";
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
            return isCompleted
                ? (components.Count > 0 ? string.Join(", ", components) : "Active")
                : $"{(components.Count > 0 ? string.Join(", ", components) : "Less than a second")} (Active)";
        }
    }
}
