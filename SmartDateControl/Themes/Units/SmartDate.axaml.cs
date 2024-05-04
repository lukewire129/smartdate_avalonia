using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SmartDateControl.Themes.Units;

public class SmartDate : ContentControl
{
    private Popup _popup;
    private CalendarSwitch _switch;
    private CalendarBox _listbox;

    private bool _keepPopupOpen;

    public bool KeepPopupOpen
    {
        get { return (bool)GetValue(KeepPopupOpenProperty); }
        set { SetValue(KeepPopupOpenProperty, value); }
    }

    public static readonly AvaloniaProperty KeepPopupOpenProperty =
        AvaloniaProperty.Register<SmartDate, bool>(
            nameof(KeepPopupOpen));


    private DateTime _currentMonth;

    public DateTime CurrentMonth
    {
        get { return (DateTime)GetValue(CurrentMonthProperty); }
        set { SetValue(CurrentMonthProperty, value); }
    }

    public static readonly AvaloniaProperty CurrentMonthProperty =
        AvaloniaProperty.Register<SmartDate, DateTime>(
            nameof(CurrentMonth));


    private DateTime? _selectedDate;

    public DateTime? SelectedDate
    {
        get { return (DateTime?)GetValue(SelectedDateProperty); }
        set { SetValue(SelectedDateProperty, value); }
    }

    public static readonly AvaloniaProperty? SelectedDateProperty =
        AvaloniaProperty.Register<SmartDate, DateTime?>(
            nameof(SelectedDate));


    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _switch = e.NameScope.Get<CalendarSwitch>("PART_Switch");
        _switch.Click += _switch_Click;
        
        _popup = e.NameScope.Get<Popup>("PART_Popup");
        _listbox = e.NameScope.Get<CalendarBox>("PART_ListBox");
        ChevronButton leftButton = e.NameScope.Get<ChevronButton>("PART_Left");
        ChevronButton rightButton = e.NameScope.Get<ChevronButton>("PART_Right");
        
        _listbox.PointerReleased += _listbox_MouseLeftButtonUp;
        
        leftButton.Click += (s, e) => MoveMonthClick(-1);
        rightButton.Click += (s, e) => MoveMonthClick(1);
    }

    private void MoveMonthClick(int month)
    {
        GenerateCalendar(CurrentMonth.AddMonths(month));
    }

    private void _popup_Closed(object sender, EventArgs e)
    {
        _switch.IsChecked = IsPointerOver;
    }

    private void _switch_Click(object sender, RoutedEventArgs e)
    {
        if (_switch.IsChecked == true)
        {
            GenerateCalendar(SelectedDate ?? DateTime.Now);
            if (SelectedDate is not null)
            {
                var item = _listbox.Items.FirstOrDefault(x => ((CalendarBoxItem)x).Date == SelectedDate);
                ((ListBoxItem)item).IsSelected = true;
            }
        }
    }

    private void _listbox_MouseLeftButtonUp(object sender, PointerReleasedEventArgs e)
    {
        if (_listbox.SelectedItem is CalendarBoxItem selected)
        {
            SelectedDate = selected.Date;
            GenerateCalendar(selected.Date);

             _switch.IsChecked = KeepPopupOpen;
        }
    }

    private void GenerateCalendar(DateTime current)
    {
        if (current.ToString("yyyyMM") == CurrentMonth.ToString("yyyyMM")) return;

        CurrentMonth = current;
        _listbox.Items.Clear();
        DateTime fDayOfMonth = new(current.Year, current.Month, 1);
        DateTime lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

        int fOffset = (int)fDayOfMonth.DayOfWeek;
        int lOffset = 6 - (int)lDayOfMonth.DayOfWeek;

        DateTime fDay = fDayOfMonth.AddDays(-fOffset);
        DateTime lDay = lDayOfMonth.AddDays(lOffset);

        for (DateTime day = fDay; day <= lDay; day = day.AddDays(1))
        {
            CalendarBoxItem boxItem = new();
            boxItem.Date = day;
            boxItem.DateFormat = day.ToString("yyyyMMdd");
            boxItem.Content = day.Day;
            boxItem.IsCurrentMonth = day.Month == current.Month;

            _listbox.Items.Add(boxItem);
        }

        if (SelectedDate != null)
        {
            _listbox.SelectedValue = SelectedDate.Value.ToString("yyyyMMdd");
        }
    }
}