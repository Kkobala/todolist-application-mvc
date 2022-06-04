using Microsoft.EntityFrameworkCore;

namespace TodoList_Application
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<TodoList> TodoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoList>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
