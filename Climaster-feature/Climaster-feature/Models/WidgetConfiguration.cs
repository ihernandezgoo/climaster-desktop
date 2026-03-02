using System.Text.Json.Serialization;

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

    public class WidgetElement
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("fontSize")]
        public int? FontSize { get; set; }

        [JsonPropertyName("alignment")]
        public string? Alignment { get; set; }

        [JsonPropertyName("days")]
        public int? Days { get; set; }
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
