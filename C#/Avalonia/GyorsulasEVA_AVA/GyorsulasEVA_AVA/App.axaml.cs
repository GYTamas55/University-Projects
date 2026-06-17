using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GyorsulasEVA_AVA.Model;
using GyorsulasEVA_AVA.ViewModels;
using GyorsulasEVA_AVA.Views;

namespace GyorsulasEVA_AVA;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Game mainGame = new Game();
            MainViewModel viewModel = new MainViewModel(mainGame);
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel,
            };
        }
        base.OnFrameworkInitializationCompleted();
    }
}
