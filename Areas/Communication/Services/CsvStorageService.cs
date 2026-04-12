using System.Globalization;
using CsvHelper;
using Lab5.Areas.Communication.Models;

namespace Lab5.Areas.Communication.Services;

public class CsvStorageService : ICsvStorageService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly string _dataDirectory;

    public CsvStorageService(IWebHostEnvironment environment)
    {
        _dataDirectory = Path.Combine(environment.ContentRootPath, "Data");
        Directory.CreateDirectory(_dataDirectory);
    }

    public Task SaveSubscriptionAsync(string email)
    {
        var record = new SubscriptionCsvRecord
        {
            CreatedAtUtc = DateTime.UtcNow,
            Email = email
        };

        return AppendRecordAsync(Path.Combine(_dataDirectory, "subscriptions.csv"), record);
    }

    public Task SaveContactAsync(ContactRequest request)
    {
        var record = new ContactCsvRecord
        {
            CreatedAtUtc = DateTime.UtcNow,
            Name = request.Name,
            Email = request.Email,
            Subject = request.Subject,
            Message = request.Message
        };

        return AppendRecordAsync(Path.Combine(_dataDirectory, "contacts.csv"), record);
    }

    private async Task AppendRecordAsync<T>(string path, T record)
    {
        await _semaphore.WaitAsync();
        try
        {
            var hasContent = File.Exists(path) && new FileInfo(path).Length > 0;
            await using var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            await using var writer = new StreamWriter(stream);
            await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            if (!hasContent)
            {
                csv.WriteHeader<T>();
                await csv.NextRecordAsync();
            }

            csv.WriteRecord(record);
            await csv.NextRecordAsync();
            await writer.FlushAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private class SubscriptionCsvRecord
    {
        public DateTime CreatedAtUtc { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    private class ContactCsvRecord
    {
        public DateTime CreatedAtUtc { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
