using System.Collections.Concurrent;
using Lab5.Areas.Feed.Database;
using Lab5.Areas.Feed.Services;
using Lab5.Services;
using Microsoft.EntityFrameworkCore;

LoadDotEnv(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICsvStorageService, CsvStorageService>();
builder.Services.AddDbContext<GreenswampContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FeedDb")));
builder.Services.AddScoped<IFeedQueryService, FeedQueryService>();
builder.Services.AddScoped<IHashtagFormatter, HashtagFormatter>();
builder.Services.AddScoped<FeedDatabaseInitializer>();
builder.Services.Configure<GmailSmtpOptions>(options =>
{
    options.Host = builder.Configuration["GmailSmtp:Host"] ?? "smtp.gmail.com";

    var portRaw = Environment.GetEnvironmentVariable("GMAIL_SMTP_PORT")
                  ?? builder.Configuration["GmailSmtp:Port"];
    options.Port = int.TryParse(portRaw, out var port) ? port : 587;

    options.Username = Environment.GetEnvironmentVariable("GMAIL_SMTP_USERNAME") ?? string.Empty;
    options.AppPassword = Environment.GetEnvironmentVariable("GMAIL_SMTP_APP_PASSWORD") ?? string.Empty;
    options.FromEmail = Environment.GetEnvironmentVariable("GMAIL_SMTP_FROM_EMAIL") ?? string.Empty;
    options.ToEmail = Environment.GetEnvironmentVariable("GMAIL_SMTP_TO_EMAIL") ?? string.Empty;
});
builder.Services.AddTransient<IEmailNotificationService, GmailEmailNotificationService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<FeedDatabaseInitializer>();
    await initializer.InitializeAsync();
}

var logFilePath = Path.Combine(app.Environment.ContentRootPath, "requests.log");
var logLocks = new ConcurrentDictionary<string, object>();

app.Use(async (context, next) =>
{
    await next();

    var remoteIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    var method = context.Request.Method;
    var path = context.Request.Path.HasValue ? context.Request.Path.Value : "/";
    var statusCode = context.Response.StatusCode;

    var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t{remoteIp}\t{method}\t{path}\t{statusCode}";
    var fileLock = logLocks.GetOrAdd(logFilePath, _ => new object());

    lock (fileLock)
    {
        File.AppendAllLines(logFilePath, [line]);
    }
});

app.UseStatusCodePagesWithReExecute("/404");

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapAreaControllerRoute(
    name: "feed_area",
    areaName: "Feed",
    pattern: "{area:exists}/{controller=Feed}/{action=Index}/{id?}");

app.MapGet("/index.html", (HttpContext context) =>
{
    context.Response.Redirect("/", permanent: true);
    return Task.CompletedTask;
});

app.MapGet("/about.html", (HttpContext context) =>
{
    context.Response.Redirect("/about", permanent: true);
    return Task.CompletedTask;
});

app.MapGet("/contact.html", (HttpContext context) =>
{
    context.Response.Redirect("/contact", permanent: true);
    return Task.CompletedTask;
});

app.MapGet("/privacy.html", (HttpContext context) =>
{
    context.Response.Redirect("/privacy", permanent: true);
    return Task.CompletedTask;
});

app.MapGet("/tos.html", (HttpContext context) =>
{
    context.Response.Redirect("/tos", permanent: true);
    return Task.CompletedTask;
});

app.Run();

static void LoadDotEnv(string path)
{
    if (!File.Exists(path))
    {
        return;
    }

    foreach (var rawLine in File.ReadAllLines(path))
    {
        var line = rawLine.Trim();
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
        {
            continue;
        }

        var separatorIndex = line.IndexOf('=');
        if (separatorIndex <= 0)
        {
            continue;
        }

        var key = line[..separatorIndex].Trim();
        var value = line[(separatorIndex + 1)..].Trim();

        if (value.Length >= 2 &&
            ((value.StartsWith('"') && value.EndsWith('"')) ||
             (value.StartsWith('\'') && value.EndsWith('\''))))
        {
            value = value[1..^1];
        }

        Environment.SetEnvironmentVariable(key, value);
    }
}
