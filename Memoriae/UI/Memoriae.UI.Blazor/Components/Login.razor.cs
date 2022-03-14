using Memoriae.Http.AuthentificationService;
using Memoriae.UI.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;
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

        public bool LoaderVisible { get; set; } = false;

        public string Error { get; set; }

        protected override void OnInitialized()
        {
            editContext = new EditContext(user);
        }           

        protected async Task ExecuteLogin(EditContext formContext)
        {
            if(!ValidateForm(formContext)) return;            
            await ExecuteLogin().ConfigureAwait(false);            
        }

        protected void ExecuteGoogleLogin(EditContext formContext)
        {
            if (!ValidateForm(formContext)) return;
        }

        protected async Task ExecuteRegister(EditContext formContext)
        {
            if (!ValidateForm(formContext)) return;
            await ExecuteRegister().ConfigureAwait(false);
        }

        private async Task ExecuteLogin()
        {
            SetLoader();
            var result = await AuthentificationService.Login(new BAL.User.Core.User { Login = user.Login, Password = user.Password });
            if (!result.Success) Error = string.IsNullOrEmpty(result.Error) ? "Ошибка входа" : result.Error;
            user.Password = null;
            CancelLoader();

        }

        private async Task ExecuteRegister()
        {
            SetLoader();
            var result = await AuthentificationService.Register(new BAL.User.Core.User { Login = user.Login, Password = user.Password });
            if (!result.Success) Error = result.Errors.Any() ? string.Join(", ", result.Errors) : "Произошла ошибка при регистрации";

            RegistrationIsOn = false;
            user.Password = null;
            CancelLoader();
        }

        private bool ValidateForm(EditContext formContext)
        {
            Error = null;
            return formContext.Validate();            
        }

        protected void ChangeView()
        {
            RegistrationIsOn = !RegistrationIsOn;
            Error = null;
        }

        protected void SetLoader() => LoaderVisible = true;

        protected void CancelLoader() => LoaderVisible = false;


    }
}
