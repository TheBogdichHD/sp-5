using Lab5.Areas.Communication.Models;
using Lab5.Areas.Communication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab5.Areas.Site.Pages;

public class ContactModel : PageModel
{
    private readonly ICsvStorageService _csvStorageService;
    private readonly IEmailNotificationService _emailNotificationService;

    public ContactModel(
        ICsvStorageService csvStorageService,
        IEmailNotificationService emailNotificationService)
    {
        _csvStorageService = csvStorageService;
        _emailNotificationService = emailNotificationService;
    }

    [BindProperty]
    public ContactRequest Input { get; set; } = new();

    public bool IsSubmitted { get; private set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _csvStorageService.SaveContactAsync(Input);
        await _emailNotificationService.SendContactAsync(Input);
        IsSubmitted = true;
        Input = new ContactRequest();
        ModelState.Clear();

        return Page();
    }
}
