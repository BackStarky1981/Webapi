using Microsoft.EntityFrameworkCore;
using ToDo.Api.Models;

namespace ToDo.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDoItem>()
                .HasData(
                    new ToDo.Api.Models.ToDoItem { Id = 1, Name = "Pendiente 1", IsCompleted = false },
                    new ToDo.Api.Models.ToDoItem { Id = 2, Name = "Pendiente 2", IsCompleted = true },
                    new ToDo.Api.Models.ToDoItem { Id = 3, Name = "Pendiente 3", IsCompleted = false }
                );



        }

        public DbSet<ToDo.Api.Models.ToDoItem> ToDoItems { get; set; } = null!;
    }
}
