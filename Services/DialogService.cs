using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TodoList.Services.Interfaces;
using TodoList.ViewModels.Interfaces;

namespace TodoList.Services;

public class DialogService(Window mainWindow, IServiceProvider serviceProvider) : IDialogService
{
    public async Task<DialogResult<TResult>> ShowDialogAsync<TViewModel, TResult>()
        where TViewModel : IDialogRequestClose, IDialogResultProvider<TResult>
    {
        var vm = serviceProvider.GetRequiredService<TViewModel>();
        var result = await ShowDialogInternalAsync(vm);
        return new DialogResult<TResult>(result, vm.GetResult());
    }

    public async Task<DialogResult<TResult>> ShowDialogAsync<TViewModel, TArg, TResult>(TArg arg)
        where TViewModel : IDialogRequestClose, IDialogInitialize<TArg>, IDialogResultProvider<TResult>
    {
        var vm = serviceProvider.GetRequiredService<TViewModel>();
        vm.Initialize(arg);
        var result = await ShowDialogInternalAsync(vm);
        return new DialogResult<TResult>(result, vm.GetResult());
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