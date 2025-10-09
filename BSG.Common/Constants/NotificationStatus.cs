namespace BSG.Common.Constants;

public class NotificationStatus
{
    public const string Pending = "Pending";
    public const string Active = "Active";
    public const string Snoozed = "Snoozed";
    public const string Closed = "Closed";

    public static List<string> GetAllNotificationStatuses()
    {
        return
        [
            Pending,
            Active,
            Snoozed,
            Closed
        ];
    }
}