using BSG.App.User.Model;
using BSG.Common.DTO;
using BSG.States.Base;

namespace BSG.App.User.State;

public interface IUserState: IWorkWithState<UserDto, UserFunction>
{
    Task<bool> Submit(UserDto user);
}