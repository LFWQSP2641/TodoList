namespace TodoList.ViewModels.Interfaces;

public interface IDialogInitialize<in TArg>
{
    void Initialize(TArg arg);
}
