using Microsoft.Maui.Platforms.Linux.Gtk4.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Platforms.Linux.Gtk4.Essentials.Hosting;

namespace Net11Maui.Linux;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp
            .CreateBuilder()
            .UseMauiAppLinuxGtk4<App>();

        builder.AddLinuxGtk4Essentials();

        return builder.Build();
    }
}
