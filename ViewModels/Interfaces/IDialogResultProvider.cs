namespace TodoList.ViewModels.Interfaces;

public interface IDialogResultProvider<out T>
{
    T? GetResult();
}
