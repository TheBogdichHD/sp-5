using Lab5.Areas.Feed.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Areas.Feed.Controllers;

[Area("Feed")]
public sealed class FeedController(IFeedQueryService feedQueryService) : Controller
{
    [HttpGet("/feed")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await feedQueryService.GetFeedAsync(null, cancellationToken);
        return View(model);
    }

    [HttpGet("/ponds/{tag?}")]
    public async Task<IActionResult> Ponds(string? tag, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            return Redirect("/feed");
        }

        var model = await feedQueryService.GetFeedAsync(tag, cancellationToken);
        return View("Index", model);
    }

    [HttpGet("/feed/post/{postId:int}")]
    public async Task<IActionResult> Post(int postId, CancellationToken cancellationToken)
    {
        var model = await feedQueryService.GetPostAsync(postId, cancellationToken);
        if (model is null)
        {
            return Redirect("/404");
        }

        return View(model);
    }
}
