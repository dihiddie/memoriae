using Memoriae.Http.AuthentificationService;
using Memoriae.UI.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace Memoriae.UI.Blazor.Components
{
    public partial class Login
    {
        private User user = new User();

        private EditContext editContext;

        [Inject]
        public IAuthentificationService AuthentificationService { get; set; }

        protected override void OnInitialized()
        {
            editContext = new EditContext(user);
        }   
     

        private string errors;


        private async Task ExecuteLogin()
        {
            try
            {
                var result = await AuthentificationService.Login(new BAL.User.Core.User { Login = user.Login, Password = user.Password });
                errors = result.Success.ToString();
            }
            catch (Exception ex)
            {
                errors = ex.Message;
            }

        }

        private async Task ExecuteRegister()
        {
            try
            {
                var result = await AuthentificationService.Register(new BAL.User.Core.User { Login = user.Login, Password = user.Password });
                errors = result.Success.ToString();
            }
            catch (Exception ex)
            {
                errors = ex.Message;
            }

        }
    }
}
