using TodoApp.Application.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Repositories;


// Creates a builder like FastApi() in Python
var builder = WebApplication.CreateBuilder(args);

// Add CORS, understanding that this is to allow frontend to connect to backend, if no CORS, since they have 2 different domain name, they cannot connect to each other
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Edit so that it fits the frontend domain
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Dependency Injection
builder.Services.AddScoped<TodoService>();
builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable CORS
app.UseCors();

//Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Routes, similar to python's version of app.get("/"), etc. etc.
app.MapGet("/api/todos", async (TodoService service) =>
    Results.Ok(await service.GetAllAsync()));
    // If result is Ok, get all notes

app.MapGet("/api/todos/{id}", async (int id, TodoService service) =>
{
    var todo = await service.GetByIdAsync(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

// For the POST API, since we have a constraint that title and description cannot be empty, use a try catch block to better display error message
app.MapPost("/api/todos", async (TodoItem item, TodoService service) =>
{
    try
    {
        await service.AddAsync(item);
        return Results.Ok(item);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPut("/api/todos/{id}", async (int id, TodoItem item, TodoService service) =>
{
    if (item.Id != id)
    {
        return Results.BadRequest("ID mismatch");
    }

    try
    {
        await service.UpdateAsync(item);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapDelete("/api/todos/{id}", async (int id, TodoService service) =>
{
    await service.DeleteAsync(id);
    return Results.NoContent();
});

app.Run();