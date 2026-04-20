using System.ComponentModel.DataAnnotations;

namespace Lab5.Models;

public class EmailRequest
{
    [Required]
    [EmailAddress]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;
}
