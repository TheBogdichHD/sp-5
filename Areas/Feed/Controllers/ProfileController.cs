using Lab5.Areas.Feed.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Areas.Feed.Controllers;

[Area("Feed")]
public class ProfileController(IFeedQueryService queryService) : Controller
{
    [HttpGet("/profile/{username}")]
    public async Task<IActionResult> Index(string username, CancellationToken cancellationToken)
    {
        var model = await queryService.GetProfileAsync(username, cancellationToken);
        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }
}
