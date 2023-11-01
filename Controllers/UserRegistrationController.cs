using Biometrics.Constants;
using Biometrics.Repository;
using Biometrics.Request;
using Biometrics.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Biometrics.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserRegistrationController : ControllerBase
    {
        private readonly IUserRegistrationService _service;

        public UserRegistrationController(IUserRegistrationService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("all-zone")]
        public IActionResult getZoneList(string znCod, string userLevelCode, string userLevelId, string userLevelPassword)
        {
            if (string.IsNullOrEmpty(znCod))
            {
                return BadRequest("Invalid user request!!!");
            }
            try
            {
                List<SelectListItem> responses = _service.getAllZone(znCod,userLevelCode,userLevelId,userLevelPassword);
                List<ZoneResponse> zoneResponses = new List<ZoneResponse>();
                if (responses == null)
                {
                    return Ok(new CommonResponseBuilder<string>("")
                    .SetSuccess(false)
                    .SetErrorMessage("No data found")
                    .AddMeta("errorCode", 404)
                    .Build()
                    );
                }
                else
                {
                    for (int i = 0; i < responses.Count; i++)
                    {
                        zoneResponses.Add(new ZoneResponse(responses[i].Text, responses[i].Value));
                    }
                }
                var response = new CommonResponseBuilder<List<ZoneResponse>>(zoneResponses)
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
        
        [Authorize]
        [HttpGet("all-branch")]
        public IActionResult getBranchList(string znCod, string brCod, string userLevelCode, string userLevelId, string userLevelPassword)
        {
            if (string.IsNullOrEmpty(znCod))
            {
                return BadRequest("Invalid user request!!!");
            }
            try
            {
                List<SelectListItem> responses = _service.getAllBranchByZoneCode(znCod, brCod,userLevelCode,userLevelId,userLevelPassword);
                List<BranchResponse> branchResponseList = new List<BranchResponse>();
                if (responses == null)
                {
                    return Ok(new CommonResponseBuilder<string>("")
                    .SetSuccess(false)
                    .SetErrorMessage("No data found")
                    .AddMeta("errorCode", 404)
                    .Build()
                    );
                }
                else
                {
                    for (int i = 0; i < responses.Count; i++)
                    {
                        branchResponseList.Add(new BranchResponse(responses[i].Text, responses[i].Value));
                    }
                }
                var response = new CommonResponseBuilder<List<BranchResponse>>(branchResponseList)
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
         
        [Authorize]
        [HttpGet("all-sub-branch")]
        public IActionResult getSubBranchList(string znCod, string brCod,string subBrCod, string userLevelCode, string userLevelId, string userLevelPassword)
        {
            if (string.IsNullOrEmpty(znCod))
            {
                return BadRequest("Invalid user request!!!");
            }
            try
            {
                List<SelectListItem> responses = _service.getAllSubBranchByBranchCode(znCod, brCod,subBrCod, userLevelCode,userLevelId,userLevelPassword);
                List<BranchResponse> branchResponseList = new List<BranchResponse>();
                if (responses == null)
                {
                    return Ok(new CommonResponseBuilder<string>("")
                    .SetSuccess(false)
                    .SetErrorMessage("No data found")
                    .AddMeta("errorCode", 404)
                    .Build()
                    );
                }
                else
                {
                    for (int i = 0; i < responses.Count; i++)
                    {
                        branchResponseList.Add(new BranchResponse(responses[i].Text, responses[i].Value));
                    }
                }
                var response = new CommonResponseBuilder<List<BranchResponse>>(branchResponseList)
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
        
        [Authorize]
        [HttpGet("branch-wise-employee-list")]
        public IActionResult getSubBranchWiseEmployeeList(string znCod, string brCod,string subBrCod, string userLevelCode, string userLevelId, string userLevelPassword, string empId)
        {
            if (string.IsNullOrEmpty(znCod))
            {
                return BadRequest("Invalid user request!!!");
            }
            try
            {
                List<SelectListItem> responses = _service.getAllEmployeeByBranchReportWithTerminal(znCod, brCod,subBrCod, userLevelCode,userLevelId,userLevelPassword, empId);
                List<BranchResponse> branchResponseList = new List<BranchResponse>();
                if (responses == null)
                {
                    return Ok(new CommonResponseBuilder<string>("")
                    .SetSuccess(false)
                    .SetErrorMessage("No data found")
                    .AddMeta("errorCode", 404)
                    .Build()
                    );
                }
                else
                {
                    for (int i = 0; i < responses.Count; i++)
                    {
                        branchResponseList.Add(new BranchResponse(responses[i].Text, responses[i].Value));
                    }
                }
                var response = new CommonResponseBuilder<List<BranchResponse>>(branchResponseList)
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
    }
}
