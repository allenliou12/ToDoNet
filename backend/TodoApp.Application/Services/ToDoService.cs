using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
// This using keyword? Is similar to from domain.entities import ToDoItems in Python


namespace TodoApp.Application.Services;

// The following codeblock is to like creates a class named TodoService, which handles the business workflow logic? According to GPT. From what I understand this files is for like constraints on what the methods can do? 

// Defines the condition how the methods/operations can be used (These methods/operations are defined in teh ITodoRepo.cs files)
// Can be amended later after business rules are identified, like Title cannot be empty etc. etc.
public class TodoService
{
    private readonly ITodoRepository _repo;

    public TodoService(ITodoRepository repo)
    {
        _repo = repo;
    }

    public Task<List<TodoItem>> GetAllAsync() => _repo.GetAllAsync();

    public Task<TodoItem?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

    public async Task AddAsync(TodoItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Title))
            throw new ArgumentException("Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(item.Description))
            throw new ArgumentException("Description cannot be empty.");

        await _repo.AddAsync(item);
    }

    public async Task UpdateAsync(TodoItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Title))
            throw new ArgumentException("Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(item.Description))
            throw new ArgumentException("Description cannot be empty.");

        await _repo.UpdateAsync(item);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
}
