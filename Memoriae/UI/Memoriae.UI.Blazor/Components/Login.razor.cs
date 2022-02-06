using Memoriae.BAL.User.Core;
using Memoriae.Http.AuthentificationService;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Memoriae.UI.Blazor.Components
{
    public partial class Login
    {
        private User user = new User();

        [Inject]
        public IAuthentificationService AuthentificationService { get; set; }

        private string errors = "Пока нет ошибок";


        private async Task DoLogin()
        {
            try
            {
                var result = await AuthentificationService.Login(user);
                errors = result.Success.ToString();
            }
            catch (Exception ex)
            {
                errors = ex.Message;
            }

        }

        private async Task Register()
        {
            try
            {
                var result = await AuthentificationService.Register(user);
                errors = result.Success.ToString();
            }
            catch (Exception ex)
            {
                errors = ex.Message;
            }

        }
    }
}
