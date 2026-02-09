using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TodoList.Services.Interfaces;
using TodoList.ViewModels.Interfaces;

namespace TodoList.Services;

public class DialogService(Window mainWindow, IServiceProvider serviceProvider) : IDialogService
{
    public async Task<(bool? Result, TViewModel ViewModel)> ShowDialogAsync<TViewModel>()
        where TViewModel : IDialogRequestClose
    {
        var vm = serviceProvider.GetRequiredService<TViewModel>();
        var result = await ShowDialogInternalAsync(vm);
        return (result, vm);
    }

    public async Task<(bool? Result, TViewModel ViewModel)> ShowDialogAsync<TViewModel, TArg>(TArg arg)
        where TViewModel : IDialogRequestClose, IDialogInitialize<TArg>
    {
        var vm = serviceProvider.GetRequiredService<TViewModel>();
        vm.Initialize(arg);
        var result = await ShowDialogInternalAsync(vm);
        return (result, vm);
    }

    private async Task<bool?> ShowDialogInternalAsync<TViewModel>(TViewModel vm) where TViewModel : IDialogRequestClose
    {
        var window = new Window
        {
            Content = vm,
            DataContext = vm,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        TaskCompletionSource<bool?> tcs = new();

        void OnRequestClose(bool? result)
        {
            vm.RequestClose -= OnRequestClose;
            window.Close(result);
            tcs.SetResult(result);
        }

        vm.RequestClose += OnRequestClose;

        await window.ShowDialog<bool?>(mainWindow);
        return await tcs.Task;
    }
}