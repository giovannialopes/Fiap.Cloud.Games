namespace FCG.Domain.Shared.DTO;

public class ErrorDto
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
