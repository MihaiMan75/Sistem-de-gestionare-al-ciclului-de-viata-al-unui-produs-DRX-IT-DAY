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
using System.Windows.Media.Media3D;

namespace WPF_UI.ViewModels
{
    public partial class MaterialManagementViewModel :BaseViewModel
    {
        private readonly IMaterialsService _materialService;

        [ObservableProperty]
        private ObservableCollection<MaterialDto> _materials;

        [ObservableProperty]
        private MaterialDto _selectedMaterial;

        [ObservableProperty]
        private MaterialDto _currentMaterial;

        [ObservableProperty]
        private string _searchText;

        public MaterialManagementViewModel()
        {
            //delete later
            string connectionString = "Data Source=Lenovo_Teo;Initial Catalog=DRXITDAY;Integrated Security=True;TrustServerCertificate=True";
            DbContext dbContext = new DbContext(connectionString);
            RepositoryFactory repositoryFactory=new RepositoryFactory(dbContext);
            _materialService = new MaterialsService(repositoryFactory);
            //delete later
            //_materialService = materialService;
            LoadMaterials();
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
                if (existingMaterial != null)
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
