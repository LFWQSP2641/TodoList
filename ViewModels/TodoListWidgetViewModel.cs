using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TodoList.Models;
using TodoList.Services.Interfaces;

namespace TodoList.ViewModels;

public partial class TodoListWidgetViewModel : ViewModelBase
{
    public ObservableCollection<TodoItem> Todos { get; } = [];

    public IReadOnlyList<TodoLevelItem> LevelOptions { get; } =
    [
        new TodoLevelItem(TodoLevel.Low,    "低"),
        new TodoLevelItem(TodoLevel.Medium, "中"),
        new TodoLevelItem(TodoLevel.High,   "高")
    ];

    [ObservableProperty]
    public partial TodoItem? SelectedTodo { get; set; }

    private bool CanEditSelectedTodo => SelectedTodo != null;

    [RelayCommand]
    private async Task AddTodoAsync()
    {
        var newTodo = new TodoItem { Title = "New Todo" };
        var dialogService = Ioc.Default.GetService<IDialogService>()!;
        var dialogResult = await dialogService.ShowDialogAsync<TodoEditorViewModel, TodoItem, TodoItem>(newTodo);
        if (dialogResult is { Result: true, Payload: not null })
        {
            Todos.Add(dialogResult.Payload);
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
        var dialogResult = await dialogService.ShowDialogAsync<TodoEditorViewModel, TodoItem, TodoItem>(SelectedTodo);
        if (dialogResult is { Result: true, Payload: not null })
        {
            var index = Todos.IndexOf(SelectedTodo);
            if (index >= 0)
            {
                Todos[index] = dialogResult.Payload;
                SelectedTodo = dialogResult.Payload;
            }
        }
    }

    public class TodoLevelItem(TodoLevel value, string displayName)
    {
        public TodoLevel Value { get; } = value;
        public string DisplayName { get; } = displayName;

        public override string ToString() => DisplayName;
    }
}