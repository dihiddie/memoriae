using Memoriae.BAL.User.Core;
using System.Threading.Tasks;

namespace Memoriae.Http.AuthentificationService
{
    public interface IAuthentificationService
    {
        Task<RegistrationResponse> Register(User user);

        Task<AuthResponse> Login(User user);

        Task Logout();
    }
}
