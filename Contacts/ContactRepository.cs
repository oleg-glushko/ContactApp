using ContactApp.Utils;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ContactApp.Contacts;

public class ContactRepository
{
    private const int PageSize = 10;
    private readonly ContactDbContext _context;

    public ContactRepository(ContactDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Contact> All(int page = 1)
    {
        if (page <= 0) page = 1;

        return _context.Contacts
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }

    public Contact? GetById(int id)
    {
        return _context.Contacts.Find(id);
    }

    public IReadOnlyList<Contact> Search(string text)
    {
        return _context.Contacts
            .Where(x =>
                x.First != null && x.First.Contains(text) ||
                x.Last != null && x.Last.Contains(text) ||
                x.Email.Contains(text) ||
                x.Phone.Contains(text)
            ).ToList();
    }

    public void Add(Contact contact)
    {
        _context.Add(contact);
        _context.SaveChanges();
    }

    public void Update(Contact contact)
    {
        _context.Update(contact);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        _context.Database.ExecuteSqlInterpolated($"DELETE FROM Contacts WHERE Id = {id}");
    }

    public void BulkDelete(int[] ids)
    {
        var parameters = ids.Select((id, index) => new SqliteParameter($"@p{index}", id)).ToArray();
        var placeholders = string.Join(",", parameters.Select(p => p.ParameterName));
        var commandText = FormattableStringFactory.Create(
            $"DELETE FROM Contacts WHERE Id IN ({placeholders})", parameters);
        _context.Database.ExecuteSqlInterpolated(commandText);
    }

    public bool EmailIsUnique(string email)
    {
        Contact? contactWithSameEmail = _context.Contacts.SingleOrDefault(x => x.Email!.ToUpper() == email.ToUpper());

        return contactWithSameEmail == null;
    }

    public int Count()
    {
        Thread.Sleep(TimeSpan.FromSeconds(2));
        return _context.Contacts.Count();
    }
}