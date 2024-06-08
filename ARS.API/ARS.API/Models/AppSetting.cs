namespace Payroll.API.Models
{
    public class AppSetting
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public string? SettingName { get; set; }
        public string? Value { get; set; }
    }
}
