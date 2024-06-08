namespace Payroll.API.Models
{
    public class TimePolicy
    {
        public int Id { get; set; }

        public string TPOL_CODE { get; set; }

        public string? TPOL_DESC { get; set; }

        public int TPOL_FLEX_TIME { get; set; }

        public string? TPOL_FLEX_FROM { get; set; }

        public string? TPOL_FLEX_TO { get; set; }

        public int TPOL_NT_DIFF { get; set; }

        public string? TPOL_NT_DIFF_FROM { get; set; }

        public string? TPOL_NT_DIFF_TO { get; set; }

        public int TPOL_LATE_APP_1 { get; set; }

        public int TPOL_LATE_APP_2 { get; set; }

        public int TPOL_LATE_APP_3 { get; set; }

        public int TPOL_LATE_APP_4 { get; set; }

        public int TPOL_UT_APP_1 { get; set; }

        public int TPOL_UT_APP_2 { get; set; }

        public int TPOL_UT_APP_3 { get; set; }

        public int TPOL_UT_APP_4 { get; set; }

        public int TPOL_OT_NEED_REQUEST { get; set; }

        public decimal? TPOL_OT_START_ALLOW { get; set; }

        public decimal? TPOL_OT_ROUND { get; set; }

        public decimal? TPOL_OT_MIN_HRS { get; set; }

        public decimal? TPOL_OT_MAX_HRS { get; set; }

        public int TPOL_OT_APPLY_8HRS_RULE { get; set; }

        public int TPOL_EARLY_OT_ALLOWED { get; set; }

        public decimal? TPOL_EARLY_OT_MIN { get; set; }

        public decimal? TPOL_EARLY_OT_MAX { get; set; }

        public int? TPOL_COMPUTE_BY_PERIOD { get; set; }

        public string? TPOL_CREA_BY { get; set; }

        public DateTime? TPOL_CREA_DT { get; set; }

        public string? TPOL_MODI_BY { get; set; }

        public DateTime? TPOL_MODI_DT { get; set; }

        public int? TPOL_LATE_ALLOW_1 { get; set; }

        public int? TPOL_LATE_ALLOW_2 { get; set; }

        public int? TPOL_LATE_ALLOW_3 { get; set; }

        public int? TPOL_LATE_ALLOW_4 { get; set; }

        public int? TPOL_LATE_MAX { get; set; }

        public int? TPOL_UT_ALLOW_1 { get; set; }

        public int? TPOL_UT_ALLOW_2 { get; set; }

        public int? TPOL_UT_ALLOW_3 { get; set; }

        public int? TPOL_UT_ALLOW_4 { get; set; }

        public int? TPOL_UT_MAX { get; set; }
        public ICollection<HrisShift> Shifts { get; set; }
    }
}
