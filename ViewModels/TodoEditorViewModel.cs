using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoList.Models;
using TodoList.ViewModels.Interfaces;

namespace TodoList.ViewModels;

public partial class TodoEditorViewModel : ViewModelBase, IDialogRequestClose
{
    private TodoItem? _selectedItem;

    public TodoEditorViewModel()
    {
        Title = "";
    }

    [ObservableProperty] public partial string Title { get; set; }

    [ObservableProperty] public partial string? Description { get; set; }

    [ObservableProperty] public partial TodoLevel Level { get; set; } = TodoLevel.Medium;

    public event Action<bool?>? RequestClose;

    public void Initialize(TodoItem todoItem)
    {
        _selectedItem = todoItem;
        Title = todoItem.Title ?? "";
        Description = todoItem.Description;
        Level = todoItem.Level;
    }

    public TodoItem GetResult()
    {
        return new TodoItem
        {
            Id = _selectedItem?.Id ?? 0,
            CreatedTime = _selectedItem?.CreatedTime ?? new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
            EditedTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
            Title = Title,
            Description = Description,
            Level = Level,
        };
    }

    [RelayCommand]
    private void Save()
    {
        RequestClose?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}
