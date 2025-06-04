namespace FCG.Domain.Shared.DTO;

public class CarteiraDto
{
    public class CarteiraDtoRequest
    {
        public Guid UsuarioId { get; set; }
        public decimal Saldo { get; set; }
    }

    public class CarteiraDtoResponse
    {
        public decimal Saldo { get; set; }
    }
}
