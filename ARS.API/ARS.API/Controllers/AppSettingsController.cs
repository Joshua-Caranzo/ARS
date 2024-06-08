using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.API.Services.AppSettings;
using Payroll.API.Extention;

namespace Payroll.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AppSettingsController(IAppSettingsService appSettingsService) : ControllerBase
    {
        private readonly IAppSettingsService _appSettingsService = appSettingsService;

        [HttpGet]
        public async Task<ActionResult> GetAppSetting(string settingName, CancellationToken cancellationToken)
        {
            var personId = 1;// this.GetPayrollId();
           // if (personId is null) return Unauthorized();
            var setting = await _appSettingsService.GetAppSetting(personId, settingName, cancellationToken);
            return Ok(setting);
        }
    }
}
