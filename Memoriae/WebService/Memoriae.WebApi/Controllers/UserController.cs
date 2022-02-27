using Memoriae.BAL.User.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Memoriae.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        private readonly IConfigurationSection jwtSettings;

        public UserController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
         
            this.userManager = userManager;
            jwtSettings = configuration.GetSection("JwtSettings");
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="user">Модель пользователя</param>
        /// <returns>Результат регистрации</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null || !ModelState.IsValid) return BadRequest();

            var identityUser = new IdentityUser { UserName = user.Login };
            var result = await userManager.CreateAsync(identityUser, user.Password).ConfigureAwait(false);
            if(!result.Succeeded)
                return BadRequest(new RegistrationResponse { Success = result.Succeeded, Errors = result.Errors.Select(x => x.Description) });
            
            return StatusCode(201);
        }

        /// <summary>
        /// Вход пользователя в систему
        /// </summary>
        /// <param name="user">Модель пользователя</param>
        /// <returns>Результат входа в систему</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var identityUser = await userManager.FindByNameAsync(user.Login);

            if (identityUser == null)
                return Unauthorized(new AuthResponse { Success = false, Error = "Пользователь с таким логином не найден!" });

            if (!await userManager.CheckPasswordAsync(identityUser, user.Password).ConfigureAwait(false))                
                return Unauthorized(new AuthResponse { Success = false, Error = "Неверный пароль!" });           
            
            return Ok(GetAuthResponse(identityUser));
        }       

        private AuthResponse GetAuthResponse(IdentityUser identityUser)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(identityUser);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new AuthResponse { Success = true, Token = token };
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims(IdentityUser user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("validIssuer").Value,
                audience: jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
    }
}
