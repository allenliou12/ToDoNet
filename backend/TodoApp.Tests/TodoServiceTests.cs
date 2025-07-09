using Xunit;
using Moq;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

public class TodoServiceTests
{
    private readonly Mock<ITodoRepository> _mockRepo;
    private readonly TodoService _service;

    public TodoServiceTests()
    {
        _mockRepo = new Mock<ITodoRepository>();
        _service = new TodoService(_mockRepo.Object);
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Title_Is_Empty()
    {
        var todo = new TodoItem { Title = "", Description = "Test description" };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(todo));

        Assert.Equal("Title cannot be empty.", ex.Message);
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Description_Is_Empty()
    {
        var todo = new TodoItem { Title = "Valid title", Description = "" };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(todo));

        Assert.Equal("Description cannot be empty.", ex.Message);
    }

    [Fact]
    public async Task AddAsync_Should_Call_Repo_When_Valid()
    {
        var todo = new TodoItem { Title = "Title", Description = "Description" };

        await _service.AddAsync(todo);

        _mockRepo.Verify(repo => repo.AddAsync(todo), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Title_Is_Empty()
    {
        var todo = new TodoItem { Title = "", Description = "Something" };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(todo));

        Assert.Equal("Title cannot be empty.", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_Repo_When_Valid()
    {
        var todo = new TodoItem { Title = "Updated Title", Description = "Updated Description" };

        await _service.UpdateAsync(todo);

        _mockRepo.Verify(repo => repo.UpdateAsync(todo), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_Call_Repo_With_Id()
    {
        int testId = 5;

        await _service.DeleteAsync(testId);

        _mockRepo.Verify(repo => repo.DeleteAsync(testId), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Todos()
    {
        var fakeTodos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "A", Description = "..." },
            new TodoItem { Id = 2, Title = "B", Description = "..." }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(fakeTodos);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("A", result[0].Title);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Todo()
    {
        var todo = new TodoItem { Id = 1, Title = "X", Description = "..." };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(todo);

        var result = await _service.GetByIdAsync(1);

        Assert.Equal("X", result?.Title);
    }
}
