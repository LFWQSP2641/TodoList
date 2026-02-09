namespace TodoList.Services.Interfaces;

public readonly record struct DialogResult<T>(bool? Result, T? Payload);
