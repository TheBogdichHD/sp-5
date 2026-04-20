using Lab5.Areas.Feed.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Areas.Feed.Controllers;

[Area("Feed")]
public class PostController(IFeedQueryService queryService) : Controller
{
    [HttpGet("/feed/post/{postId:long}")]
    public async Task<IActionResult> Details(long postId, CancellationToken cancellationToken)
    {
        var model = await queryService.GetPostDetailsAsync(postId, cancellationToken);
        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }
}
