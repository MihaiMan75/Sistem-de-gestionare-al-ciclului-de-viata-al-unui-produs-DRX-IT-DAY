using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Microsoft.Win32;
using LiveChartsCore.SkiaSharpView.SKCharts;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView;  
using SkiaSharp;  
using System.Windows;
using WPF_UI.Interfaces;
using WPF_UI.Messages;
using Style = MigraDoc.DocumentObjectModel.Style;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore;
using System.IO;
using LiveChartsCore.SkiaSharpView.Painting;
using static BusinessLogic.Enums;
using WPF_UI.Services;
using WPF_UI.Wrappers;

namespace WPF_UI.ViewModels
{
    public partial class ReportsViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly IServiceFactory _serviceFactory;
        private readonly INavigationService _navigationService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<ProductDto> _availableProducts;

        [ObservableProperty]
        private ProductDto _selectedAvailableProduct;

        [ObservableProperty]
        private ObservableCollection<ProductDto> _productsForReport;

        [ObservableProperty]
        private ProductDto _selectedProductForReport;

        [ObservableProperty]
        private bool _includeStageHistory = true;

        [ObservableProperty]
        private bool _includeBOMDetails = true;

        [ObservableProperty]
        private bool _includeCharts = true;
        [ObservableProperty]
        private ObservableCollection<ProductDto> _filteredProducts;

        [ObservableProperty]
        private string _showAllProductsText = "Show all products";

        private bool _showAllProducts = false;
        private UserDto _currentUser;

        [ObservableProperty]
        private string _reportTitle = "Product Status Report";

        public ReportsViewModel(IServiceFactory serviceFactory, IAuthService authService, INavigationService navigationService)
        {
            _serviceFactory = serviceFactory;
            _navigationService = navigationService;
            _productService = _serviceFactory.GetProductService();
            _authService = authService;
            _currentUser = _authService.CurrentUser;

            AvailableProducts = new ObservableCollection<ProductDto>();
            ProductsForReport = new ObservableCollection<ProductDto>();

            LoadProductsCommand.Execute(null);
        }

        partial void OnSearchTextChanged(string value)
        {
            FilterProducts();
        }



