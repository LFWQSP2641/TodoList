namespace TodoList.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public TodoListWidgetViewModel TodoListWidgetViewModel { get; } = new();
}
