using System.Runtime.InteropServices;

namespace Net11Maui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Mono exposes the Mono.RuntimeStructs type; CoreCLR does not
        var runtime = Type.GetType("Mono.RuntimeStructs") is null ? "CoreCLR" : "Mono";
        RuntimeLabel.Text = $"{RuntimeInformation.FrameworkDescription} on {runtime} ({DeviceInfo.Platform})";
    }

    int _longPresses;

    void OnLongPressed(object? sender, LongPressedEventArgs e)
    {
        _longPresses++;
        LongPressLabel.Text = $"Long presses: {_longPresses}";
    }
}
