using System.Text.Json.Serialization;

namespace FCG.Domain.Shared.DTO;

public class LoginDto
{
    public class LoginDtoRequest
    {
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
    }

    public class LoginDtoResponse
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;

    }
}
