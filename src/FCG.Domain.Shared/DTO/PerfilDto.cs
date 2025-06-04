using FCG.Domain.Shared.Enum;

namespace FCG.Domain.Shared.DTO;

public class PerfilDto
{
    public class PerfilRequest
    { 
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public PerfilEnum PerfilEnum { get; set; } 
    }

    public class PerfilResponse
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
    }


}
