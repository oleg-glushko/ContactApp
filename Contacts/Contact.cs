using System.ComponentModel.DataAnnotations;

namespace ContactApp.Contacts;

public class Contact
{
    public int Id { get; init; }
    public string? First { get; set; }
    public string? Last { get; set; }
    [Required]
    public required string Phone { get; set; }
    [Required, EmailAddress]
    public required string Email { get; set; }
}
