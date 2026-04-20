using System.ComponentModel.DataAnnotations;

namespace Lab5.Models;

public class ContactRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.edu$", ErrorMessage = "Email must be on the .edu domain.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;
}
