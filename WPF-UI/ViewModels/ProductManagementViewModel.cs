using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using BusinessLogic.Services;
using System.ComponentModel;
using WPF_UI.Interfaces;
using BusinessLogic;

namespace WPF_UI.ViewModels
{
    public partial class ProductManagementViewModel:BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly IBomService _bomService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ProductDto _selectedProduct;

        [ObservableProperty]
        private ProductDto _currentProduct;

        [ObservableProperty]
        private ObservableCollection<ProductDto> _products;

        [ObservableProperty]
        private ObservableCollection<BomDto> _bOMs;



        private readonly IServiceFactory _serviceFactory;
        public ProductManagementViewModel(IServiceFactory serviceFactory, IAuthService authService)
        {
            _serviceFactory = serviceFactory;
            _productService = _serviceFactory.GetProductService();
            _bomService = _serviceFactory.GetBomService();
            _authService = authService;
            _products = new ObservableCollection<ProductDto>();
            _bOMs = new ObservableCollection<BomDto>();
            LoadProductsCommand.Execute(null);
            LoadBOMsCommand.Execute(null);

            //test
            CurrentProduct = new ProductDto();
            CurrentProduct.Id = 0;
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                LoadProductsCommand.Execute(null);
                return;
            }

            var filteredList = Products.Where(p =>
            p.Name.ToString().Contains(value) ||
                p.Description.ToLower().Contains(value.ToLower()));

            Products = new ObservableCollection<ProductDto>(filteredList);
        }
        [RelayCommand]
        private async Task LoadProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                Products = new ObservableCollection<ProductDto>(products);

                //debug
                MessageBox.Show("Products loaded");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task LoadBOMs()
        {
            try
            {
                var boms = await _bomService.GetAllBomsAsync();
                BOMs = new ObservableCollection<BomDto>(boms);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading BOMs: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            try
            {
                var existingProduct = await _productService.GetProductByIdAsync(CurrentProduct.Id);

                if (existingProduct != null)
                {
                    existingProduct.Description = CurrentProduct.Description;
                    existingProduct.EstimatedWeight = CurrentProduct.EstimatedWeight;
                    existingProduct.EstimatedHeight = CurrentProduct.EstimatedHeight;
                    existingProduct.EstimatedWidth = CurrentProduct.EstimatedWidth;
                    existingProduct.Name = CurrentProduct.Name;
                    existingProduct.ProductBom = CurrentProduct.ProductBom;


                    //momentanly we are not updating the stage history
                    existingProduct.StageHistory = CurrentProduct.StageHistory;

                    await _productService.UpdateProductAsync(existingProduct);
                }
                else
                {   //for stage history we need to add the first stage somehow with the current user
                    if (_authService == null)
                    {
                        throw new InvalidOperationException("Authentication service is not initialized.");
                    }

                    List<ProductStageHistoryDto> stageHistory = new List<ProductStageHistoryDto>();
                    stageHistory.Add(new ProductStageHistoryDto
                    {
                        ProductStage = new StageDto
                        {
                            Id = (int)Enums.Stage.Design,
                            Name = Enums.Stage.Design.ToString(),
                            Description = "Design stage"
                        },
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        User = _authService.CurrentUser

                    });
                    CurrentProduct.StageHistory = stageHistory;
                    CurrentProduct.Curentstage = stageHistory.FirstOrDefault()?.ProductStage;
                    CurrentProduct.Id = await _productService.CreateProductAsync(CurrentProduct);
                }

                await LoadProducts();
                ResetForm();
            }
            catch (Exception ex)
            {
                // Handle error - could show message to user
                System.Diagnostics.Debug.WriteLine($"Error saving Product: {ex.Message}");
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            ResetForm();
        }

        [RelayCommand]
        private  async void Edit(ProductDto Product)
        {
            if (Product == null) return;

            // Create a copy of the material for editing
            CurrentProduct = new ProductDto
            {
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description,
                EstimatedHeight = Product.EstimatedHeight,
                EstimatedWidth = Product.EstimatedWidth,
                EstimatedWeight = Product.EstimatedWeight,
                ProductBom = Product.ProductBom,
                StageHistory = Product.StageHistory,
                Curentstage = Product.Curentstage
            };
        }

        [RelayCommand]
        private async Task Delete(ProductDto product)
        {
            if (product == null) return;

            try
            {
                // Could add confirmation dialog here
                await _productService.DeleteProductAsync(product.Id);
                await LoadProducts();
            }
            catch (Exception ex)
            {
                // Handle error - could show message to user
                System.Diagnostics.Debug.WriteLine($"Error deleting Product: {ex.Message}");
            }
        }

        private void ResetForm()
        {
            CurrentProduct = new ProductDto();
            SelectedProduct = null;
        }
    }
}
