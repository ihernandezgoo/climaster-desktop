using System.Text.Json.Serialization;
using Climaster_feature.ViewModels;

namespace Climaster_feature.Models
{
    public class WidgetConfiguration
    {
        [JsonPropertyName("widgetId")]
        public string WidgetId { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public WidgetSize Size { get; set; } = new();

        [JsonPropertyName("background")]
        public WidgetBackground Background { get; set; } = new();

        [JsonPropertyName("layout")]
        public List<WidgetElement> Layout { get; set; } = new();
    }

    public class WidgetSize
    {
        [JsonPropertyName("width")]
        public string Width { get; set; } = "match_parent";

        [JsonPropertyName("height")]
        public string Height { get; set; } = "wrap_content";

        [JsonPropertyName("gridWidth")]
        public int GridWidth { get; set; } = 4;

        [JsonPropertyName("gridHeight")]
        public int GridHeight { get; set; } = 2;
    }

    public class WidgetBackground
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "glassmorphism";

        [JsonPropertyName("color")]
        public string Color { get; set; } = "#33FFFFFF";

        [JsonPropertyName("cornerRadius")]
        public int CornerRadius { get; set; } = 24;
    }

    public class WidgetElement : ViewModelBase
    {
        private string _type = string.Empty;
        private int? _fontSize;
        private string? _alignment;
        private int? _days;
        private bool _isSelected;

        [JsonPropertyName("type")]
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        [JsonPropertyName("fontSize")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? FontSize
        {
            get => _fontSize;
            set
            {
                if (SetProperty(ref _fontSize, value))
                {
                    OnPropertyChanged(nameof(HasFontSize));
                }
            }
        }

        [JsonPropertyName("alignment")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Alignment
        {
            get => _alignment;
            set
            {
                if (SetProperty(ref _alignment, value))
                {
                    OnPropertyChanged(nameof(HasAlignment));
                }
            }
        }

        [JsonPropertyName("days")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Days
        {
            get => _days;
            set
            {
                if (SetProperty(ref _days, value))
                {
                    OnPropertyChanged(nameof(HasDays));
                }
            }
        }

        // UI-only property
        [JsonIgnore]
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        // Propiedades auxiliares para UI
        [JsonIgnore]
        public bool HasFontSize => FontSize.HasValue;

        [JsonIgnore]
        public bool HasAlignment => !string.IsNullOrEmpty(Alignment);

        [JsonIgnore]
        public bool HasDays => Days.HasValue;

        [JsonIgnore]
        public List<string> AvailableAlignments { get; } = new() { "left", "center", "right" };
    }

    // Clase auxiliar para tamaños predefinidos
    public class WidgetGridSize
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MaxElements { get; set; }
        public List<string> RecommendedElements { get; set; } = new();

        public override string ToString() => DisplayName;
    }
}
