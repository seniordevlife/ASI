using ASI.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{

    protected override void OnConfiguring
       (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "ContactDb");
    }

    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<Email> Emails { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public ApplicationDbContext()
    { }
    public static ApplicationDbContext Create()
    {
        return new ApplicationDbContext();
    }
}
