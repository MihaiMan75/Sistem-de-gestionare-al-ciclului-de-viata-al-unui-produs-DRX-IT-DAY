using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using WPF_UI.Interfaces;

namespace WPF_UI.ViewModels
{
    public partial class MaterialManagementViewModel :BaseViewModel
    {
        private readonly IMaterialsService _materialService;
        private readonly IServiceFactory _serviceFactory;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private ObservableCollection<MaterialDto> _materials;

        [ObservableProperty]
        private MaterialDto _selectedMaterial;

        [ObservableProperty]
        private MaterialDto _currentMaterial;

        [ObservableProperty]
        private string _searchText;

        public MaterialManagementViewModel(IServiceFactory serviceFactory, IAuthService authService, INavigationService navigationService)
        {
            _serviceFactory = serviceFactory;
            _materialService = _serviceFactory.GetMaterialsService();
            _authService = authService;
            LoadMaterialsCommand.Execute(null);
            CurrentMaterial = new MaterialDto();
        }

        [RelayCommand]
        private async Task LoadMaterials()
        {
            try
            {
                var materials = await _materialService.GetAllMaterialsAsync();
                Materials = new ObservableCollection<MaterialDto>(materials);
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Error loading materials: {ex.Message}");
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                LoadMaterialsCommand.Execute(null);
                return;
            }

            var filteredList = Materials.Where(m =>
            m.MaterialNumber.ToString().Contains(value) ||
                m.MaterialDescription.ToLower().Contains(value.ToLower()));

            Materials = new ObservableCollection<MaterialDto>(filteredList);
        }

        //private bool CanSave()
        //{
        //    return CurrentMaterial != null &&
        //           !string.IsNullOrWhiteSpace(CurrentMaterial.MaterialDescription) &&
        //           CurrentMaterial.Weight > 0;
        //}

        //[RelayCommand(CanExecute = nameof(CanSave))]
        [RelayCommand]
        private async Task Save()
        {
            try
            {
              var existingMaterial = await _materialService.GetMaterialByIdAsync(CurrentMaterial.MaterialNumber);

                if(existingMaterial != null)
                { 
                    existingMaterial.MaterialNumber = CurrentMaterial.MaterialNumber;
                    existingMaterial.MaterialDescription = CurrentMaterial.MaterialDescription;
                    existingMaterial.Weight = CurrentMaterial.Weight;
                    existingMaterial.Height = CurrentMaterial.Height;
                    existingMaterial.Width = CurrentMaterial.Width;
                    await _materialService.UpdateMaterialAsync(existingMaterial);

                }
                else
                {
                    await _materialService.CreateMaterialAsync(CurrentMaterial);
                }
                
                await LoadMaterials();
                ResetForm();
            }
            catch (Exception ex)
            {
                // Handle error - could show message to user
                System.Diagnostics.Debug.WriteLine($"Error saving material: {ex.Message}");
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            ResetForm();
        }

        [RelayCommand]
        private void Edit(MaterialDto material)
        {
            if (material == null) return;

            // Create a copy of the material for editing
            CurrentMaterial = new MaterialDto
            {
                MaterialNumber = material.MaterialNumber,
                MaterialDescription = material.MaterialDescription,
                Weight = material.Weight,
                Height = material.Height,
                Width = material.Width
            };
        }

        [RelayCommand]
        private async Task Delete(MaterialDto material)
        {
            if (material == null) return;
            var answ = MessageBox.Show("Are you sure you want to delete this Materil?", "Delete Material", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answ == MessageBoxResult.No)
            {
                return;
            }
            try
            {
                // Could add confirmation dialog here
                await _materialService.DeleteMaterialAsync(material.MaterialNumber);
                await LoadMaterials();
            }
            catch (Exception ex)
            {
                // Handle error - could show message to user
                System.Diagnostics.Debug.WriteLine($"Error deleting material: {ex.Message}");
            }
        }

        private void ResetForm()
        {
            CurrentMaterial = new MaterialDto();
            SelectedMaterial = null;
        }

      
    }
}
