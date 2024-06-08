namespace Payroll.API.Services.AppSettings
{
    public interface IAppSettingsService
    {
        Task<string?> GetAppSetting(int personId, string settingName, CancellationToken cancellationToken);
    }
}
