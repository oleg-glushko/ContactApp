using ContactApp.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactApp.Pages.Contacts;

public class ViewModel : PageModel
{
    private readonly ContactRepository _contactRepository;

    public Contact Contact { get; set; } = null!;

    public ViewModel(ContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public ActionResult OnGet(int id)
    {
        var contact = _contactRepository.GetById(id);
        if (contact is null)
            return NotFound();

        Contact = contact;
        return Page();
    }
}
