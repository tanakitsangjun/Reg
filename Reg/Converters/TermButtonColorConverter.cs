using System.Globalization;
using Microsoft.Maui.Graphics;

namespace Reg.Converters;

public class TermButtonColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int buttonTerm && parameter is int selectedTerm)
        {
            return buttonTerm == selectedTerm ? 
                Color.FromArgb("#FFD700") : // Selected: Gold
                Color.FromArgb("#7F8C8D"); // Unselected: Gray
        }
        return Color.FromArgb("#7F8C8D");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
