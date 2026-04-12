using Lab5.Areas.Feed.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Areas.Feed.Controllers;

[Area("Feed")]
public sealed class ProfileController(IFeedQueryService feedQueryService) : Controller
{
    [HttpGet("/profile")]
    public IActionResult RedirectToDefaultProfile()
    {
        return Redirect("/profile/frogger_vig");
    }

    [HttpGet("/profile/{username}")]
    public async Task<IActionResult> Index(string username, CancellationToken cancellationToken)
    {
        var model = await feedQueryService.GetProfileAsync(username, cancellationToken);
        if (model is null)
        {
            return Redirect("/404");
        }

        return View(model);
    }
}
