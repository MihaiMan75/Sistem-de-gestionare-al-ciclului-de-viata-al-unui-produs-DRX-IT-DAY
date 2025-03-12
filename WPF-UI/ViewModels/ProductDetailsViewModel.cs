using BusinessLogic;
using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_UI.Interfaces;
using WPF_UI.Messages;
using WPF_UI.Wrappers;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Windows.Controls;

namespace WPF_UI.ViewModels
{
    public partial class ProductDetailsViewModel:BaseViewModel, IRecipient<ProductSelectedMessage>
    {
        private readonly IProductService _productService;
        private readonly IStageService _stageService;
        private readonly IAuthService _authService;
        private readonly IProductStageHistoryService _productStageHistoryService;
        private readonly INavigationService _navigationService;

        private List<StageDto> stages = new List<StageDto>();


        [ObservableProperty]
        private ProductDto _currentProduct;

        [ObservableProperty]
        private StageDto _currentStage;

        [ObservableProperty]
        private ProductStageHistoryViewModel _currentStageHistory;

        [ObservableProperty]
        private ObservableCollection<BomMaterialDto> _bOMMaterials;

        [ObservableProperty]
        private ObservableCollection<ProductStageHistoryViewModel> _stageHistory;

        [ObservableProperty]
        private string _nextStage = "null";

        [ObservableProperty]
        private Visibility _buttonVisibility = Visibility.Visible;

        [ObservableProperty]
        private DateTime _endDate;

        [ObservableProperty]
        private IEnumerable<ISeries> _materialsSeries;

        [ObservableProperty]
        private LabelVisual _materialPieTitle;

        [ObservableProperty]
        private IEnumerable<ISeries> _stageSeries;

        [ObservableProperty]
        private IEnumerable<Axis> _xAxes;

        [ObservableProperty]
        private IEnumerable<Axis> _yAxes;


        private readonly IServiceFactory _serviceFactory;
        public ProductDetailsViewModel(IServiceFactory serviceFactory, IAuthService authService, INavigationService navigationService)
        {
            _serviceFactory = serviceFactory;
            _productService = _serviceFactory.GetProductService();
            _stageService = _serviceFactory.GetStageService();
            _productStageHistoryService = _serviceFactory.GetProductStageHistoryService();
            _authService = authService;
            BOMMaterials = new ObservableCollection<BomMaterialDto>();
            StageHistory = new ObservableCollection<ProductStageHistoryViewModel>();
            // LoadStagesCommand.Execute(null);
            WeakReferenceMessenger.Default.Register<ProductSelectedMessage>(this);
            _navigationService = navigationService;

            SetupPieChart();
            SetupStageHistoryChart();

        }

        private void SetupPieChart()
        {
            if (BOMMaterials != null && BOMMaterials.Count > 0)
            {
                MaterialsSeries = BOMMaterials
                    .GroupBy(m => m.Material.MaterialDescription)
                    .Select(g => new LiveChartsCore.SkiaSharpView.PieSeries<double>
                    {
                        Values = new[] { g.Sum(m => m.Quantity) },
                        Name = $"{g.Key}",
                        //InnerRadius = 50
                    })
                    .ToArray();

                MaterialPieTitle = new LabelVisual
                {
                    Text = "Materials Distribution",
                    TextSize = 20,
                    Padding = new LiveChartsCore.Drawing.Padding(15)
                };
            }
            else
            {
                // Fallback 
                MaterialsSeries = new[] { 2, 4, 1, 4, 3 }.AsPieSeries();
                MaterialPieTitle = new LabelVisual
                {
                    Text = "Sample Chart",
                    TextSize = 20,
                    Padding = new LiveChartsCore.Drawing.Padding(15)
                };
            }
        }

