using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Text.Json;
using Climaster_feature.Models;
using Climaster_feature.Services;
using Climaster_feature.Helpers;
using Climaster_feature.Resources;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Climaster_feature.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly QRCodeService _qrCodeService;
        private readonly LocalServerService _serverService;

        private string _widgetId = "custom_widget_v1";
        private string _backgroundColor = "#33FFFFFF";
        private string _baseColor = "#FFFFFF"; // Kolorea alpha gabe
        private int _backgroundOpacity = 20; // 0-100 (20 = 33 hex-en)
        private int _cornerRadius = 24;
        private string _previewJson = string.Empty;
        private bool _isServerRunning = false;
        private string _serverUrl = string.Empty;
        private BitmapImage? _qrCodeImage = null;
        private WidgetGridSize _selectedSize;
        private WidgetElement? _selectedElement;

        public MainViewModel()
        {
            _qrCodeService = new QRCodeService();
            _serverService = new LocalServerService();

            // Eskuragarri dauden tamainak hasieratu
            InitializeWidgetSizes();
            _selectedSize = AvailableSizes[3]; // Lehenetsia: 4x2 (Ertaina)

            // Komandoak
            AddElementCommand = new RelayCommand<string>(AddElement);
            RemoveElementCommand = new RelayCommand<WidgetElement>(RemoveElement);
            MoveUpCommand = new RelayCommand<WidgetElement>(MoveUp);
            MoveDownCommand = new RelayCommand<WidgetElement>(MoveDown);
            GenerateJsonCommand = new RelayCommand(GenerateJson);
            GenerateQRCommand = new RelayCommand(GenerateQRWithJson);
            StartServerCommand = new AsyncRelayCommand(StartServerAsync);
            StopServerCommand = new RelayCommand(StopServer);
            SaveJsonCommand = new RelayCommand(SaveJson);
            SelectElementCommand = new RelayCommand<WidgetElement>(SelectElement);

            // LayoutElements-eko propietate aldaketei harpidetu
            LayoutElements.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (WidgetElement item in e.NewItems)
                    {
                        item.PropertyChanged += (sender, args) => UpdatePreviewJson();
                    }
                }
                OnPropertyChanged(nameof(CurrentElementsInfo));
                OnPropertyChanged(nameof(CanAddMoreElements));
                OnPropertyChanged(nameof(UsedSpacePercentage));
                OnPropertyChanged(nameof(RemainingSpace));
                UpdatePreviewJson();
            };

            // Hasierako elementuekin hasieratu
            LayoutElements.Add(new WidgetElement { Type = "current_temp", FontSize = 48, Alignment = "center" });
            LayoutElements.Add(new WidgetElement { Type = "current_condition_text", FontSize = 20, Alignment = "center" });
            
            UpdatePreviewJson();
        }

        public ObservableCollection<WidgetElement> LayoutElements { get; } = new();
        public ObservableCollection<WidgetGridSize> AvailableSizes { get; } = new();
        public ObservableCollection<string> PredefinedColors { get; } = new()
        {
            "#FFFFFF", "#000000", "#FFFFFF", "#000000",
            "#4CAF50", "#F44336", "#2196F3", "#FF9800",
            "#FFC107", "#00BCD4", "#9C27B0", "#FF5722"
        };

        public WidgetElement? SelectedElement
        {
            get => _selectedElement;
            set => SetProperty(ref _selectedElement, value);
        }

        public WidgetGridSize SelectedSize
        {
            get => _selectedSize;
            set
            {
                if (SetProperty(ref _selectedSize, value))
                {
                    OnPropertyChanged(nameof(MaxElementsInfo));
                    OnPropertyChanged(nameof(CurrentElementsInfo));
                    OnPropertyChanged(nameof(CanAddMoreElements));
                    OnPropertyChanged(nameof(RecommendedElementsInfo));
                    OnPropertyChanged(nameof(CurrentSizeInfo));
                    OnPropertyChanged(nameof(UsedSpacePercentage));
                    OnPropertyChanged(nameof(RemainingSpace));
                    OnPropertyChanged(nameof(SpaceStatusColor));
                    UpdatePreviewJson();
                }
            }
        }

        public string MaxElementsInfo => $"{Strings.MaxElements} {SelectedSize.MaxElements}";
        public string CurrentElementsInfo => $"{Strings.CurrentElements} {LayoutElements.Count}";
        public string CurrentSizeInfo => $"{SelectedSize.Width}x{SelectedSize.Height} bloke";
        public bool CanAddMoreElements => CalculateTotalElementSize() < SelectedSize.MaxElements;
        public string RecommendedElementsInfo => string.Join("\n• ", SelectedSize.RecommendedElements.Prepend(""));
        
        public double UsedSpacePercentage
        {
            get
            {
                if (SelectedSize.MaxElements == 0) return 0;
                return (double)CalculateTotalElementSize() / SelectedSize.MaxElements * 100;
            }
        }

        public int RemainingSpace => SelectedSize.MaxElements - CalculateTotalElementSize();

        public string SpaceStatusColor
        {
            get
            {
                double percentage = UsedSpacePercentage;
                if (percentage >= 90) return "#F44336"; // Gorria
                if (percentage >= 70) return "#FF9800"; // Laranja
                if (percentage >= 50) return "#FFC107"; // Horia
                return "#4CAF50"; // Berdea
            }
        }
        public string WidgetId
        {
            get => _widgetId;
            set
            {
                if (SetProperty(ref _widgetId, value))
                    UpdatePreviewJson();
            }
        }

        public string BaseColor
        {
            get => _baseColor;
            set
            {
                if (SetProperty(ref _baseColor, value))
                {
                    UpdateBackgroundColorFromComponents();
                }
            }
        }

        public int BackgroundOpacity
        {
            get => _backgroundOpacity;
            set
            {
                if (SetProperty(ref _backgroundOpacity, value))
                {
                    OnPropertyChanged(nameof(OpacityPercentage));
                    UpdateBackgroundColorFromComponents();
                }
            }
        }

        public string OpacityPercentage => $"{BackgroundOpacity}%";

        public string BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (SetProperty(ref _backgroundColor, value))
                {
                    // Kolore hex balioaren baitako kolorea eta opazioa atertu
                    ExtractColorComponents(value);
                    UpdatePreviewJson();
                }
            }
        }

        public int CornerRadius
        {
            get => _cornerRadius;
            set
            {
                if (SetProperty(ref _cornerRadius, value))
                    UpdatePreviewJson();
            }
        }

        public string PreviewJson
        {
            get => _previewJson;
            set => SetProperty(ref _previewJson, value);
        }

        public bool IsServerRunning
        {
            get => _isServerRunning;
            set => SetProperty(ref _isServerRunning, value);
        }

        public string ServerUrl
        {
            get => _serverUrl;
            set => SetProperty(ref _serverUrl, value);
        }

        public BitmapImage? QRCodeImage
        {
            get => _qrCodeImage;
            set => SetProperty(ref _qrCodeImage, value);
        }

        public ICommand AddElementCommand { get; }
        public ICommand RemoveElementCommand { get; }
        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }
        public ICommand GenerateJsonCommand { get; }
        public ICommand GenerateQRCommand { get; }
        public ICommand StartServerCommand { get; }
        public ICommand StopServerCommand { get; }
        public ICommand SaveJsonCommand { get; }
        public ICommand SelectElementCommand { get; }

        private void SelectElement(WidgetElement? element)
        {
            // Lehenik elementu guztiak desautatu
            foreach (var el in LayoutElements)
            {
                el.IsSelected = false;
            }
            
            // Klik egin zaion elementua hautatu
            if (element != null)
            {
                element.IsSelected = true;
            }
            
            SelectedElement = element;
        }

        private void InitializeWidgetSizes()
        {
            // Widget tamaina errealistagoak espazio errealean oinarrituta
            AvailableSizes.Add(new WidgetGridSize
            {
                Width = 2,
                Height = 1,
                DisplayName = "Oso Txikia (2x1)",
                Description = "Informazio oso oinarrizkoa",
                MaxElements = 4, // 4 unidades de espacio
                RecommendedElements = new List<string> { "Tenperatura ikonoarekin" }
            });

            AvailableSizes.Add(new WidgetGridSize
            {
                Width = 4,
                Height = 1,
                DisplayName = "Txikia Horizontala (4x1)",
                Description = "Lerro bakarreko informazioa",
                MaxElements = 8, // 8 unidades
                RecommendedElements = new List<string> { "Kokapena", "Tenperatura" }
            });

            AvailableSizes.Add(new WidgetGridSize
            {
                Width = 2,
                Height = 2,
                DisplayName = "Karratua Txikia (2x2)",
                Description = "Informazio oinarrizkoa bertikala",
                MaxElements = 8,
                RecommendedElements = new List<string> { "Kokapena", "Tenperatura", "Baldintza" }
            });

            AvailableSizes.Add(new WidgetGridSize
            {
                Width = 4,
                Height = 2,
                DisplayName = "Ertaina (4x2)",
                Description = "Informazio osoa oinarrizkoa",
                MaxElements = 16,
                RecommendedElements = new List<string> { "Kokapena", "Tenperatura", "Baldintza", "Hezetasuna", "Haizea" }
            });

            AvailableSizes.Add(new WidgetGridSize
            {
                Width = 4,
                Height = 3,
                DisplayName = "Handia (4x3)",
                Description = "Informazio zabala eguneko iragarpena",
                MaxElements = 24,
                RecommendedElements = new List<string> { "Kokapena", "Tenperatura", "Baldintza", "Hezetasuna", "Haizea", "Zatitzailea", "Eguneko Iragarpena (3 egun)" }
            });

            AvailableSizes.Add(new WidgetGridSize
            {
                Width = 4,
                Height = 4,
                DisplayName = "Oso Handia (4x4)",
                Description = "Informazio osoa detallatua",
                MaxElements = 32,
                RecommendedElements = new List<string> { "Kokapena", "Tenperatura", "Baldintza", "Hezetasuna", "Haizea", "Zatitzailea", "Eguneko Iragarpena (5 egun)", "Orduko Iragarpena" }
            });

            AvailableSizes.Add(new WidgetGridSize
            {
                Width = 4,
                Height = 5,
                DisplayName = "Extra Handia (4x5)",
                Description = "Pantaila osoko widgeta",
                MaxElements = 40,
                RecommendedElements = new List<string> { "Informazio guztia + Iragarpena osoa" }
            });
        }

        private int GetElementSize(string elementType)
        {
            // Elementu bakoitzak hartzen duen espazioa (unitateetan)
            return elementType switch
            {
                "location_name" => 2,              // Kokapenaren izena (2x0.5)
                "current_temp" => 4,              // Tenperatura handia espazio asko hartzen du (4x1)
                "current_condition_text" => 2,     // Baldintza testua (2x1)
                "humidity" => 2,                   // Hezetasuna (2x0.5)
                "wind_speed" => 2,                 // Haizea (2x0.5)
                "horizontal_divider" => 1,         // Zatitzailea (4x txikia)
                "daily_forecast_row" => 8,         // Eguneko iragarpena (4x2 gutxi gorabehera)
                "hourly_forecast" => 6,            // Orduko iragarpena (4x1.5)
                _ => 2
            };
        }

        private int CalculateTotalElementSize()
        {
            return LayoutElements.Sum(e => GetElementSize(e.Type));
        }

        private void AddElement(string? elementType)
        {
            if (string.IsNullOrEmpty(elementType)) return;

            var newElement = elementType switch
            {
                "location_name" => new WidgetElement { Type = "location_name", FontSize = 18, Alignment = "center" },
                "current_temp" => new WidgetElement { Type = "current_temp", FontSize = 48, Alignment = "center" },
                "current_condition_text" => new WidgetElement { Type = "current_condition_text", FontSize = 20, Alignment = "center" },
                "horizontal_divider" => new WidgetElement { Type = "horizontal_divider", Alignment = "center" },
                "daily_forecast_row" => new WidgetElement { Type = "daily_forecast_row", Days = 3, Alignment = "center" },
                "humidity" => new WidgetElement { Type = "humidity", FontSize = 16, Alignment = "center" },
                "wind_speed" => new WidgetElement { Type = "wind_speed", FontSize = 16, Alignment = "center" },
                "hourly_forecast" => new WidgetElement { Type = "hourly_forecast", Alignment = "center" },
                _ => new WidgetElement { Type = elementType, Alignment = "center" }
            };

            int elementSize = GetElementSize(elementType);
            int currentSize = CalculateTotalElementSize();
            int newTotalSize = currentSize + elementSize;

            // Elementua sartzen den balidatu
            if (newTotalSize > SelectedSize.MaxElements)
            {
                MessageBox.Show(
                    $"?? Ezin da elementua gehitu!\n\n" +
                    $"Widget tamaina: {SelectedSize.Width}x{SelectedSize.Height} ({SelectedSize.MaxElements} unitate)\n" +
                    $"Uneko okupazioa: {currentSize} unitate\n" +
                    $"Elementu honen tamaina: {elementSize} unitate\n" +
                    $"Gehigarria: {newTotalSize} unitate (gehiegi!)\n\n" +
                    $"?? Gomendapena: Hautatu tamaina handiagoa edo kendu beste elementu batzuk.",
                    Strings.CannotAddElement,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            LayoutElements.Add(newElement);
            OnPropertyChanged(nameof(CurrentElementsInfo));
            OnPropertyChanged(nameof(CanAddMoreElements));
            UpdatePreviewJson();
        }

        private void RemoveElement(WidgetElement? element)
        {
            if (element != null)
            {
                LayoutElements.Remove(element);
                if (SelectedElement == element)
                {
                    SelectedElement = null;
                }
                OnPropertyChanged(nameof(CurrentElementsInfo));
                OnPropertyChanged(nameof(CanAddMoreElements));
                OnPropertyChanged(nameof(UsedSpacePercentage));
                OnPropertyChanged(nameof(RemainingSpace));
                OnPropertyChanged(nameof(SpaceStatusColor));
                UpdatePreviewJson();
            }
        }

        private void MoveUp(WidgetElement? element)
        {
            if (element == null) return;

            int index = LayoutElements.IndexOf(element);
            if (index > 0)
            {
                LayoutElements.Move(index, index - 1);
                UpdatePreviewJson();
            }
        }

        private void MoveDown(WidgetElement? element)
        {
            if (element == null) return;

            int index = LayoutElements.IndexOf(element);
            if (index < LayoutElements.Count - 1)
            {
                LayoutElements.Move(index, index + 1);
                UpdatePreviewJson();
            }
        }

        private void GenerateJson()
        {
            UpdatePreviewJson();
            MessageBox.Show(Strings.JsonUpdatedMessage, Strings.JsonUpdated, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateQRWithJson()
        {
            try
            {
                var config = BuildConfiguration();
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions 
                { 
                    WriteIndented = false
                });

                var qrBitmap = _qrCodeService.GenerateQRCodeBitmap(json);
                QRCodeImage = BitmapHelper.ConvertBitmapToBitmapImage(qrBitmap);
                
                ServerUrl = $"JSON QR kodean txertatuta ({json.Length} karaktere)";
                IsServerRunning = false;
                
                MessageBox.Show(
                    string.Format(Strings.QRGeneratedMessage, json.Length),
                    Strings.QRGenerated,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(Strings.QRErrorMessage, ex.Message),
                    Strings.QRError,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                
                QRCodeImage = null;
                ServerUrl = string.Empty;
            }
        }

        private async Task StartServerAsync()
        {
            try
            {
                var config = BuildConfiguration();
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

                string url = await _serverService.StartServer(json);
                ServerUrl = url;
                IsServerRunning = true;

                var qrBitmap = _qrCodeService.GenerateQRCodeBitmap(url);
                QRCodeImage = BitmapHelper.ConvertBitmapToBitmapImage(qrBitmap);
                
                MessageBox.Show(
                    string.Format(Strings.ServerStartedMessage, url),
                    Strings.ServerStarted,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (System.Net.HttpListenerException ex) when (ex.ErrorCode == 5)
            {
                MessageBox.Show(
                    string.Format(Strings.AccessDeniedMessage, ex.Message),
                    Strings.AccessDenied,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                
                IsServerRunning = false;
                ServerUrl = string.Empty;
                QRCodeImage = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{Strings.QRError}:\n\n{ex.Message}",
                    "Zerbitzari Errorea",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                
                IsServerRunning = false;
                ServerUrl = string.Empty;
                QRCodeImage = null;
            }
        }

        private void StopServer()
        {
            try
            {
                _serverService.StopServer();
                IsServerRunning = false;
                ServerUrl = string.Empty;
                QRCodeImage = null;
                MessageBox.Show(Strings.ServerStoppedMessage, Strings.ServerStopped, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea zerbitzaria gelditzean:\n{ex.Message}", "Errorea", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveJson()
        {
            try
            {
                var config = BuildConfiguration();
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "JSON fitxategiak (*.json)|*.json",
                    FileName = $"{WidgetId}.json",
                    DefaultExt = ".json"
                };

                if (dialog.ShowDialog() == true)
                {
                    System.IO.File.WriteAllText(dialog.FileName, json);
                    MessageBox.Show(
                        string.Format(Strings.JsonSavedMessage, dialog.FileName),
                        Strings.JsonSaved,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(Strings.JsonSaveErrorMessage, ex.Message),
                    Strings.JsonSaveError,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void UpdatePreviewJson()
        {
            try
            {
                var config = BuildConfiguration();
                PreviewJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                PreviewJson = $"Errorea JSONa sortzean: {ex.Message}";
            }
        }

        private WidgetConfiguration BuildConfiguration()
        {
            return new WidgetConfiguration
            {
                WidgetId = WidgetId,
                Size = new WidgetSize
                {
                    Width = "match_parent",
                    Height = "wrap_content",
                    GridWidth = SelectedSize.Width,
                    GridHeight = SelectedSize.Height
                },
                Background = new WidgetBackground
                {
                    Type = "glassmorphism",
                    Color = BackgroundColor,
                    CornerRadius = CornerRadius
                },
                Layout = new List<WidgetElement>(LayoutElements)
            };
        }

        private void ExtractColorComponents(string hexColor)
        {
            if (string.IsNullOrEmpty(hexColor) || hexColor.Length < 7)
                return;

            try
            {
                if (hexColor.StartsWith("#") && hexColor.Length >= 9)
                {
                    // Alpha atera eta ehunekora bihurtu
                    string alpha = hexColor.Substring(1, 2);
                    int alphaValue = Convert.ToInt32(alpha, 16);
                    _backgroundOpacity = (int)((alphaValue / 255.0) * 100);
                    OnPropertyChanged(nameof(BackgroundOpacity));
                    OnPropertyChanged(nameof(OpacityPercentage));

                    // RGB atera
                    _baseColor = "#" + hexColor.Substring(3);
                    OnPropertyChanged(nameof(BaseColor));
                }
                else if (hexColor.StartsWith("#") && hexColor.Length == 7)
                {
                    _baseColor = hexColor;
                    OnPropertyChanged(nameof(BaseColor));
                }
            }
            catch
            {
                // Analisia huts egiten badu, uneko balioak mantendu
            }
        }

        private void UpdateBackgroundColorFromComponents()
        {
            try
            {
                // Gardentasun ehunekoa hex alpha-ra bihurtu
                int alphaValue = (int)((BackgroundOpacity / 100.0) * 255);
                string alpha = alphaValue.ToString("X2");

                // # kendu oinarrizko koloretik badago
                string rgb = BaseColor.TrimStart('#');

                // Ziurtatu RGB 6 karaktere dituela
                if (rgb.Length != 6)
                    rgb = "FFFFFF";

                _backgroundColor = $"#{alpha}{rgb}";
                OnPropertyChanged(nameof(BackgroundColor));
                UpdatePreviewJson();
            }
            catch
            {
                // Bihurketak huts egiten badu, lehenetsia erabili
                _backgroundColor = "#33FFFFFF";
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }
    }
}
