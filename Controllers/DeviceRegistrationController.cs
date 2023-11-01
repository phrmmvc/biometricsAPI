using Biometrics.Constants;
using Biometrics.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biometrics.Controllers
{

  /*  [ApiController]
    [Route("api/[controller]")]
    public class DeviceRegistrationController : ControllerBase
    {
        private readonly IDeviceRegistrationService _service;

        public DeviceRegistrationController(IDeviceRegistrationService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("device-registration")]
        public IActionResult getSubBranchWiseEmployeeList()
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
*/
}
