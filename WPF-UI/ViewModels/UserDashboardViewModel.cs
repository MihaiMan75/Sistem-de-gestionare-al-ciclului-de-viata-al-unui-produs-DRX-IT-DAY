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
using System.Windows.Data;
using WPF_UI.Interfaces;
using WPF_UI.Messages;
using WPF_UI.Services;
using WPF_UI.Wrappers;
using static BusinessLogic.Enums;

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

        [ObservableProperty]
        private bool _allowedToManageProducts = false;

        [ObservableProperty]
        private string _showAllProductsText = "Show all products";

        private bool _showAllProducts = false;
        private UserDto _currentUser;

        private List<int> allowedToCreateProducIds = new List<int> { 1, 6 }; //creator concept (1) and admin (6)

        public UserDashboardViewModel(IServiceFactory serviceFactory, IAuthService authService, INavigationService navigationService)
        {
            _serviceFactory = serviceFactory;
            _navigationService = navigationService;
            _productService = _serviceFactory.GetProductService();

            Products = new ObservableCollection<ProductDtoWithProgress>();
            FilteredProducts = new ObservableCollection<ProductDtoWithProgress>();
           foreach(var role in authService.CurrentUser.Roles)
            {
                if (allowedToCreateProducIds.Contains(role.Id) )
                {
                    AllowedToManageProducts = true;
                    break;
                }
            }
            _currentUser = authService.CurrentUser;
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

        [RelayCommand]
        private void ShowAll()
        {
            _showAllProducts = !_showAllProducts;
            FilterProducts();
            if(_showAllProducts)
            {
                ShowAllProductsText = "Show my products";
            }
            else
            {
                ShowAllProductsText = "Show all products";
            }
        }

        private void FilterProducts()
        {
            //filter in function of the user role
            FilteredProducts = new ObservableCollection<ProductDtoWithProgress>();
           if (_showAllProducts)
            {
                FilteredProducts = new ObservableCollection<ProductDtoWithProgress>(Products);
        
            }
            else
            {
                //filter in function of the user role
                //foreach role add the products that the user can see into a collection using the PermissionService
                FilteredProducts = new ObservableCollection<ProductDtoWithProgress>();

                if (Products.Count == 0) return;

                    foreach (ProductDtoWithProgress product in Products)
                    {
                       if(PermissionService.HasPermission(_currentUser, (Stages)product.Curentstage.Id))
                        {
                            FilteredProducts.Add(product);
                        }
                    }

            }

            //search bar filter
            if (string.IsNullOrWhiteSpace(SearchText))
            {
  
                return;
            }

            var searchTerm = SearchText.ToLower();
            var filtered = FilteredProducts.Where(p =>
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
            // Map stage IDs to progress values (1-7)
            // Assuming the stages progress from Design (1) to a final stage
            // Modify this mapping based on your actual stage progression
            switch (stageId)
            {
                case 1: // 'Concept'
                    return 0;
                case 2: // 'Fezabilitate'
                    return 1;
                case 3: // 'Proiectare' 
                    return 2;
                case 4: // 'Productie'
                    return 3;
                case 5: // 'Retragere'  
                    return 5;
                case 6: //'Stand by'
                    return 6;
                case 7: // 'Cancel'
                    return 7;
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

