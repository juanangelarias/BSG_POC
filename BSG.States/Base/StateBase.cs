namespace BSG.States.Base;

public abstract class StateBase
{
    public Action? NotifyChanges { get; set; }
    protected void OnPropertyChanged() => NotifyChanges?.Invoke();
}