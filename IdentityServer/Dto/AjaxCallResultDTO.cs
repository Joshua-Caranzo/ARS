namespace Identity.API.Dto
{
    public class AjaxCallResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Contents { get; set; }
        public int StatusCode { get; set; }

        public AjaxCallResultDTO() { }

        public AjaxCallResultDTO(bool success, string message, object contents, int statusCode)
        {
            Success = success;
            Message = message;
            Contents = contents;
            StatusCode = statusCode;
        }
    }

}
