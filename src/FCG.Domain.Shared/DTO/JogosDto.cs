using System.ComponentModel.DataAnnotations;

namespace FCG.Domain.Shared.DTO;

public class JogosDto
{
    public class JogosDtoRequest
    {
        [Required]
        public string Nome { get; set; } = string.Empty;
        [Required]
        public decimal Preco { get; set; }
        [Required]
        public string Descricao { get; set; } = string.Empty;
        [Required]
        public long Quantidade { get; set; }
        [Required]
        public string Tipo { get; set; } = string.Empty;
    }

    public class JogosDtoResponse
    {
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public long Quantidade { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }

    public class JogosDtoComprarJogos
    {
        public string Nome { get; set; } = string.Empty;
    }
}
