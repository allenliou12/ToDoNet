using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Infrastructure.Repositories;

// This class provides the actual logic/behaviour of what each methods are supposed to do, these methods are defined in teh IToDoRepo.cs
// Contains the algorithm? for each methods defined earlier

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<TodoItem> _items = new();
    private int _nextId = 1;

    public Task<List<TodoItem>> GetAllAsync()
    {
        return Task.FromResult(_items);
    }

    public Task<TodoItem?> GetByIdAsync(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(item);
    }

    public Task AddAsync(TodoItem item)
    {
        item.Id = _nextId++;
        item.CreatedAt = DateTime.UtcNow; 
        _items.Add(item);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TodoItem item)
    {
        var index = _items.FindIndex(x => x.Id == item.Id);
        if (index != -1)
        {
            _items[index] = item;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            _items.Remove(item);
        }
        return Task.CompletedTask;
    }
}
