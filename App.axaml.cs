using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
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
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            var services = new ServiceCollection();
            services.AddSingleton<Window>(_ => desktop.MainWindow!);
            services.AddSingleton<IDataTemplate>(locator);
            ConfigureServices(services);
            ConfigureViewModels(services);
            ConfigureViews(services);
            var provider = services.BuildServiceProvider();
            Ioc.Default.ConfigureServices(provider);

            var vm = Ioc.Default.GetService<MainWindowViewModel>();
            var view = (Window)locator.Build(vm)!;
            view.DataContext = vm;

            desktop.MainWindow = view;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
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