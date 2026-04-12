using System.Collections.Concurrent;
using Lab5.Areas.Communication.Services;
using Lab5.Areas.Feed.Data;
using Lab5.Areas.Feed.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Rewrite;

LoadDotEnv(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");

builder.Services
    .AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
    });
builder.Services.AddRazorPages();
builder.Services.AddDbContext<GreenswampContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")
        ?? "Data Source=Data/greenswamp.db"));
builder.Services.AddScoped<IFeedQueryService, FeedQueryService>();
builder.Services.AddSingleton<ICsvStorageService, CsvStorageService>();
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
await FeedDatabaseInitializer.InitializeAsync(app);

var logsDirectory = Path.Combine(app.Environment.ContentRootPath, "logs");
Directory.CreateDirectory(logsDirectory);
var logFilePath = Path.Combine(logsDirectory, "requests.log");
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

var rewriteOptions = new RewriteOptions()
    .AddRewrite("^index\\.html$", "/", skipRemainingRules: true)
    .AddRedirect("^(.+)\\.html$", "$1", statusCode: StatusCodes.Status301MovedPermanently);

app.UseRewriter(rewriteOptions);

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapAreaControllerRoute(
    name: "feed_area",
    areaName: "Feed",
    pattern: "Feed/{controller=Feed}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

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
