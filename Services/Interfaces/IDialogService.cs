using System.Threading.Tasks;

namespace TodoList.Services.Interfaces;

public interface IDialogService
{
    Task<bool?> ShowDialogAsync(object vm);
    Task<bool?> ShowDialogAsync<T>(T vm) where T : TodoList.ViewModels.Interfaces.IDialogRequestClose;
}