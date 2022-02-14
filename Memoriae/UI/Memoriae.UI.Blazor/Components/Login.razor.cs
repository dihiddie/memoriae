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

        public bool RegistrationIsOn { get; set; }

        protected override void OnInitialized()
        {
            editContext = new EditContext(user);
        }      

        private async Task ExecuteLogin()
        {
            try
            {
                var result = await AuthentificationService.Login(new BAL.User.Core.User { Login = user.Login, Password = user.Password });                
            }
            catch (Exception ex)
            {
                
            }

        }

        private async Task ExecuteRegister()
        {
            try
            {
                var result = await AuthentificationService.Register(new BAL.User.Core.User { Login = user.Login, Password = user.Password });                
                
                RegistrationIsOn = false;
                user = new User();
            }
            catch (Exception ex)
            {
            }

        }

        protected async Task ExecuteLogin(EditContext formContext)
        {
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;

            await ExecuteLogin().ConfigureAwait(false);
            
        }

        protected async Task ExecuteRegister(EditContext formContext)
        {
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;

            await ExecuteRegister().ConfigureAwait(false);

        }

        protected void ChangeView() => RegistrationIsOn = !RegistrationIsOn;
    }
}
