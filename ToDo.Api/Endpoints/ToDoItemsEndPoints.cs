using Microsoft.EntityFrameworkCore;
using ToDo.Api.Data;
using ToDo.Api.DTOs;
using ToDo.Api.Models;

namespace ToDo.Api.Endpoints
{
    public static class ToDoItemsEndPoints
    {
        public static void MapToDoItemsEndPoints(this IEndpointRouteBuilder routes)
        {
            var toDoEndpoints = routes.MapGroup("/api/pendientes").WithTags("Pendientes");

            toDoEndpoints.MapGet("/", GetAllToDoItems);

            toDoEndpoints.MapGet("/{id:int}", GetToDoItemById);

            toDoEndpoints.MapPost("/", CreateToDoItem);

            toDoEndpoints.MapPut("/{id}", UpdateToDoItem);

            toDoEndpoints.MapDelete("/{id:int}", DeleteToDoItem);
        }



        private async static Task<IResult> GetAllToDoItems(
            AppDbContext dbContext)
        {
            var toDoItems = await dbContext
                .ToDoItems
                .Select(p => new ToDoItemDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsCompleted = p.IsCompleted,
                })
                .ToListAsync();


            return Results.Ok(toDoItems);
        }

        private async static Task<IResult> GetToDoItemById(AppDbContext dbContext, int id)
        {
            var toDoItem = await dbContext
            .ToDoItems
            .FirstOrDefaultAsync(tdi => tdi.Id == id);

            if (toDoItem is null)
            {
                return Results.NotFound();
            }
            var toDoItemDto = new ToDoItemDto()
            {
                Id = toDoItem.Id,
                Name = toDoItem.Name,
                IsCompleted = toDoItem.IsCompleted,

            };
            return Results.Ok(toDoItemDto);
        }

        private async static Task<IResult> CreateToDoItem(
            AppDbContext dbContext, CreateToDoItemDto createToDoItemDto)
        {
            var toDoItem = new ToDoItem()
            {
                Name = createToDoItemDto.Name,
                IsCompleted = false
            };

            await dbContext.ToDoItems.AddAsync(toDoItem);
            await dbContext.SaveChangesAsync();

            var toDoItemDto = new ToDoItemDto()
            {
                Id = toDoItem.Id,
                Name = toDoItem.Name,
                IsCompleted = false
            };
            return Results.Created(
                $"/api/pendientes/{toDoItem.Id}", toDoItemDto);
        }

        private static async Task<IResult> UpdateToDoItem(
            AppDbContext dbContext, int Id, UpdateToDoItemDto item)
        {

            var toDoItem = await dbContext
            .ToDoItems
            .FirstOrDefaultAsync(tdi => tdi.Id == Id);

            if (toDoItem is null)
            {
                return Results.NotFound();
            }

            toDoItem.Name = item.Name;
            toDoItem.IsCompleted = item.IsCompleted;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }

        private static async Task<IResult> DeleteToDoItem(
            AppDbContext dbContext, int id)
        {
            var toDoItem = await dbContext
                .ToDoItems
                .FirstOrDefaultAsync(tdi => tdi.Id == id);
            if (toDoItem is null)
            {
                return Results.NotFound();
            }
            dbContext.ToDoItems.Remove(toDoItem);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        }


    }
}
