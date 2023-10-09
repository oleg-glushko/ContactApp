using ContactApp.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ContactApp.Pages.Contacts;

public class EditModel : PageModel
{
    private readonly ContactRepository _contactRepository;

    public bool ShowMessage { get; set; }

    public int Id { get; set; }

    [BindProperty]
    public string? First { get; set; }

    [BindProperty]
    public string? Last { get; set; }

    [BindProperty, EmailAddress]
    public string Email { get; set; } = null!;

    [BindProperty]
    public string Phone { get; set; } = null!;

    public EditModel(ContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public ActionResult OnGet(int id)
    {
        var contact = _contactRepository.GetById(id);
        if (contact is null)
            return NotFound();

        Id = contact.Id;
        First = contact.First;
        Last = contact.Last;
        Email = contact.Email ?? string.Empty;
        Phone = contact.Phone;
        return Page();
    }

    public ActionResult OnPost(int id)
    {
        if (!_contactRepository.EmailIsUnique(Email))
        {
            ModelState.AddModelError("Email", "Email Must Be Unique");
        }

        if (!ModelState.IsValid)
            return Page();

        var contact = _contactRepository.GetById(id);

        if (contact is null)
            return NotFound();

        contact.First = First;
        contact.Last = Last;
        contact.Email = Email;
        contact.Phone = Phone;
        _contactRepository.Update(contact);

        return RedirectToPage("Index");
    }
}
