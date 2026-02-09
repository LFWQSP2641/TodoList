using System.Threading.Tasks;
using TodoList.ViewModels.Interfaces;

namespace TodoList.Services.Interfaces;

public interface IDialogService
{
    Task<DialogResult<TResult>> ShowDialogAsync<TViewModel, TResult>()
        where TViewModel : IDialogRequestClose, IDialogResultProvider<TResult>;

    Task<DialogResult<TResult>> ShowDialogAsync<TViewModel, TArg, TResult>(TArg arg)
        where TViewModel : IDialogRequestClose, IDialogInitialize<TArg>, IDialogResultProvider<TResult>;
}