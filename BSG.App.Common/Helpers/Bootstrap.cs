using Blazored.LocalStorage;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.DataServices;
using BSG.States;

namespace BSG.App.Common.Helpers;

public class Bootstrap(IUserDataService userDataService, ILocalStorageService localStorage, IGeneralState generalState)
{
    private readonly IUserDataService _userDataService = userDataService;

    public async Task<List<Metadata>> GetMetadata()
    {
        var user = await localStorage.GetItemAsync<UserDto>("user");
        if (user == null || user.Id < 1)
        {
            await localStorage.ClearAsync();
            user = await _userDataService.GetUser(2);
        }

        await localStorage.SetItemAsync("user", user);

        var metadata = await localStorage.GetItemAsync<List<Metadata>>("metadata") ?? [];

        if (metadata.Count != 0)
            return metadata;

        metadata = await _userDataService.GetMetadata(user!.Id);
        await localStorage.SetItemAsync("metadata", metadata);

        return metadata;
    }

    public async Task<List<Metadata>> ReloadMetadata()
    {
        await localStorage.RemoveItemAsync("metadata");

        var metadata =  await GetMetadata();

        generalState.Metadata = metadata;
        
        return metadata;
    }

    public async Task<UserDto?> GetUser()
    {
        var user = await localStorage.GetItemAsync<UserDto>("user");
        return user;
    }

    public async Task<List<Metadata>> ChangeUser()
    {
        var user = await localStorage.GetItemAsync<UserDto>("user");

        long userId =
            user == null
                ? 2
                : user.Id == 2
                    ? 3
                    : 2;

        user = await _userDataService.GetUser(userId);

        await localStorage.SetItemAsync("user", user);

        return await ReloadMetadata();
    }
}