        private void SetupStageHistoryChart()
        {
            if (StageHistory != null && StageHistory.Count > 0)
            {
                // Sort history just in case
                var sortedHistory = StageHistory.OrderBy(sh => sh.StartDate).ToList();

                // Calculate duration 
                var durationValues = sortedHistory.Select(sh => {
                    DateTime endPoint = sh.EndDate ?? DateTime.Now;
                    TimeSpan duration = endPoint - sh.StartDate;
                    return duration.TotalDays; // Keep as double for the chart
                }).ToArray();

                // Format 
                var durationLabels = sortedHistory.Select(sh => {
                    DateTime endPoint = sh.EndDate ?? DateTime.Now;
                    TimeSpan duration = endPoint - sh.StartDate;

                    int days = (int)duration.TotalDays;
                    int hours = duration.Hours;
                    int minutes = duration.Minutes;
                    int seconds = duration.Seconds;

                    bool isExpected = sh.EndDate.HasValue && sh.EndDate > DateTime.Now;
                    

                    string formattedDuration;
                    if (days > 0)
                        formattedDuration = $"{days}d{hours}h";
                    else if (hours > 0)
                        formattedDuration = $"{hours}h{minutes}m";
                    else if (seconds > 0)
                        formattedDuration = $"{minutes}m{seconds}s"; // Show seconds only for short durations
                    else
                        formattedDuration = $"Active";

                    return isExpected ? $"{formattedDuration} expected" : formattedDuration;
                }).ToArray();

                // Left column
                var stageDurationSeries = new ColumnSeries<double>
                {
                    Values = durationValues,
                    Name = "Stage Duration",
                    Fill = new SolidColorPaint(SKColors.RoyalBlue),
                    Stroke = null,
                    MaxBarWidth = 45,
                    DataLabelsSize = 18,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                    DataLabelsFormatter = point => durationLabels[point.Index]
                };
                Random random = new Random();
                foreach (var sd in stageDurationSeries.Values)
                {
                    var randomColor = new SKColor((byte)random.Next(0, 257), (byte)random.Next(0, 257), (byte)random.Next(0, 257));
                    stageDurationSeries.Fill = new SolidColorPaint(randomColor);
                }

                // Create an X Axis
                var xAxis = new Axis
                {
                    Labels = sortedHistory.Select(sh => sh.ProductStage.Name).ToArray(),
                    LabelsRotation = 45,
                    TextSize = 12
                };

                // Create a Y Axis
                var yAxis = new Axis
                {
                    Name = "Duration",
                    NamePaint = new SolidColorPaint(SKColors.Gray),
                    TextSize = 12,
                    Labeler = value =>
                    {
                        int days = (int)value;
                        double fractionalDay = value - days;
                        int totalSeconds = (int)(fractionalDay * 24 * 60 * 60);
                        int hours = totalSeconds / 3600;
                        int minutes = (totalSeconds % 3600) / 60;
                        int seconds = totalSeconds % 60;

                        if (days > 0)
                            return $"{days}d {hours}h {minutes}m";
                        else if (hours > 0)
                            return $"{hours}h {minutes}m";
                        else
                            return $"{minutes}m {seconds}s"; // Show seconds <1h
                    }
                };

                StageSeries = new ISeries[] { stageDurationSeries };
                XAxes = new Axis[] { xAxis };
                YAxes = new Axis[] { yAxis };
            }
            else
            {
                // Fallback
                StageSeries = new ISeries[]
                {
            new ColumnSeries<double>
            {
                Values = new double[] { 0 },
                Name = "No stage history available"
            }
                };

                XAxes = new Axis[] { new Axis { Labels = new[] { "No Data" } } };
                YAxes = new Axis[] { new Axis { Name = "Duration (Days)" } };
            }
        }


        [RelayCommand]
        private async Task LoadStages()
        {
            EndDate = DateTime.Now;
            try
            {
                stages = (List<StageDto>)await _stageService.GetAllStagesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task LoadProductDetails()
        {
            try
            {


                var currentDate = DateTime.Now;
                CurrentStage = CurrentProduct.Curentstage;

                // Convert ProductStageHistoryDto objects to ProductStageHistoryViewModel objects
                StageHistory = new ObservableCollection<ProductStageHistoryViewModel>(
                    CurrentProduct.StageHistory.Select(dto => new ProductStageHistoryViewModel(dto))
                );

                //if the end time is = to the start time then the stage is active
                BOMMaterials = new ObservableCollection<BomMaterialDto>(CurrentProduct.ProductBom.BomMaterials);

                // Find the most recent stage history and wrap it
                var latestHistoryDto = CurrentProduct.StageHistory
                    .Where(stage => stage.StartDate <= currentDate)
                    .OrderByDescending(stage => stage.StartDate)
                    .FirstOrDefault();

                if (latestHistoryDto != null)
                {
                    CurrentStageHistory = new ProductStageHistoryViewModel(latestHistoryDto);
                }



                await LoadNextStage();
                SetupPieChart();
                SetupStageHistoryChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadNextStage()
        {
            if (stages.Count <= 0)
            {
                await LoadStages();
            }

            if (stages.Count <= CurrentStage.Id)
            {
                NextStage = CurrentStage.Name;
            }
            else
            {
                NextStage = stages[CurrentStage.Id].Name;
            }
        }
        public void Receive(ProductSelectedMessage message)
        {
            var Product = message.Value;
            if (Product != null)
            {

                CurrentProduct = Product;
                LoadProductDetails();
            }
        }

        [RelayCommand]
        public async Task ChangeStage()
        {
            try
            {
                StageDto stage;
                var currentStageIndex = CurrentStage.Id;

                if (currentStageIndex < stages.Count)
                {
                    stage = stages[currentStageIndex];
                }
                else
                {
                    ButtonVisibility = Visibility.Collapsed;
                    return;
                }

                if (EndDate < DateTime.Now)
                {
                    EndDate = DateTime.Now;
                }

                // Update the current stage endDate
                if (CurrentStageHistory != null)
                {
                    CurrentStageHistory.EndDate = DateTime.Now;
                    await _productStageHistoryService.UpdateProductStageHistoryAsync(
                        CurrentStageHistory.GetDto(), CurrentProduct.Id);
                }

                // Create a new history entry
                var newHistoryDto = new ProductStageHistoryDto
                {
                    ProductStage = stage,
                    StartDate = DateTime.Now,
                    User = _authService.CurrentUser,
                    EndDate = EndDate
                };

                await _productService.AddProductStageAsync(CurrentProduct, newHistoryDto);
                CurrentProduct.StageHistory.Add(newHistoryDto);

                // Update the UI collection by finding and updating the wrapped object
                var updatedStageHistory = StageHistory.FirstOrDefault(sh =>
                    sh.ProductStage.Id == CurrentStage.Id);

                if (updatedStageHistory != null)
                {
                    updatedStageHistory.EndDate = DateTime.Now;
                }

                // Add the new history item as a wrapped object
                StageHistory.Add(new ProductStageHistoryViewModel(newHistoryDto));

                CurrentProduct.Curentstage = stage;
                CurrentStage = stage;
                CurrentStageHistory = StageHistory.Last();

                await LoadNextStage();
                SetupStageHistoryChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        public void GoBack()
        {
            _navigationService.NavigateBack();
        }
       
    }
}
