using Microsoft.AspNetCore.Mvc;

namespace Payroll.API.Extention
{
    public static class ControllerExtensions
    {
        public static int? GetPayrollId(this ControllerBase controller)
        {
            var personId = controller.User.Claims.SingleOrDefault(c => c.Type == "uid")?.Value;

            return int.TryParse(personId, out var pi) ? pi : null;
        }
    }
}
