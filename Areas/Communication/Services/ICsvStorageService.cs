using Lab5.Areas.Communication.Models;

namespace Lab5.Areas.Communication.Services;

public interface ICsvStorageService
{
    Task SaveSubscriptionAsync(string email);
    Task SaveContactAsync(ContactRequest request);
}
