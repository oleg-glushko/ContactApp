using ContactApp.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ContactApp.Pages.Contacts;

public class NewModel : PageModel
{
    private readonly ContactRepository _contactRepository;

    [BindProperty]
    public string? First { get; set; }

    [BindProperty]
    public string? Last { get; set; }

    [BindProperty, EmailAddress]
    public string Email { get; set; } = null!;

    [BindProperty]
    public string Phone { get; set; } = null!;

    public NewModel(ContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }


    public ActionResult OnPost()
    {
        if (!_contactRepository.EmailIsUnique(Email))
        {
            ModelState.AddModelError("Email", "Email Must Be Unique");
        }

        if (!ModelState.IsValid)
            return Page();

        var contact = new Contact
        {
            Id = 0,
            First = First,
            Last = Last,
            Phone = Phone,
            Email = Email,
        };

        _contactRepository.Add(contact);
        _contactRepository.Add(contact);
        return RedirectToPage("Index");
    }
}
