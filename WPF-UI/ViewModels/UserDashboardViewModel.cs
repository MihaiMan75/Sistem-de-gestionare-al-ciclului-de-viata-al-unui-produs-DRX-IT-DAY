using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataAccess.Models;
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
    public partial class UserDashboardViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly INavigationService _navigationService;
        private readonly IServiceFactory _serviceFactory;

        [ObservableProperty]
        private ObservableCollection<ProductDtoWithProgress> _products;

        [ObservableProperty]
        private ObservableCollection<ProductDtoWithProgress> _filteredProducts;

        [ObservableProperty]
        private string _searchText;


        public UserDashboardViewModel(IServiceFactory serviceFactory, IAuthService authService, INavigationService navigationService)
        {
            _serviceFactory = serviceFactory;
            _navigationService = navigationService;
            _productService = _serviceFactory.GetProductService();

            Products = new ObservableCollection<ProductDtoWithProgress>();
            FilteredProducts = new ObservableCollection<ProductDtoWithProgress>();

            // Load products when the ViewModel is created
            LoadProductsCommand.Execute(null);
        }


        partial void OnSearchTextChanged(string value)
        {
            FilterProducts();
        }

        [RelayCommand]
        private async Task LoadProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();

                // Convert ProductDto to ProductDtoWithProgress
                var productsWithProgress = products.Select(p => new ProductDtoWithProgress
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    EstimatedHeight = p.EstimatedHeight,
                    EstimatedWidth = p.EstimatedWidth,
                    EstimatedWeight = p.EstimatedWeight,
                    ProductBom = p.ProductBom,
                    StageHistory = p.StageHistory,
                    Curentstage = p.Curentstage,
                    StageProgressValue = CalculateStageProgress(p.Curentstage?.Id ?? 0)
                }).ToList();

                Products = new ObservableCollection<ProductDtoWithProgress>(productsWithProgress);
                FilterProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredProducts = new ObservableCollection<ProductDtoWithProgress>(Products);
                return;
            }

            var searchTerm = SearchText.ToLower();
            var filtered = Products.Where(p =>
                (p.Name?.ToLower().Contains(searchTerm) ?? false) ||
                (p.Description?.ToLower().Contains(searchTerm) ?? false)).ToList();

            FilteredProducts = new ObservableCollection<ProductDtoWithProgress>(filtered);
        }

        [RelayCommand]
        private void ViewProductDetails(ProductDtoWithProgress product)
        {
            if (product == null) return;

            // Use the original ProductDto for messaging
            var originalProductDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                EstimatedHeight = product.EstimatedHeight,
                EstimatedWidth = product.EstimatedWidth,
                EstimatedWeight = product.EstimatedWeight,
                ProductBom = product.ProductBom,
                StageHistory = product.StageHistory,
                Curentstage = product.Curentstage
            };

            _navigationService.NavigateTo<ProductDetailsViewModel>();
            WeakReferenceMessenger.Default.Send(new ProductSelectedMessage(originalProductDto));
        }

        [RelayCommand]
        private void CreateNewProduct()
        {
            _navigationService.NavigateTo<ProductManagementViewModel>();
        }

        private int CalculateStageProgress(int stageId)
        {
            // Map stage IDs to progress values (0-4)
            // Assuming the stages progress from Design (1) to a final stage
            // Modify this mapping based on your actual stage progression
            switch (stageId)
            {
                case 1: // Design
                    return 1;
                case 2: // Production
                    return 2;
                case 3: // Testing
                    return 3;
                case 4: // Deployment
                    return 4;
                default:
                    return 0;
            }
        }
    

    [RelayCommand]
        private void EditProduct(ProductDtoWithProgress product)
        {
            if (product == null) return;
            
            var originalProductDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                EstimatedHeight = product.EstimatedHeight,
                EstimatedWidth = product.EstimatedWidth,
                EstimatedWeight = product.EstimatedWeight,
                ProductBom = product.ProductBom,
                StageHistory = product.StageHistory,
                Curentstage = product.Curentstage
            };
            _navigationService.NavigateTo<ProductManagementViewModel>();
            WeakReferenceMessenger.Default.Send(new ProductSelectedMessage(originalProductDto));
        }
    }
}

