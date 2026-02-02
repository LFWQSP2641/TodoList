namespace TodoList.ViewModels.Interfaces;

public interface IDialogViewModel<out T>
{
    bool? DialogResult { get; set; }
    T? GetResult();
}