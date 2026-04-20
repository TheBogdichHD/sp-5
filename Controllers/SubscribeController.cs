using Lab5.Models;
using Lab5.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Controllers;

[Route("subscribe")]
public class SubscribeController : Controller
{
    private readonly ICsvStorageService _csvStorageService;
    private readonly IEmailNotificationService _emailNotificationService;

    public SubscribeController(
        ICsvStorageService csvStorageService,
        IEmailNotificationService emailNotificationService)
    {
        _csvStorageService = csvStorageService;
        _emailNotificationService = emailNotificationService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Subscribe(EmailRequest data)
    {
        if (!ModelState.IsValid)
        {
            TempData["SubscribeError"] = "Please enter a valid email.";
            return RedirectBack();
        }

        try
        {
            await _csvStorageService.SaveSubscriptionAsync(data.Email);
            await _emailNotificationService.SendSubscriptionAsync(data.Email);
            TempData["SubscribeSuccess"] = "1";
        }
        catch (Exception ex)
        {
            TempData["SubscribeError"] = $"Subscription saved but notification email failed: {ex.Message}";
        }

        return RedirectBack();
    }

    private IActionResult RedirectBack()
    {
        var referer = Request.Headers.Referer.ToString();
        if (string.IsNullOrWhiteSpace(referer))
        {
            return Redirect("/");
        }

        return Redirect(referer);
    }
}
