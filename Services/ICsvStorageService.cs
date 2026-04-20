using Lab5.Models;

namespace Lab5.Services;

public interface ICsvStorageService
{
    Task SaveSubscriptionAsync(string email);
    Task SaveContactAsync(ContactRequest request);
}
