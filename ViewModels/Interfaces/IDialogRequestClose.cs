using System;

namespace TodoList.ViewModels.Interfaces;

public interface IDialogRequestClose
{
    event Action<bool?>? RequestClose;
}
