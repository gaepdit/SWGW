using SWGW.Domain.Entities.WorkEntries;

namespace SWGW.AppServices.Notifications;

public interface INotificationService
{
    Task<NotificationResult> SendNotificationAsync(Template template, string recipientEmail, WorkEntry workEntry,
        CancellationToken token = default);
}
