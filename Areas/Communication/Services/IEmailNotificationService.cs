using Lab5.Areas.Communication.Models;

namespace Lab5.Areas.Communication.Services;

public interface IEmailNotificationService
{
    Task SendSubscriptionAsync(string subscriberEmail);
    Task SendContactAsync(ContactRequest request);
}
