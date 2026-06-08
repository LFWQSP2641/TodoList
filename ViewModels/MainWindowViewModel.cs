namespace TodoList.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public TodoListWidgetViewModel TodoListWidgetViewModel { get; } = new();
}
