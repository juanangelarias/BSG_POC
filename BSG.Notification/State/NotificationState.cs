using BSG.Common.Constants;
using BSG.Common.DTO;
using BSG.DataServices;
using BSG.States.Base;

namespace BSG.Notification.State;

public interface INotificationState
{
    List<NotificationPropertyDefinitionDto> PropertyDefinitions { get; set; }
    List<NotificationDto> Notifications { get; set; }


    Task GetNotifications(string email);
    Task GetPropertyDefinitions();
    Task<NotificationPropertyDefinitionDto?> SavePropertyDefinition(NotificationPropertyDefinitionDto propertyDefinition);
    Task DeletePropertyDefinition(long id);
}

public class NotificationState : StateBase, INotificationState
{
    #region Fields & Properties

    private readonly INotificationPropertyDataService _propertyDataService;
    private readonly INotificationDataService _notificationDataService;

    #region PropertyDefinitions

    private List<NotificationPropertyDefinitionDto> _propertyDefinitions = [];

    public List<NotificationPropertyDefinitionDto> PropertyDefinitions
    {
        get => _propertyDefinitions;
        set
        {
            _propertyDefinitions = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Notifications

    private List<NotificationDto> _notifications = [];

    public List<NotificationDto> Notifications
    {
        get => _notifications;
        set
        {
            _notifications = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #endregion

    public NotificationState(INotificationPropertyDataService propertyDataService,
        INotificationDataService notificationDataService)
    {
        _propertyDataService = propertyDataService;
        _notificationDataService = notificationDataService;
    }

    public async Task GetNotifications(string email)
    {
        Notifications = await _notificationDataService.GetByEmail(email);
    }

    public async Task GetPropertyDefinitions()
    {
        PropertyDefinitions = await _propertyDataService.Get();
    }

    public async Task<NotificationPropertyDefinitionDto?> SavePropertyDefinition(NotificationPropertyDefinitionDto prop)
    {
        var response = prop.Id == 0
            ? await _propertyDataService.Create(prop)
            : await _propertyDataService.Update(prop);

        return response;
    }

    public async Task DeletePropertyDefinition(long id)
    {
        await _propertyDataService.Delete(id);
        
        var toRemove = PropertyDefinitions
            .FirstOrDefault(w => w.Id == id);
        
        if(toRemove != null)
            PropertyDefinitions.Remove(toRemove);
    }
}