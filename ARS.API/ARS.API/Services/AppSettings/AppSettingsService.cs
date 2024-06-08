using Microsoft.AspNetCore.Connections;
using Payroll.API.Context;
using Payroll.API.Services.User.Dto;
using Payroll.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Payroll.API.Services.AppSettings
{
    public class AppSettingsService(ParyollContext payrollcontext) : IAppSettingsService
    {
        private readonly ParyollContext _payrollcontext = payrollcontext;

        public async Task<string?> GetAppSetting(int personId, string settingName, CancellationToken cancellationToken)
        {

            var appsetting = await _payrollcontext.AppSettings
                .Where(setting => setting.PersonID == personId && setting.SettingName == settingName)
                .Select(setting => setting.Value)
                .FirstOrDefaultAsync(cancellationToken);

            return appsetting;
        }
    }
}
