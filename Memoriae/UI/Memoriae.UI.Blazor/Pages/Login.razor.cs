using Memoriae.Http.AuthentificationService;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Memoriae.UI.Blazor.Pages
{
    public partial class Login
    {
        [Inject]
        public IAuthentificationService AuthentificationService { get; set; }

        private string errors = "Пока нет ошибок";


        private async Task IncrementCount()
        {
            try
            {
                if (AuthentificationService == null)
                    errors = "AuthentificationService is null";

                var result = await AuthentificationService.Login(new BAL.User.Core.User { Login = "login", Password = "password" });
                errors = result.Success.ToString();
            }
            catch(Exception ex)
            {
                errors = ex.Message;
            }
            
        }
    }
}
