namespace TodoApp.Domain.Entities;
// Basically this is the "model" of each Note Item
// The "schema" of each Note Item
// The "skeleton" of each Note Item and what it is supposed to look like

// This is the same thing as the class creation in Python with the __init__ method
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}