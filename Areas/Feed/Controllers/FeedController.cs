using Lab5.Areas.Feed.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Areas.Feed.Controllers;

[Area("Feed")]
public class FeedController(IFeedQueryService queryService) : Controller
{
    [HttpGet("/feed")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await queryService.GetFeedAsync(cancellationToken);
        return View(model);
    }

    [HttpGet("/ponds/{tag}")]
    public async Task<IActionResult> Ponds(string tag, CancellationToken cancellationToken)
    {
        var model = await queryService.GetPondAsync(tag, cancellationToken);
        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }
}
