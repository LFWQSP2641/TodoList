using System.Threading.Tasks;
using TodoList.ViewModels.Interfaces;

namespace TodoList.Services.Interfaces;

public interface IDialogService
{
    Task<(bool? Result, TViewModel ViewModel)> ShowDialogAsync<TViewModel>()
        where TViewModel : IDialogRequestClose;

    Task<(bool? Result, TViewModel ViewModel)> ShowDialogAsync<TViewModel, TArg>(TArg arg)
        where TViewModel : IDialogRequestClose, IDialogInitialize<TArg>;
}