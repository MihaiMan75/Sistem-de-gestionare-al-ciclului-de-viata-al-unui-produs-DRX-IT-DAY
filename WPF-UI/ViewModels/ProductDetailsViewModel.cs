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
        private ProductStageHistoryDto _currentStageHistory;

        [ObservableProperty]
        private ObservableCollection<BomMaterialDto> _bOMMaterials;

        [ObservableProperty]
        private ObservableCollection<ProductStageHistoryDto> _stageHistory;

        [ObservableProperty]
        private string _nextStage = "null";

        [ObservableProperty]
        private Visibility _buttonVisibility = Visibility.Visible;


        private readonly IServiceFactory _serviceFactory;
        public ProductDetailsViewModel(IServiceFactory serviceFactory, IAuthService authService, INavigationService navigationService)
        {
            _serviceFactory = serviceFactory;
            _productService = _serviceFactory.GetProductService();
            _stageService = _serviceFactory.GetStageService();
            _productStageHistoryService = _serviceFactory.GetProductStageHistoryService();
            _authService = authService;
            BOMMaterials = new ObservableCollection<BomMaterialDto>();
            StageHistory = new ObservableCollection<ProductStageHistoryDto>();
           // LoadStagesCommand.Execute(null);
            WeakReferenceMessenger.Default.Register<ProductSelectedMessage>(this);
            _navigationService = navigationService;
           

        }

        [RelayCommand]
        private async Task LoadStages()
        {
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
                StageHistory = new ObservableCollection<ProductStageHistoryDto>(CurrentProduct.StageHistory);
                //if the end time is = to the start time then the stage is active
                BOMMaterials = new ObservableCollection<BomMaterialDto>(CurrentProduct.ProductBom.BomMaterials);
                CurrentStageHistory = StageHistory.Where(stage => stage.StartDate <= currentDate)
                        .OrderByDescending(stage => stage.StartDate)
                        .FirstOrDefault();
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
            //verfy if theres another stage in the enum //test only
            try
            {
                StageDto stage;
                var currentStageIndex = CurrentStage.Id;
                if (currentStageIndex  < stages.Count) //Id starts from 1 and the list form 0 so stage = stages[currentStageIndex]; goes by one up.
                {
                    stage = stages[currentStageIndex];
                }
                else
                {
                    ButtonVisibility = Visibility.Collapsed;
                    return;
                }


                //update the product stage
                var productStageHisotry = new ProductStageHistoryDto
                {
                    ProductStage = stage,
                    StartDate = DateTime.Now,
                    User = _authService.CurrentUser,
                    EndDate = DateTime.Now
                };

                    await _productService.AddProductStageAsync(CurrentProduct, productStageHisotry);

                //update the current stage endDate
                if (CurrentStageHistory != null)
                {
                    CurrentStageHistory.EndDate = DateTime.Now;
                    await _productStageHistoryService.UpdateProductStageHistoryAsync(CurrentStageHistory, CurrentProduct.Id);
                    StageHistory.Remove(CurrentStageHistory);
                    StageHistory.Add(CurrentStageHistory);

                }

                //update the current product in view
                CurrentProduct.StageHistory.Add(productStageHisotry);
            StageHistory.Add(productStageHisotry);
            CurrentProduct.Curentstage = stage;
            CurrentStage = stage;
            await LoadNextStage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        [RelayCommand]
        public void GoBack()
        {
            _navigationService.NavigateBack();
        }
    }
}
