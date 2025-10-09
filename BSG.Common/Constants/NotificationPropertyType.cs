namespace BSG.Common.Constants;

public class NotificationPropertyType
{
    public const string Email = "Email";
    public const string Phone = "Phone";
    public const string String = "String";
    public const string Integer = "Integer";
    public const string Boolean = "Boolean";
    public const string Date = "Date";
    public const string Decimal = "Decimal";

    public static List<string> GetAllNotificationPropertyTypes()
    {
        return
        [
            Email,
            Phone,
            String,
            Integer,
            Boolean,
            Date,
            Decimal
        ];
    }
}