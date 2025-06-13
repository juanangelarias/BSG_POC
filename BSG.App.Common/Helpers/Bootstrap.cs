using Blazored.LocalStorage;
using BSG.Common.DTO;
using BSG.DataServices;

namespace BSG.App.Common.Helpers;

public class Bootstrap(IComponentDataService componentDataService, ILocalStorageService localStorage)
{
    private readonly IComponentDataService _componentDataService = componentDataService;

    public async Task<List<ComponentDto>> GetMetadata()
    {
        var metadata = await localStorage.GetItemAsync<List<ComponentDto>>("metadata");

        if (metadata != null && metadata.Count != 0) 
            return metadata;
        
        metadata = await _componentDataService.GetExtended();
        await localStorage.SetItemAsync("metadata", metadata);

        return metadata;
    }

    public async Task<List<ComponentDto>> ReloadMetadata()
    {
        await localStorage.RemoveItemAsync("metadata");
        var metadata = await _componentDataService.GetExtended();
        await localStorage.SetItemAsync("metadata", metadata);

        return metadata;
    }
}