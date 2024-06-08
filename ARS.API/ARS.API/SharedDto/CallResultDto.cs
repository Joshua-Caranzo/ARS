namespace Payroll.API.SharedDto
{
    public class CallResultDto<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public T Data2 { get; set; }
        public int? TotalCount { get; set; }

    }

}
