using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
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

namespace WPF_UI.ViewModels
{
    public partial class BOMManagementViewModel:BaseViewModel, IRecipient<BomSelectedMessage>
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IBomService _bomService;
        private readonly IBomMaterialService _bomMaterialService;
        private readonly IMaterialsService _materialService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<BomMaterialDto> _bOMMaterials;

        [ObservableProperty]
        private ObservableCollection<MaterialDto> _availableMaterials;

        [ObservableProperty]
        private MaterialDto _selectedMaterial;

        [ObservableProperty]
        private int _quantityToAdd;

        [ObservableProperty]
        private string _unitMeasureToAdd;

        [ObservableProperty]
        private ObservableCollection<int> _unitMeasures;

        [ObservableProperty]
        private BomDto _selectedBOM;

        [ObservableProperty]
        private BomDto _currentBOM;

        [ObservableProperty]
        private BomMaterialDto _selectedBOMMaterial;

        [ObservableProperty]
        private string _searchText;




        public BOMManagementViewModel(IServiceFactory serviceFactory,IAuthService authService, INavigationService navigationService)
        {
            _serviceFactory = serviceFactory;
            _bomService = _serviceFactory.GetBomService();
            _bomMaterialService = _serviceFactory.GetBomMaterialService();
            _materialService = _serviceFactory.GetMaterialsService();
            WeakReferenceMessenger.Default.Register<BomSelectedMessage>(this);
            LoadMaterialsCommand.Execute(null);
            BOMMaterials = new ObservableCollection<BomMaterialDto>();
            UnitMeasures = new ObservableCollection<int> { 1, 2, 3, 4, 5 }; // Added values to the collection
            CurrentBOM = new BomDto();
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task LoadMaterials()
        {
            try
            {
                var materials = await _materialService.GetAllMaterialsAsync();
                AvailableMaterials = new ObservableCollection<MaterialDto>(materials);
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error loading materials: {ex.Message}");
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            LoadMaterialsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task AddMaterial()
        {
            if(QuantityToAdd <= 0 || string.IsNullOrWhiteSpace(UnitMeasureToAdd) || SelectedMaterial == null)
            {
                MessageBox.Show("Please fill all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var bomMaterial = new BomMaterialDto
                {
                    Material = SelectedMaterial,
                    BomId = 0,
                    Quantity = QuantityToAdd,
                    UnitMeasureCode = UnitMeasureToAdd
         };
           
            BOMMaterials.Add(bomMaterial);
            QuantityToAdd = 0;
            UnitMeasureToAdd = string.Empty;
            SelectedMaterial = null;
        }

        [RelayCommand]
        private async Task Save()
        {

            try
            {
                
                if (string.IsNullOrWhiteSpace(CurrentBOM.Name))
                {

                    MessageBox.Show("Please fill the name field", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new Exception("Please fill the name field");
                }

                if (!BOMMaterials.Any())
                {
                    MessageBox.Show("Please add at least one material", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new Exception("Please add at least one material");
                }

                var existingBOM = await _bomService.GetBomByIdAsync(CurrentBOM.Id);
                if (existingBOM != null)
                { //update
                    existingBOM.Name = CurrentBOM.Name;
                    existingBOM.BomMaterials = BOMMaterials.ToList();
                    foreach (var bomMaterial in existingBOM.BomMaterials)
                    {
                        bomMaterial.BomId = existingBOM.Id;
                    }
                    //await _bomService.UpdateBomAsync(existingBOM);
                    WeakReferenceMessenger.Default.Send(new BomSelectedMessage(CurrentBOM));

                    _navigationService.NavigateBack();

                }
                else
                {
                    CurrentBOM.BomMaterials = BOMMaterials.ToList();

                    //We save the BOM in productService to only
                   // var bomId = await _bomService.CreateBomAsync(CurrentBOM);
                   // CurrentBOM.Id = bomId;

                    // Send message to update the ProductManagementViewModel
                    WeakReferenceMessenger.Default.Send(new BomSelectedMessage(CurrentBOM));

                    _navigationService.NavigateBack();
                }

               // CurrentBOM = new BomDto();
              //  BOMMaterials.Clear();


                //MessageBox.Show("BOM saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving BOM: {ex.Message}");
                MessageBox.Show("Error saving BOM" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        [RelayCommand]
        private void Delete(BomMaterialDto bomMaterialDto)
        {
            BOMMaterials.Remove(bomMaterialDto);
        }

        [RelayCommand]
        private void Cancel()
        {
            CurrentBOM = new BomDto();
            BOMMaterials.Clear();
            SelectedMaterial = null;
            QuantityToAdd = 0;
            UnitMeasureToAdd = string.Empty;
        }

        public void Receive(BomSelectedMessage message)
        {
            CurrentBOM = message.Value;
            BOMMaterials = new ObservableCollection<BomMaterialDto>(CurrentBOM.BomMaterials);
        }
    }
}
