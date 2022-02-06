using Blazored.LocalStorage;
using Memoriae.BAL.User.Core;
using Memoriae.Http.AuthentificationService.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Memoriae.Http.AuthentificationService
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly HttpClient httpClient;        
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly ILocalStorageService localStorage;

        public AuthentificationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.authStateProvider = authStateProvider;
            this.localStorage = localStorage;
        }

        public async Task<RegistrationResponse> Register(User user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync($"user/register", content).ConfigureAwait(false);
            var data = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            return !responseMessage.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<RegistrationResponse>(data)
                : new RegistrationResponse { Success = true };
        }

        public async Task<AuthResponse> Login(User user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync($"user/login", content).ConfigureAwait(false);
            var data = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<AuthResponse>(data);
            if (!responseMessage.IsSuccessStatusCode) return result;

            await localStorage.SetItemAsync("authToken", result.Token);
            ((AuthStateProvider)authStateProvider).NotifyUserAuthentication(user.Login);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            return result;
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)authStateProvider).NotifyUserLogout();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }       
    }
}
