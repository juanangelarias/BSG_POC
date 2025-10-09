using BSG.Common.Constants;
using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class NotificationPropertyDto: DtoBase
{
    public long NotificationId { get; set; }
    public long NotificationPropertyDefinitionId { get; set; }
    public string TextValue { get; set; } = null!;

    public object? Value => Property.Type switch
    {
        NotificationPropertyType.Boolean => bool.Parse(TextValue),
        NotificationPropertyType.Integer => int.Parse(TextValue),
        NotificationPropertyType.Decimal => decimal.Parse(TextValue),
        NotificationPropertyType.Date => DateTime.Parse(TextValue),
        _ => TextValue
    };
    
    public NotificationDto Notification { get; set; } = null!;
    public NotificationPropertyDefinitionDto Property { get; set; } = null!;
}