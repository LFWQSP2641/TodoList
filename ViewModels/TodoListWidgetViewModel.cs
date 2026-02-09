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

    [ObservableProperty]
    public partial TodoItem? SelectedTodo { get; set; }

    private bool CanEditSelectedTodo => SelectedTodo != null;

    [RelayCommand]
    private async Task AddTodoAsync()
    {
        var newTodo = new TodoItem { Title = "New Todo" };
        var dialogService = Ioc.Default.GetService<IDialogService>()!;
        var (result, todoEditorViewModel) = await dialogService.ShowDialogAsync<TodoEditorViewModel, TodoItem>(newTodo);
        if (result == true)
        {
            var resultTodo = todoEditorViewModel.GetResult();
            Todos.Add(resultTodo);
        }
    }

    [RelayCommand(CanExecute = nameof(CanEditSelectedTodo))]
    private void RemoveTodo()
    {
        if (SelectedTodo != null)
        {
            Todos.Remove(SelectedTodo);
        }
    }

    [RelayCommand(CanExecute = nameof(CanEditSelectedTodo))]
    private async Task EditTodoAsync()
    {
        if (SelectedTodo == null) return;
        var dialogService = Ioc.Default.GetService<IDialogService>()!;
        var (result, todoEditorViewModel) = await dialogService.ShowDialogAsync<TodoEditorViewModel, TodoItem>(SelectedTodo);
        if (result == true)
        {
            var resultTodo = todoEditorViewModel.GetResult();
            var index = Todos.IndexOf(SelectedTodo);
            if (index >= 0)
            {
                Todos[index] = resultTodo;
                SelectedTodo = resultTodo;
            }
        }
    }
}