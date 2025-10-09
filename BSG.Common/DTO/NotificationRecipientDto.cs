using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class NotificationRecipientDto: DtoBase
{
    public long NotificationId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? PhoneNumber { get; set; } = "";
    public DateTime? Snoozed { get; set; }
    public string Status { get; set; } = "";


    public NotificationDto Notification { get; set; } = null!;
}