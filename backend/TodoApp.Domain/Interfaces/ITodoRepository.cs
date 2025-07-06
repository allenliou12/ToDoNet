using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;
// This namespace is similar to a package in Python

// List of "What" methods every NoteItem object under the NoteItems class will have
// A list of what data operations/methods are allowed
public interface ITodoRepository
{
    Task<List<TodoItem>> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(int id);
    Task AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
    Task DeleteAsync(int id);
}

