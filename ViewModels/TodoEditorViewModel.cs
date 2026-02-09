using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoList.Models;
using TodoList.ViewModels.Interfaces;

namespace TodoList.ViewModels;

public partial class TodoEditorViewModel : ViewModelBase, IDialogResultProvider<TodoItem>, IDialogRequestClose, IDialogInitialize<TodoItem>
{
    public event Action<bool?>? RequestClose;

    [ObservableProperty]
    public partial string Title { get; set; }
    
    [ObservableProperty]
    public partial string? Description { get; set; }

    public TodoEditorViewModel()
    {
        Title = "";
    }

    public void Initialize(TodoItem todoItem)
    {
        Title = todoItem.Title ?? "";
        Description = todoItem.Description;
    }
    
    public TodoItem GetResult()
    {
        return new TodoItem {
            Title = Title,
            Description = Description
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