using BSG.App.Component.Model;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.DataServices;
using BSG.States.Base;

namespace BSG.App.Component.State;

public interface IComponentState
    : IWorkWithState<ComponentDto, ComponentFunction>
{
    Task<bool> Submit(ComponentDto component);
}

public class ComponentState(IComponentDataService componentService)
    : WorkWithState<ComponentDto, ComponentFunction>, IComponentState
{
    public async Task<bool> Submit(ComponentDto component)
    {
        var isNew = component.Id == 0;
        
        var updatedComponent = isNew
            ? await componentService.Create(component)
            : await componentService.Update(component);

        if (updatedComponent == null)
            return false;
        
        if(isNew)
            List.Add(updatedComponent);
        else
        {
            var index = List.ToList().FindIndex(x => x.Id == updatedComponent.Id);
            List[index] = updatedComponent;
        }

        Selected = null;
        
        return true;
    }

    public override async Task Get(QueryParams parameters)
    {
        var page = await componentService.GetPage(parameters);
        List = page.Items.ToList();
        Count = page.Count;
    }

    public override async Task Delete()
    {
        if (Selected == null)
            return;
        
        await componentService.Delete(Selected.FirstOrDefault()?.Id ?? 0);
        
        var toDelete = List.FirstOrDefault(x => x.Id == Selected.FirstOrDefault()?.Id);
        if (toDelete != null)
            List.Remove(toDelete);
        
        OnPropertyChanged();
        
        Selected = null;
    }

    public override void SetSelected(long id)
    {
        Selected = id == 0
            ? [GetNew()]
            : List.Where(f => f.Id == id).ToList();
    }

    protected override ComponentDto GetNew()
    {
        return new ComponentDto { Id = 0 };   
    }
}