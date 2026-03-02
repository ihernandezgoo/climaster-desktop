using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Climaster_feature.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Climaster_feature.Views
{
    public partial class AndroidWidgetPreview : UserControl
    {
        public static readonly DependencyProperty LayoutElementsProperty =
            DependencyProperty.Register(nameof(LayoutElements), typeof(ObservableCollection<WidgetElement>),
                typeof(AndroidWidgetPreview), new PropertyMetadata(null, OnLayoutChanged));

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(string),
                typeof(AndroidWidgetPreview), new PropertyMetadata("#33FFFFFF", OnLayoutChanged));

        public static readonly DependencyProperty CornerRadiusValueProperty =
            DependencyProperty.Register(nameof(CornerRadiusValue), typeof(int),
                typeof(AndroidWidgetPreview), new PropertyMetadata(24, OnLayoutChanged));

        private ObservableCollection<WidgetElement>? _previousCollection;

        public AndroidWidgetPreview()
        {
            InitializeComponent();
        }

        public ObservableCollection<WidgetElement>? LayoutElements
        {
            get => (ObservableCollection<WidgetElement>?)GetValue(LayoutElementsProperty);
            set => SetValue(LayoutElementsProperty, value);
        }

        public string BackgroundColor
        {
            get => (string)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public int CornerRadiusValue
        {
            get => (int)GetValue(CornerRadiusValueProperty);
            set => SetValue(CornerRadiusValueProperty, value);
        }

        private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AndroidWidgetPreview preview)
            {
                // Unsubscribe from previous collection and its items
                if (preview._previousCollection != null)
                {
                    preview._previousCollection.CollectionChanged -= preview.OnCollectionChanged;
                    foreach (var item in preview._previousCollection)
                    {
                        item.PropertyChanged -= preview.OnElementPropertyChanged;
                    }
                }

                // Subscribe to new collection and its items
                if (e.NewValue is ObservableCollection<WidgetElement> newCollection)
                {
                    newCollection.CollectionChanged += preview.OnCollectionChanged;
                    foreach (var item in newCollection)
                    {
                        item.PropertyChanged += preview.OnElementPropertyChanged;
                    }
                    preview._previousCollection = newCollection;
                }

                preview.RenderWidget();
            }
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Subscribe to new items
            if (e.NewItems != null)
            {
                foreach (WidgetElement item in e.NewItems)
                {
                    item.PropertyChanged += OnElementPropertyChanged;
                }
            }

            // Unsubscribe from old items
            if (e.OldItems != null)
            {
                foreach (WidgetElement item in e.OldItems)
                {
                    item.PropertyChanged -= OnElementPropertyChanged;
                }
            }

            // Re-render when collection changes
            RenderWidget();
        }

        private void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Re-render when any element property changes
            // Ignore IsSelected changes as they don't affect rendering
            if (e.PropertyName != nameof(WidgetElement.IsSelected))
            {
                RenderWidget();
            }
        }

        private void RenderWidget()
        {
            ElementsContainer.Children.Clear();

            if (LayoutElements == null) return;

            // Set background
            try
            {
                var converter = new BrushConverter();
                WidgetContainer.Background = (Brush)converter.ConvertFrom(BackgroundColor)!;
                WidgetContainer.CornerRadius = new CornerRadius(CornerRadiusValue);
            }
            catch
            {
                WidgetContainer.Background = new SolidColorBrush(Color.FromArgb(51, 255, 255, 255));
            }

            // Render elements
            foreach (var element in LayoutElements)
            {
                var control = CreateElementControl(element);
                if (control != null)
                {
                    ElementsContainer.Children.Add(control);
                }
            }
        }

        private UIElement? CreateElementControl(WidgetElement element)
        {
            return element.Type switch
            {
                "current_temp" => CreateTempControl(element),
                "current_condition_text" => CreateConditionControl(element),
                "horizontal_divider" => CreateDivider(),
                "daily_forecast_row" => CreateDailyForecast(element),
                "humidity" => CreateInfoControl("??", "Humedad: 65%", element),
                "wind_speed" => CreateInfoControl("???", "Viento: 15 km/h", element),
                "hourly_forecast" => CreateHourlyForecast(),
                _ => null
            };
        }

        private TextBlock CreateTempControl(WidgetElement element)
        {
            var tb = new TextBlock
            {
                Text = "24°",
                FontSize = element.FontSize ?? 48,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Margin = new Thickness(0, 10, 0, 10)
            };

            if (element.Alignment == "center")
                tb.HorizontalAlignment = HorizontalAlignment.Center;
            else if (element.Alignment == "right")
                tb.HorizontalAlignment = HorizontalAlignment.Right;

            return tb;
        }

        private TextBlock CreateConditionControl(WidgetElement element)
        {
            var tb = new TextBlock
            {
                Text = "Parcialmente nublado",
                FontSize = element.FontSize ?? 20,
                Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                Margin = new Thickness(0, 0, 0, 10)
            };

            if (element.Alignment == "center")
                tb.HorizontalAlignment = HorizontalAlignment.Center;
            else if (element.Alignment == "right")
                tb.HorizontalAlignment = HorizontalAlignment.Right;

            return tb;
        }

        private Border CreateDivider()
        {
            return new Border
            {
                Height = 1,
                Background = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255)),
                Margin = new Thickness(0, 10, 0, 10)
            };
        }

        private StackPanel CreateInfoControl(string emoji, string text, WidgetElement element)
        {
            var stack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 5, 0, 5)
            };

            stack.Children.Add(new TextBlock
            {
                Text = $"{emoji} {text}",
                FontSize = element.FontSize ?? 16,
                Foreground = Brushes.White
            });

            return stack;
        }

        private Grid CreateDailyForecast(WidgetElement element)
        {
            var grid = new Grid { Margin = new Thickness(0, 10, 0, 10) };
            int days = element.Days ?? 3;

            for (int i = 0; i < days; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            string[] dayNames = { "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb", "Dom" };
            string[] icons = { "??", "?", "??" };

            for (int i = 0; i < days; i++)
            {
                var dayStack = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                dayStack.Children.Add(new TextBlock
                {
                    Text = dayNames[i % 7],
                    Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                dayStack.Children.Add(new TextBlock
                {
                    Text = icons[i % 3],
                    FontSize = 24,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 5)
                });

                dayStack.Children.Add(new TextBlock
                {
                    Text = $"{22 + i}°",
                    Foreground = Brushes.White,
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                Grid.SetColumn(dayStack, i);
                grid.Children.Add(dayStack);
            }

            return grid;
        }

        private ScrollViewer CreateHourlyForecast()
        {
            var scroll = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Margin = new Thickness(0, 10, 0, 10)
            };

            var stack = new StackPanel { Orientation = Orientation.Horizontal };

            for (int i = 0; i < 6; i++)
            {
                var hourStack = new StackPanel
                {
                    Margin = new Thickness(0, 0, 15, 0)
                };

                hourStack.Children.Add(new TextBlock
                {
                    Text = $"{12 + i}:00",
                    Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                    FontSize = 11,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                hourStack.Children.Add(new TextBlock
                {
                    Text = i % 2 == 0 ? "??" : "??",
                    FontSize = 20,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 5)
                });

                hourStack.Children.Add(new TextBlock
                {
                    Text = $"{23 + i}°",
                    Foreground = Brushes.White,
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                stack.Children.Add(hourStack);
            }

            scroll.Content = stack;
            return scroll;
        }
    }
}
