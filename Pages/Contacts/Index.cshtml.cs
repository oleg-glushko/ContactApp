using ContactApp.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactApp.Pages.Contacts;

public class IndexModel : PageModel
{
    private readonly ContactRepository _contactRepository;
    public IReadOnlyList<Contact> Contacts { get; set; } = new List<Contact>();

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; }

    public IndexModel(ContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public ActionResult OnGet(string? q)
    {
        PageNumber = PageNumber <= 0
            ? 1
            : PageNumber;

        Contacts = q is null
            ? _contactRepository.All(PageNumber)
            : _contactRepository.Search(q);

        //Response.Headers.Append("Vary", "HX-Request");

        if (Request.Headers.TryGetValue("hx-trigger", out var trigger) && trigger == "search")
        {
            return Partial("_Rows", Contacts);
        }

        return Page();
    }
}
