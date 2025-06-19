using BSG.App.User.Model;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.DataServices;
using BSG.States.Base;

namespace BSG.App.User.State;

public interface IUserState: IWorkWithState<UserDto, UserFunction>
{
    Task<bool> Submit(UserDto user);
}

public class UserState(IUserDataService userService): WorkWithState<UserDto, UserFunction>, IUserState
{
    public override async Task Get(QueryParams parameters)
    {
        var page = await userService.GetPage(parameters);
        List = page.Items.ToList();
        Count = page.Count;
    }
    
    public async Task<bool> Submit(UserDto user)
    {
        var isNew = user.Id == 0;

        var updatedUser = isNew
            ? await userService.Create(user)
            : await userService.Update(user);

        if (updatedUser == null)
            return false;
        
        if(isNew)
            List.Add(updatedUser);
        else
        {
            var index = List.ToList().FindIndex(x => x.Id == updatedUser.Id);
            List[index] = updatedUser;
        }

        Selected = null;

        return true;
    }

    public override async Task Delete()
    {
        if(Selected == null)
            return;
        
        await userService.Delete(Selected.FirstOrDefault()?.Id ?? 0);

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
    
    protected override UserDto GetNew()
    {
        return new UserDto { Id = 0 };
    }
}