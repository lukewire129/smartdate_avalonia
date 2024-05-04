using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace SmartDateControl.Themes.Units;

public class CalendarBoxItem : ListBoxItem
{
    public string DateFormat { get; set; }

    public DateTime Date;

    private bool _isCurrentMonth;
    public bool IsCurrentMonth
    {
        get { return (bool)GetValue(IsCurrentMonthProperty); }
        set { SetValue(IsCurrentMonthProperty, value); }
    }

    public static readonly AvaloniaProperty IsCurrentMonthProperty =
        AvaloniaProperty.Register<CalendarBoxItem, bool>(
            nameof(IsCurrentMonth));
}