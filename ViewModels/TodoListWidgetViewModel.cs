using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using TodoList.Models;
using TodoList.Services.Interfaces;

namespace TodoList.ViewModels;

public partial class TodoListWidgetViewModel : ViewModelBase
{
    public ObservableCollection<TodoItem> Todos { get; } = [];

    [RelayCommand]
    private async Task AddTodoAsync()
    {
        var newTodo = new TodoItem { Title = "New Todo" };
        var dialogService = Ioc.Default.GetService<IDialogService>()!;
        var todoEditorViewModel = new TodoEditorViewModel(newTodo);

        var windowsResult = await dialogService.ShowDialogAsync(todoEditorViewModel);
        if (windowsResult == true)
        {
            var resultTodo = todoEditorViewModel.GetResult();
            Todos.Add(resultTodo);
        }
    }
}