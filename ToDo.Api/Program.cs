using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ToDo.Api.Data;
using ToDo.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Agrega el servicio para la conexión a la base de datos SQL server
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(
    builder.Configuration
    .GetConnectionString("DefaultConnection"))
);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/api/pendientes", 
    async (AppDbContext dbContext) =>
    {
        var toDoItems = await dbContext
        .ToDoItems.ToListAsync();
        return Results.Ok(toDoItems);
    });

app.MapGet("/api/pendientes/{id:int}",
    async (AppDbContext dbContext, int id) =>
    {
        var ToDoItem = await dbContext
            .ToDoItems
            .FirstOrDefaultAsync(tdi => tdi.Id == id);
        if (ToDoItem is null)
        {
            return Results.NotFound();
        }
        return Results.Ok(ToDoItem);
    });

app.MapPost("/api/pendientes",
    async (AppDbContext dbContext, ToDoItem toDoItem ) =>
    {
        toDoItem.IsCompleted = false;
        await dbContext.ToDoItems.AddAsync(toDoItem);
        await dbContext.SaveChangesAsync();
        return Results
        .Created(
            $"/api/pendientes/{toDoItem.Id}",
            toDoItem);
    });

app.Run();

