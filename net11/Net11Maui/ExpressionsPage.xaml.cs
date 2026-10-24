using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Net11Maui;

public partial class ExpressionsPage : ContentPage
{
    public ExpressionsPage()
    {
        InitializeComponent();
        BindingContext = new ProductViewModel();
    }
}

public class ProductViewModel : INotifyPropertyChanged
{
    public string ProductName { get; } = "Laptop";
    public double Price { get; } = 1499.99;

    public double Quantity
    {
        get;
        set { field = value; OnPropertyChanged(); }
    } = 1;

    public bool IsActive
    {
        get;
        set { field = value; OnPropertyChanged(); }
    } = true;

    public event PropertyChangedEventHandler? PropertyChanged;

    void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
