namespace Net11Maui;

// Built in C#: with EnablePreviewFeatures the RC2 XAML source generator cannot assign <TabBar> children
public class AppShell : Shell
{
    public AppShell()
    {
        Title = "Net11Maui";
        Items.Add(new TabBar
        {
            Items =
            {
                new ShellContent { Title = "Runtime and controls", Route = "MainPage", ContentTemplate = new DataTemplate(typeof(MainPage)) },
                new ShellContent { Title = "C# expressions", Route = "ExpressionsPage", ContentTemplate = new DataTemplate(typeof(ExpressionsPage)) }
            }
        });
    }
}