        private void FilterProducts()
        {
            //filter in function of the user role

            if (_showAllProducts)
            {
                FilteredProducts = new ObservableCollection<ProductDto>(AvailableProducts);

            }
            else
            {
                //filter in function of the user role
                //foreach role add the products that the user can see into a collection using the PermissionService
                FilteredProducts = new ObservableCollection<ProductDto>();

                foreach (ProductDto product in AvailableProducts)
                {
                    if (PermissionService.HasPermission(_currentUser, (Stages)product.Curentstage.Id))
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

            FilteredProducts = new ObservableCollection<ProductDto>(filtered);
        }

        [RelayCommand]
        private void ShowAll()
        {
            _showAllProducts = !_showAllProducts;
            FilterProducts();
            if (_showAllProducts)
            {
                ShowAllProductsText = "Show my products";
            }
            else
            {
                ShowAllProductsText = "Show all products";
            }
        }

        [RelayCommand]
        private async Task LoadProducts()
        {
            try
            {
                var allProducts = await _productService.GetAllProductsAsync();

                // Get products that are not already in the report list
                var availableProductsList = allProducts.Where(p =>
                    !ProductsForReport.Any(rp => rp.Id == p.Id)).ToList();

                AvailableProducts = new ObservableCollection<ProductDto>(availableProductsList);
                FilterProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        [RelayCommand]
        private void AddToReport(ProductDto product)
        {
            if (product == null) return;

            ProductsForReport.Add(product);
            AvailableProducts.Remove(product);
        }


        [RelayCommand]
        private void RemoveFromReport(ProductDto product)
        {
            if (product == null) return;

            AvailableProducts.Add(product);
            ProductsForReport.Remove(product);
        }

        [RelayCommand]
        private void AddAllToReport()
        {
            foreach (var product in AvailableProducts.ToList())
            {
                ProductsForReport.Add(product);
            }
            AvailableProducts.Clear();
        }

        [RelayCommand]
        private void RemoveAllFromReport()
        {
            foreach (var product in ProductsForReport.ToList())
            {
                AvailableProducts.Add(product);
            }
            ProductsForReport.Clear();
        }

        [RelayCommand]
        private void ViewProduct(ProductDto product)
        {
            if (product == null) return;

            _navigationService.NavigateTo<ProductDetailsViewModel>();
            WeakReferenceMessenger.Default.Send(new ProductSelectedMessage(product));
        }



        [RelayCommand]
        private void GenerateReport()
        {

            if (ProductsForReport.Count == 0)
            {
                MessageBox.Show("Please add at least one product to generate a report.",
                    "No Products Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Create save file dialog
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    DefaultExt = "pdf",
                    FileName = $"{ReportTitle}_{DateTime.Now:yyyyMMdd}"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var document = CreateReport();

                    // Render PDF
                    PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
                    {
                        Document = document
                    };

                    renderer.RenderDocument();

                    // Save to file
                    renderer.PdfDocument.Save(saveFileDialog.FileName);

                    MessageBox.Show($"Report successfully generated: {saveFileDialog.FileName}",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

       
        private Document CreateReport()
        {
            
            Document document = new Document();
            DefineStyles(document);

            
            Section section = document.AddSection();
            section.PageSetup.Orientation = Orientation.Portrait;
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);

            double usableWidth = section.PageSetup.PageWidth.Point
                    - section.PageSetup.LeftMargin.Point
                    - section.PageSetup.RightMargin.Point;

            // Title
            Paragraph titleParagraph = section.AddParagraph(ReportTitle);
            titleParagraph.Style = "Title";
            titleParagraph.Format.SpaceAfter = "0.4cm";

            foreach (var product in ProductsForReport)
            {
                // one page per product
                Section productSection = document.LastSection;

                AddProductDetails(productSection, product);

                if (IncludeStageHistory)
                {
                    AddStageHistory(productSection, product);
                }

                if (IncludeBOMDetails)
                {
                    AddBOMDetails(productSection, product);
                }

                if (IncludeCharts)
                {
                    AddCharts(productSection, product);
                }

                //page break between products
                productSection.AddPageBreak();
            }

            return document;
        }

        private void AddCharts(Section section, ProductDto product)
        {
            
            Paragraph chartsHeading = section.AddParagraph("Product Charts");
            chartsHeading.Style = "Heading2";

            try
            {
                // temporary view model to render the charts
                var tempViewModel = new ProductDetailsViewModel(_serviceFactory, _authService, null);

                tempViewModel.LoadProduct(product);

                if (product.ProductBom?.BomMaterials?.Count > 0)
                {                    tempViewModel.SetupPieChart();

                    // Image bytes for the pie chart
                    byte[] pieChartImageBytes = RenderPieChartToImage(
                         tempViewModel.MaterialsSeries.ToArray(),
                         tempViewModel.MaterialPieTitle,
                         400, 250);

                    Paragraph pieParagraph = section.AddParagraph("Materials Distribution:");
                    pieParagraph.Format.SpaceBefore = "0.2cm";

                    var pieImage = section.AddImage(WPF_UI.ViewModels.MigraDocObject.GetMigraDocFilename(pieChartImageBytes));
                    pieImage.Width = "12cm";
                    pieParagraph.Format.SpaceAfter = "0.2cm";
                }

                if (product.StageHistory?.Count > 0)
                {
                    
                    tempViewModel.SetupStageHistoryChart();

                    // Image bytes for the stage history chart
                    byte[] barChartImageBytes = RenderCartesianChartToImage(
                        tempViewModel.StageSeries.ToArray(),
                        tempViewModel.XAxes.ToArray(),
                        tempViewModel.YAxes.ToArray(),
                        600, 350);

                    Paragraph stageParagraph = section.AddParagraph("Stage Duration:");
                    stageParagraph.Format.SpaceBefore = "0.2cm";

                    var stageImage = section.AddImage(WPF_UI.ViewModels.MigraDocObject.GetMigraDocFilename(barChartImageBytes));
                    stageImage.Width = "14cm"; 
                    stageParagraph.Format.SpaceAfter = "0.4cm";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error rendering charts: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Paragraph errorParagraph = section.AddParagraph("Charts could not be displayed.");
                errorParagraph.Format.Font.Italic = true;
            }
        }

        private void DefineStyles(Document document)
        {
            // Title
            Style titleStyle = document.Styles["Title"] ?? document.Styles.AddStyle("Title", "Normal");
            titleStyle.Font.Size = 12;
            titleStyle.Font.Bold = true;
            titleStyle.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            titleStyle.Font.Color = Colors.DarkSlateGray; 

            // Heading1
            Style heading1Style = document.Styles.AddStyle("Heading1", "Normal");
            heading1Style.Font.Size = 10;
            heading1Style.Font.Bold = true;
            heading1Style.ParagraphFormat.SpaceBefore = "0.2cm";
            heading1Style.ParagraphFormat.SpaceAfter = "0.2cm";
            heading1Style.Font.Color = Colors.Teal;

            // Heading2
            Style heading2Style = document.Styles.AddStyle("Heading2", "Normal");
            heading2Style.Font.Size = 8;
            heading2Style.Font.Bold = true;
            heading2Style.ParagraphFormat.SpaceBefore = "0.1cm";
            heading2Style.ParagraphFormat.SpaceAfter = "0.1cm";
            heading2Style.Font.Color = Colors.DarkCyan; 

            // Base
            Style normalStyle = document.Styles["Normal"];
            normalStyle.Font.Size = 7;
            normalStyle.Font.Color = Colors.Black; 
        }

        private void AddProductDetails(Section section, ProductDto product)
        {
            // Name as heading
            Paragraph productNameParagraph = section.AddParagraph(product.Name);
            productNameParagraph.Style = "Heading1";

            // description
            Paragraph descriptionParagraph = section.AddParagraph(product.Description);
            descriptionParagraph.Format.SpaceAfter = "0.2cm";

            //  dimensions and weight
            Paragraph dimensionsParagraph = section.AddParagraph($"Estimated Height: {product.EstimatedHeight}, Estimated Width: {product.EstimatedWidth}, Estimated Weight: {product.EstimatedWeight}");
            dimensionsParagraph.Format.SpaceAfter = "0.2cm";

            // current stage
            Paragraph currentStageParagraph = section.AddParagraph($"Current Stage: {product.Curentstage.Name}");
            currentStageParagraph.Format.SpaceAfter = "0.2cm";
        }

        private void AddStageHistory(Section section, ProductDto product)
        {
            // stage history 
            Paragraph stageHistoryHeading = section.AddParagraph("Stage History");
            stageHistoryHeading.Style = "Heading2";

            
            Table stageHistoryTable = section.AddTable();
            stageHistoryTable.Borders.Width = 0.75;

         
            stageHistoryTable.AddColumn("4cm"); // Stage Name
            stageHistoryTable.AddColumn("3cm"); // Start Date
            stageHistoryTable.AddColumn("3cm"); // End Date
            stageHistoryTable.AddColumn("4cm"); // User

            
            Row headerRow = stageHistoryTable.AddRow();
            headerRow.HeadingFormat = true;
            headerRow.Format.Font.Bold = true;
            headerRow.Shading.Color = new Color(0, 206, 201); // #00cec9
            headerRow.Cells[0].AddParagraph("Stage Name");
            headerRow.Cells[1].AddParagraph("Start Date");
            headerRow.Cells[2].AddParagraph("End Date");
            headerRow.Cells[3].AddParagraph("User");

            // alternating background colors
            bool useAlternateColor = false;
            foreach (var stageHistory in product.StageHistory)
            {
                Row row = stageHistoryTable.AddRow();
                row.Shading.Color = useAlternateColor ? new Color(129, 236, 236) : Colors.White; // #81ecec or white
                useAlternateColor = !useAlternateColor; 

                row.Cells[0].AddParagraph(stageHistory.ProductStage.Name);
                row.Cells[1].AddParagraph(stageHistory.StartDate.ToString("yyyy-MM-dd"));
                row.Cells[2].AddParagraph(stageHistory.EndDate?.ToString("yyyy-MM-dd") ?? "N/A");
                row.Cells[3].AddParagraph(stageHistory.User.Name);
            }

            section.AddParagraph().Format.SpaceAfter = "0.2cm";
        }


        private void AddBOMDetails(Section section, ProductDto product)
        {
            Paragraph bomHeading = section.AddParagraph("Bill of Materials (BOM)");
            bomHeading.Style = "Heading2";

            Table bomTable = section.AddTable();
            bomTable.Borders.Width = 0.75;

            bomTable.AddColumn("4cm"); // Material Description
            bomTable.AddColumn("3cm"); // Quantity
            bomTable.AddColumn("3cm"); // Unit Measure
            bomTable.AddColumn("4cm"); // Weight


            Row headerRow = bomTable.AddRow();
            headerRow.HeadingFormat = true;
            headerRow.Format.Font.Bold = true;
            headerRow.Shading.Color = new Color(0, 206, 201); // #00cec9
            headerRow.Cells[0].AddParagraph("Material Description");
            headerRow.Cells[1].AddParagraph("Quantity");
            headerRow.Cells[2].AddParagraph("Unit Measure");
            headerRow.Cells[3].AddParagraph("Weight");

            bool useAlternateColor = false;
            foreach (var bomMaterial in product.ProductBom.BomMaterials)
            {
                Row row = bomTable.AddRow();
                row.Shading.Color = useAlternateColor ? new Color(129, 236, 236) : Colors.White; // #81ecec or white
                useAlternateColor = !useAlternateColor;

                row.Cells[0].AddParagraph(bomMaterial.Material.MaterialDescription);
                row.Cells[1].AddParagraph(bomMaterial.Quantity.ToString());
                row.Cells[2].AddParagraph(bomMaterial.UnitMeasureCode);
                row.Cells[3].AddParagraph(bomMaterial.Material.Weight.ToString());
            }

            section.AddParagraph().Format.SpaceAfter = "0.2cm";
        }



        //somehow it works
        private byte[] RenderPieChartToImage(IEnumerable<ISeries> series, LabelVisual title, int width = 400, int height = 250)
        {
            var pieChart = new SKPieChart
            {
                Width = width,
                Height = height,
                Series = series.ToArray(),
                Title = title,
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right, 
                LegendTextSize = 10, 
                LegendBackgroundPaint = new SolidColorPaint(SKColors.White.WithAlpha(200)) 
            };

            using (var image = pieChart.GetImage())
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var ms = new MemoryStream())
            {
                data.SaveTo(ms);
                return ms.ToArray();
            }
        }

        private byte[] RenderCartesianChartToImage(IEnumerable<ISeries> series, IEnumerable<Axis> xAxes, IEnumerable<Axis> yAxes, int width = 600, int height = 400)
        {
            var cartesianChart = new SKCartesianChart
            {
                Width = width,
                Height = height,
                Series = series.ToArray(),
                XAxes = xAxes.ToArray(),
                YAxes = yAxes.ToArray()
            };

            using (var image = cartesianChart.GetImage())
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var ms = new MemoryStream())
            {
                data.SaveTo(ms);
                return ms.ToArray();
            }
        }
    }

    //prefereably I should move this to a separate class but for the sake of simplicity I will leave it here
    public static class MigraDocObject
    {
        public static string GetMigraDocFilename(byte[] imageBytes)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), $"chart_{Guid.NewGuid()}.png");

            File.WriteAllBytes(tempPath, imageBytes);

            return tempPath;
        }
    }
}
