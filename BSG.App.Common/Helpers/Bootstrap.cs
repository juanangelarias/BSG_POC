using Blazored.LocalStorage;
using BSG.Common.Model;
using BSG.DataServices;

namespace BSG.App.Common.Helpers;

public class Bootstrap(IUserDataService userDataService, ILocalStorageService localStorage)
{
    private readonly IUserDataService _userDataService = userDataService;

    public async Task<List<Metadata>> GetMetadata()
    {
        var userId = await localStorage.GetItemAsync<long>("userId");
        if (userId == 0)
        {
            userId = await _userDataService.GetUser(userId);
        }
        await localStorage.SetItemAsync("userId", userId);
        
        var metadata = await localStorage.GetItemAsync<List<Metadata>>("metadata");

        if (metadata != null && metadata.Count != 0) 
            return metadata;
        
        metadata = await _userDataService.GetMetadata(userId);
        await localStorage.SetItemAsync("metadata", metadata);

        return metadata;
    }

    public async Task<List<Metadata>> ReloadMetadata()
    {
        await localStorage.RemoveItemAsync("metadata");
        
        return await GetMetadata();
    }

    public async Task<List<Metadata>> ChangeUser()
    {
        var userId = await localStorage.GetItemAsync<long>("userId");
        if (userId == 0)
        {
            userId = await _userDataService.GetUser(userId);
        }

        userId = await _userDataService.GetUser(userId);
        
        await localStorage.SetItemAsync("userId", userId);

        return await GetMetadata();
    }
}