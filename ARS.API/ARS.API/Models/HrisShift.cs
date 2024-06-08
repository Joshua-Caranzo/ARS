namespace Payroll.API.Models
{
    public class HrisShift
    {
        public int Id { get; set; }

        public string SHFT_CODE { get; set; }

        public string? SHFT_DESC { get; set; }

        public string SHFT_TPOLICY_CODE { get; set; }
        public TimePolicy TimePolicy { get; set; }

        public string? SHFT_TYPE { get; set; }

        public int SHFT_MSWIPE { get; set; } = 0;

        public decimal SHFT_REG_HRS { get; set; } = 0m;

        public int SHFT_MANDATORY_SWIPE { get; set; } = 0;

        public string? SHFT_TM_IN_1 { get; set; }

        public string? SHFT_TM_OUT_1 { get; set; }

        public string? SHFT_TM_IN_2 { get; set; }

        public string? SHFT_TM_OUT_2 { get; set; }

        public string? SHFT_TM_IN_3 { get; set; }

        public string? SHFT_TM_OUT_3 { get; set; }

        public string? SHFT_TM_IN_4 { get; set; }

        public string? SHFT_TM_OUT_4 { get; set; }

        public string? SHFT_CREA_BY { get; set; }

        public DateTime? SHFT_CREA_DT { get; set; }

        public string? SHFT_MODI_BY { get; set; }

        public DateTime? SHFT_MODI_DT { get; set; }
        
    }
}
