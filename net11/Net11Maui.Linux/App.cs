using Microsoft.Maui.Controls;

namespace Net11Maui.Linux;

public class App : Application
{
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage());
    }
}
