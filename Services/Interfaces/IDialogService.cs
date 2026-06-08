using System.Threading.Tasks;
using TodoList.ViewModels.Interfaces;

namespace TodoList.Services.Interfaces;

public interface IDialogService
{
    Task<bool?> ShowDialogAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IDialogRequestClose;
}
