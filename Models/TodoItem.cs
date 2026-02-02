using System;

namespace TodoList.Models;

public class TodoItem
{
    public int Id { get; set; }
    public long CreatedTime { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    public required string Title { get; set; }
    public string? Description { get; set; }
    public TodoLevel Level { get; set; } = TodoLevel.Medium;
}

public enum TodoLevel
{
    Low,
    Medium,
    High
}
