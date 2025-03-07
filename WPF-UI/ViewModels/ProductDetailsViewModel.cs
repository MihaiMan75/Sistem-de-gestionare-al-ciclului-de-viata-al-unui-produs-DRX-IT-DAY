using BusinessLogic;
using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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
