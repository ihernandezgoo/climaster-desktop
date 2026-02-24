using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace climaster.Converters;

// Gradiente string-a LinearGradientBrush-era bihurtzen du
public class GradientStringToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string gradientString)
        {
            var colors = gradientString.Split(',').Select(c => c.Trim()).ToArray();
            if (colors.Length >= 2)
            {
                var gradientStops = new GradientStopCollection();
                
                for (int i = 0; i < colors.Length; i++)
                {
                    var color = (Color)ColorConverter.ConvertFromString(colors[i]);
                    gradientStops.Add(new GradientStop(color, i / (double)(colors.Length - 1)));
                }

                return new LinearGradientBrush(gradientStops, new System.Windows.Point(0, 0), new System.Windows.Point(0, 1));
            }
        }

        return new LinearGradientBrush(Colors.SkyBlue, Colors.DodgerBlue, 90);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// Unix timestamp-a data eta ordu formatuera bihurtzen du
public class UnixTimestampToDateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is long timestamp)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
            var format = parameter as string ?? "HH:mm";
            return dateTime.ToString(format);
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// Boolean balioa Visibility-ra bihurtzen du
public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
        return System.Windows.Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// Boolean balioa alderantzikatu egiten du
public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return true;
    }
}

// Tenperatura balioa kolorera bihurtzen du (hotzik/beroa)
public class TemperatureToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string tempString && double.TryParse(tempString.Replace("°C", "").Trim(), out double temp))
        {
            // Hotzik: urdina, Ertaina: berdea, Beroa: gorria
            if (temp < 10) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498db")); // Urdina
            if (temp < 20) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ecc71")); // Berdea
            if (temp < 30) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f39c12")); // Laranja
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e74c3c")); // Gorria
        }
        return new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// Eguraldiaren ikono kodea emoji batera bihurtzen du
public class WeatherIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string iconCode)
        {
            return iconCode switch
            {
                "01d" => "??",  // Zeru garbia egunez
                "01n" => "??",  // Zeru garbia gauez
                "02d" => "?",  // Hodei gutxi egunez
                "02n" => "??",  // Hodei gutxi gauez
                "03d" or "03n" => "??",  // Hodei sakabanatua
                "04d" or "04n" => "??",  // Hodei haustuak
                "09d" or "09n" => "???",  // Euri jasa
                "10d" => "???",  // Euria egunez
                "10n" => "???",  // Euria gauez
                "11d" or "11n" => "??",  // Ekaitza
                "13d" or "13n" => "??",  // Elurra
                "50d" or "50n" => "???",  // Lainoa
                _ => "???"
            };
        }
        return "???";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// Zenbakizko balioa ehunekora bihurtzen du
public class NumberToPercentageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return $"{intValue}%";
        }
        if (value is double doubleValue)
        {
            return $"{doubleValue:F0}%";
        }
        return "0%";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
