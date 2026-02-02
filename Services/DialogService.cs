using Avalonia.Controls;
using System.Threading.Tasks;
using TodoList.Services.Interfaces;
using TodoList.ViewModels.Interfaces;

namespace TodoList.Services;

public class DialogService(Window mainWindow) : IDialogService
{
    public async Task<bool?> ShowDialogAsync(object vm)
    {
        var window = new Window
        {
            Content = vm,
            DataContext = vm,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        var result = await window.ShowDialog<bool?>(mainWindow);
        return result;
    }

    public async Task<bool?> ShowDialogAsync<T>(T vm) where T : IDialogRequestClose
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