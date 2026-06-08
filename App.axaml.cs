using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Services;
using TodoList.Services.Interfaces;
using TodoList.ViewModels;
using TodoList.Views;

namespace TodoList;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var locator = new ViewLocator();
        DataTemplates.Add(locator);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = new ServiceCollection();
            services.AddSingleton(desktop);
            services.AddSingleton<IDataTemplate>(locator);
            ConfigureServices(services);
            ConfigureViewModels(services);
            ConfigureViews(services);
            var provider = services.BuildServiceProvider();
            Ioc.Default.ConfigureServices(provider);

            var vm = Ioc.Default.GetRequiredService<MainWindowViewModel>();
            var view = Ioc.Default.GetRequiredService<MainWindowView>();
            view.DataContext = vm;

            desktop.MainWindow = view;
        }

        base.OnFrameworkInitializationCompleted();
    }

    [Singleton(typeof(DialogService), typeof(IDialogService))]
    internal static partial void ConfigureServices(IServiceCollection services);

    [Singleton(typeof(MainWindowViewModel))]
    [Transient(typeof(TodoEditorViewModel))]
    [Singleton(typeof(TodoListWidgetViewModel))]
    internal static partial void ConfigureViewModels(IServiceCollection services);

    [Singleton(typeof(MainWindowView))]
    [Transient(typeof(TodoEditorView))]
    [Singleton(typeof(TodoListWidgetView))]
    internal static partial void ConfigureViews(IServiceCollection services);
}
