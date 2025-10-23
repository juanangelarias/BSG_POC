using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class NotificationDto: DtoBase
{
    public string SenderEmail { get; set; } = "";
    public DateTime EmissionTime { get; set; }
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public bool SendEmail { get; set; }
    public bool SendSms { get; set; }
    
    public List<NotificationPropertyDto> Properties { get; set; } = [];
    public List<NotificationRecipientDto> Recipients { get; set; } = [];
}