using SWGW.Domain.Entities.Perimits;

namespace SWGW.AppServices.Notifications;

public interface INotificationService
{
    Task<NotificationResult> SendNotificationAsync(Template template, string recipientEmail, Permit permit,
        CancellationToken token = default);
}
