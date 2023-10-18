using Biometrics.Repository;
using Biometrics.Request;
using Biometrics.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Biometrics.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {

            if (string.IsNullOrEmpty(request.userId) || string.IsNullOrEmpty(request.password))
            {
                return BadRequest("Invalid user request!!!");
            }
            List<LoginResponse> responses = _service.isUserExist(request);

            if (responses == null)
            {
                return Unauthorized(new String("Please correct your id and password"));
            }
            else
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                    audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                    );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new LoginResponse
                {
                    IsPersistent = responses[0].IsPersistent,
                    Role = responses[0].Role,
                    UserLevelId = responses[0].UserLevelId,
                    LevelPassword = responses[0].LevelPassword,
                    token = tokenString
                });
            }
        }
    }
}
