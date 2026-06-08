using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using TodoList.Models;
using TodoList.Services.Interfaces;

namespace TodoList.ViewModels;

public partial class TodoListWidgetViewModel : ViewModelBase
{
    public ObservableCollection<TodoItem> Todos { get; } = [];

    public IReadOnlyList<TodoLevelItem> LevelOptions { get; } =
    [
        new(TodoLevel.Low, "低"),
        new(TodoLevel.Medium, "中"),
        new(TodoLevel.High, "高"),
    ];

    [ObservableProperty] public partial TodoItem? SelectedTodo { get; set; }

    private bool CanEditSelectedTodo => SelectedTodo != null;

    [RelayCommand]
    private async Task AddTodoAsync()
    {
        var newTodo = new TodoItem { Title = "New Todo" };
        var dialogService = Ioc.Default.GetRequiredService<IDialogService>();
        var todoEditorViewModel = Ioc.Default.GetRequiredService<TodoEditorViewModel>();
        todoEditorViewModel.Initialize(newTodo);
        var dialogResult = await dialogService.ShowDialogAsync(todoEditorViewModel);
        if (dialogResult != true)
        {
            return;
        }
        var todo = todoEditorViewModel.GetResult();
        Todos.Add(todo);
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
        if (SelectedTodo == null)
        {
            return;
        }
        var dialogService = Ioc.Default.GetRequiredService<IDialogService>()!;
        var todoEditorViewModel = Ioc.Default.GetRequiredService<TodoEditorViewModel>();
        todoEditorViewModel.Initialize(SelectedTodo);
        var dialogResult = await dialogService.ShowDialogAsync(todoEditorViewModel);
        if (dialogResult != true)
        {
            return;
        }
        var todo = todoEditorViewModel.GetResult();
        var index = Todos.IndexOf(SelectedTodo);
        if (index >= 0)
        {
            Todos[index] = todo;
            SelectedTodo = todo;
        }
    }

    public class TodoLevelItem(TodoLevel value, string displayName)
    {
        public TodoLevel Value { get; } = value;
        public string DisplayName { get; } = displayName;

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
