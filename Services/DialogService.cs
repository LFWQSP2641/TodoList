using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using TodoList.Services.Interfaces;
using TodoList.ViewModels.Interfaces;

namespace TodoList.Services;

public class DialogService(IClassicDesktopStyleApplicationLifetime desktop)
    : IDialogService
{
    public async Task<bool?> ShowDialogAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IDialogRequestClose
    {
        var result = await ShowDialogInternalAsync(viewModel);
        return result;
    }

    private async Task<bool?> ShowDialogInternalAsync<TViewModel>(TViewModel vm) where TViewModel : IDialogRequestClose
    {
        var owner = desktop.Windows.FirstOrDefault(w => w.IsActive)
                    ?? desktop.MainWindow
                    ?? throw new InvalidOperationException("No active window found and MainWindow is not initialized.");

        var window = new Window
        {
            Content = vm,
            DataContext = vm,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        TaskCompletionSource<bool?> tcs = new();

        vm.RequestClose += OnRequestClose;

        await window.ShowDialog<bool?>(owner);
        return await tcs.Task;

        void OnRequestClose(bool? result)
        {
            vm.RequestClose -= OnRequestClose;
            window.Close(result);
            tcs.SetResult(result);
        }
    }
}
