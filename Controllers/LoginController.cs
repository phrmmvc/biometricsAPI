using Biometrics.Constants;
using Biometrics.Repository;
using Biometrics.Request;
using Biometrics.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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

            try
            {
                List<LoginResponse> responses = _service.isUserExist(request);
                LoginResponse loginResponse;
                if (responses == null)
                {
                    return Ok(new CommonResponseBuilder<string>("")
                    .SetSuccess(false)
                    .SetErrorMessage("Please correct your id and password")
                    .AddMeta("errorCode", 401)
                    .Build()
                    );
                }
                else
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                        audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: signinCredentials
                        );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    CheckUser(request.userId);
                    loginResponse = new LoginResponse
                    {
                        userRoleCode = HttpContext.Session.GetString("UserRoleCode"),
                        userName = HttpContext.Session.GetString("UserName"),
                        empUserId = HttpContext.Session.GetString("UserID"),
                        empId = HttpContext.Session.GetString("Employee_ID"),
                        hBrCod = HttpContext.Session.GetString("BRCode"),
                        branName = HttpContext.Session.GetString("BRNAM"),
                        znCod = HttpContext.Session.GetString("ZNCOD"),
                        userLevelCode = HttpContext.Session.GetString("UserLevelCode"),
                        rmBrCod = HttpContext.Session.GetString("RM_BRCOD"),
                        subBrCod = HttpContext.Session.GetString("SUB_BRCOD"),
                        IsPersistent = responses[0].IsPersistent,
                        Role = responses[0].Role,
                        UserLevelId = responses[0].UserLevelId,
                        LevelPassword = responses[0].LevelPassword,
                        token = tokenString
                    };
                }
                var response = new CommonResponseBuilder<LoginResponse>(loginResponse)
                    .AddMeta("statusCode", 200)
                    .SetErrorMessage("")
                    .Build();
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CommonResponseBuilder<string>("")
                    .SetSuccess(false)
                    .SetErrorMessage(ex.Message)
                    .AddMeta("errorCode", 500)
                    .Build();

                return StatusCode(500, errorResponse);
            }
        }

        private void CheckUser(string userId)
        {
            var UserID = "";
            var listPosting = new List<string>();
            StringBuilder SB = new StringBuilder();
            StringBuilder SB1 = new StringBuilder();
            StringBuilder SB2 = new StringBuilder();
            StringBuilder SB3 = new StringBuilder();
            StringBuilder SB4 = new StringBuilder();

            var dt1 = _service.getUserCurrRole(userId);
            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    HttpContext.Session.SetString("Employee_ID", dt1.Rows[0]["ID"].ToString());
                    HttpContext.Session.SetString("Encp_Employee_ID", _service.Encrypt_QueryString(dt1.Rows[0]["ID"].ToString()));
                    HttpContext.Session.SetString("UserID", dt1.Rows[0]["USERID"].ToString());
                    HttpContext.Session.SetString("UserName", dt1.Rows[0]["EMP_NAME"].ToString());
                    HttpContext.Session.SetString("UserRoleCode", dt1.Rows[0]["TROLE_ID"].ToString());
                    HttpContext.Session.SetString("superbrcod", dt1.Rows[0]["SUP_BR"].ToString());

                    HttpContext.Session.SetString("uBrCod", dt1.Rows[0]["USR_BRCOD"].ToString());
                    HttpContext.Session.SetString("branchtype", dt1.Rows[0]["H_BRTYPE"].ToString());

                    UserID = dt1.Rows[0]["USERID"].ToString();
                    if (dr["USER_LVL"].ToString() == "68")
                    {
                        HttpContext.Session.SetString("UserLevelCode", "68");
                        break;
                    }
                    else if (dr["USER_LVL"].ToString() == "5" || dr["USER_LVL"].ToString() == "45" || dr["USER_LVL"].ToString() == "56")
                    {
                        HttpContext.Session.SetString("UserLevelCode", "5");
                        break;
                    }
                    else if (dr["USER_LVL"].ToString() == "6" || dr["USER_LVL"].ToString() == "15")
                    {
                        HttpContext.Session.SetString("UserLevelCode", "6");
                        break;
                    }
                    else if (dr["TROLE_ID"].ToString() != "")
                    {
                        if (dr["TROLE_ID"].ToString() == "1" && dr["ROLE_STATUS"].ToString() == "4")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "5");
                            break;
                        }
                        else if (dr["TROLE_ID"].ToString() == "35" && dr["ROLE_STATUS"].ToString() == "4")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "5");
                            break;
                        }
                        else if (dr["TROLE_ID"].ToString() == "11" && dr["ROLE_STATUS"].ToString() == "4")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "6");
                            break;
                        }
                        else if (dr["TROLE_ID"].ToString() == "63" && dr["ROLE_STATUS"].ToString() == "4")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "6");
                            break;
                        }

                        else if (dr["TROLE_ID"].ToString() == "52" && dr["ROLE_STATUS"].ToString() == "4")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "6");
                            break;
                        }
                        else if (dr["TROLE_ID"].ToString() == "39" && dr["ROLE_STATUS"].ToString() == "4")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "5");
                            break;
                        }
                        else if (dr["TROLE_ID"].ToString() == "33" && dr["ROLE_STATUS"].ToString() == "4")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "5");
                            break;
                        }

                        else if (dr["TROLE_ID"].ToString() == "66" && dr["ROLE_STATUS"].ToString() == "4")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "5");
                            break;
                        }

                        else if (dr["TROLE_ID"].ToString() == "74" && dr["ROLE_STATUS"].ToString() == "4")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "74");
                            break;
                        }

                        else if (dr["ROLE_STATUS"].ToString() == "4" && dr["TROLE_ID"].ToString() == "9")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "9");
                            break;
                        }
                        else
                        {
                            HttpContext.Session.SetString("UserLevelCode", "99");
                        }
                    }
                    else if (dr["USER_LVL"].ToString() != "")
                    {
                        if (dr["USER_LVL"].ToString() == "5" || dr["USER_LVL"].ToString() == "54")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "5");
                            break;
                        }
                        else if (dr["USER_LVL"].ToString() == "6" || dr["USER_LVL"].ToString() == "15")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "6");
                            break;
                        }
                        else if (dr["USER_LVL"].ToString() == "68")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "68");
                            break;
                        }
                        else if (dr["USER_LVL"].ToString() == "31")
                        {
                            HttpContext.Session.SetString("UserLevelCode", "9");
                            break;
                        }
                        else
                        {
                            HttpContext.Session.SetString("UserLevelCode", "99");

                        }

                    }
                }
                foreach (DataRow dr in dt1.Rows)
                {
                    SB.Append(dr["HR_BRCOD"].ToString() + ",");
                }
                if (dt1.Rows[0]["POWER_TYPE"].ToString() != "")
                {
                    HttpContext.Session.SetString("UserLevelCode", dt1.Rows[0]["POWER_TYPE"].ToString());

                }
                var LvlCod = Convert.ToInt32(HttpContext.Session.GetString("UserLevelCode"));
            }

            var dt2 = _service.getUserDesignation(HttpContext.Session.GetString("Employee_ID"));

            if (dt2.Rows.Count.Equals(1))
            {
                foreach (DataRow dr in dt2.Rows)
                {
                    HttpContext.Session.SetString("des", dt2.Rows[0]["DESIG"].ToString());
                }

            }
            else
            {
                HttpContext.Session.SetString("des", "");
            }

            HttpContext.Session.SetString("HBrCod", SB.ToString().TrimEnd(','));

            if (UserID.ToUpper() == "RAKIBULA")
            {

                // 9535
                HttpContext.Session.SetString("HBrCod", "9198");
                HttpContext.Session.SetString("UserLevelCode", "68");

            }

            var dt3 = _service.getUserBranch(HttpContext.Session.GetString("HBrCod"));

            if (dt3.Rows.Count > 0)
            {
                SB1 = new StringBuilder();
                SB2 = new StringBuilder();
                foreach (DataRow dr in dt3.Rows)
                {
                    SB1.Append(dr["BRCOD"].ToString() + ",");

                    string branchType = dr["BRTYPE"].ToString() == null ? "NULL" : dr["BRTYPE"].ToString();
                    if (branchType == "B")
                        SB2.Append(dr["BRNAM"].ToString() + ",");
                    else
                        SB2.Append(dr["BRNAM"].ToString() + ",");

                    SB4.Append(dr["ZNCOD"].ToString() + ",");

                }
            }

            HttpContext.Session.SetString("BRCode", SB1.ToString().TrimEnd(','));
            HttpContext.Session.SetString("BRNAM", SB2.ToString().TrimEnd(','));
            HttpContext.Session.SetString("ZNCOD", SB4.ToString().TrimEnd(','));

            var dt4 = _service.getRmBrCode(HttpContext.Session.GetString("BRCode"));

            foreach (DataRow dr1 in dt4.Rows)
            {
                SB3.Append(dr1["BRCOD"].ToString() + ",");
            }
            HttpContext.Session.SetString("RM_BRCOD", SB3.ToString().Trim(','));


            var dt5 = _service.getSubBranch(HttpContext.Session.GetString("Employee_ID"));
            if (dt5.Rows.Count > 0)
            {
                if (dt5.Rows[0]["AGPCOD"].ToString() != "")
                {
                    var dt6 = _service.getSubBranchInfo(HttpContext.Session.GetString("BRCode"), dt5.Rows[0]["AGPCOD"].ToString());
                    if (dt6.Rows.Count > 0)
                    {
                        HttpContext.Session.SetString("SUB_BRCOD", dt6.Rows[0]["agpcod"].ToString());
                        HttpContext.Session.SetString("SUB_BRNAM", dt6.Rows[0]["brnam"].ToString());
                    }
                    else
                    {
                        HttpContext.Session.SetString("SUB_BRCOD", "");
                        HttpContext.Session.SetString("SUB_BRNAM", "");
                    }
                }
                else
                {
                    HttpContext.Session.SetString("SUB_BRCOD", "");
                    HttpContext.Session.SetString("SUB_BRNAM", "");
                }
            }
            else
            {
                HttpContext.Session.SetString("SUB_BRCOD", "");
                HttpContext.Session.SetString("SUB_BRNAM", "");
            }

        }

    }
}
