using Blazored.LocalStorage;
using Memoriae.BAL.Core.Interfaces;
using Memoriae.Http.AuthentificationService;
using Memoriae.Http.AuthentificationService.Providers;
using Memoriae.Http.Managers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Memoriae.UI.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") });            
            builder.Services.AddScoped<IAuthentificationService, AuthentificationService>();
            builder.Services.AddScoped<IPostManager, PostManager>();
            builder.Services.AddScoped<ITagManager, TagManager>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            await builder.Build().RunAsync();
        }
    }
}
