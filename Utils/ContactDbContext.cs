using ContactApp.Contacts;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Utils;

public class ContactDbContext : DbContext
{
    public DbSet<Contact> Contacts => Set<Contact>();

    public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().HasData(
            new Contact
            {
                Id = 1,
                First = "John",
                Last = "Smith",
                Phone = "123-456-7890",
                Email = "john@example.com"
            },
            new Contact
            {
                Id = 2,
                First = "Dana",
                Last = "Crandith",
                Phone = "123-456-7890",
                Email = "dcran@example.com"
            },
            new Contact
            {
                Id = 3,
                First = "Edith",
                Last = "Neutvaar",
                Phone = "123-456-7890",
                Email = "en@example.com"
            });
    }
}
