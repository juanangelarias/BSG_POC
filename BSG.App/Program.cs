using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using BSG.App.Common.ErrorHandling;
using BSG.App.User.State;
using BSG.DataServices;
using BSG.DataServices.Auth;
using BSG.DataServices.Helper;
using BSG.States;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

namespace BSG.App;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services
            .AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7260/") }); //new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services
            .AddBlazoredLocalStorage(config =>
            {
                config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                config.JsonSerializerOptions.WriteIndented = false;
            })
            .AddBlazoredSessionStorage(config =>
            {
                config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                config.JsonSerializerOptions.WriteIndented = false;
            });
        
        // State Providers
        builder.Services
            // G
            .AddScoped<IGeneralState, GeneralState>()
            // R
            .AddRadzenComponents()
            .AddRadzenCookieThemeService()
            .AddRadzenQueryStringThemeService()
            // U
            .AddScoped<IUserState, UserState>();
            
        // DataServices
        builder.Services
            // C
            .AddScoped<IComponentDataService, ComponentDataService>()
            //
            .AddScoped<IElementDataService, ElementDataService>()
            // P
            .AddScoped<IProductDataService, ProductDataService>()
            .AddScoped<IProductTypeDataService, ProductTypeDataService>()
            // U
            .AddScoped<IUserDataService, UserDataService>();

        builder.Services
            .AddAuthorizationCore()
            .AddScoped<AuthenticationStateProvider, CustomAuthState>()
            .AddScoped<IErrorHandler, ErrorHandler>()
            .AddScoped<IExceptionRecorderService, ExceptionRecorderService>();

        await builder.Build().RunAsync();
    }
}