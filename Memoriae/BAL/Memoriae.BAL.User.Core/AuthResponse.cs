namespace Memoriae.BAL.User.Core
{
    public class AuthResponse
    {

        public bool Success { get; set; }

        public bool PasswordCheckFailed { get; set; }

        public string Token { get; set; }
    }
}
