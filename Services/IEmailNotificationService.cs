using Lab5.Models;

namespace Lab5.Services;

public interface IEmailNotificationService
{
    Task SendSubscriptionAsync(string subscriberEmail);
    Task SendContactAsync(ContactRequest request);
}
