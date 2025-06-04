using FCG.Domain.Shared.Enum;
using System.Net.Mail;
using System.Xml.Linq;

namespace FCG.Domain.Entities;

public class PerfilEntity
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public PerfilEnum Perfil { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAlteracao { get; set; }
    public int Habilitado { get; set; } = 1;

    public static PerfilEntity Criar(string nome, string email, string senhaHash, PerfilEnum perfil) {
        return new PerfilEntity {
            Nome = nome,
            Email = email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(senhaHash),
            Perfil = perfil,
            Id = Guid.NewGuid()
        };
    }

    public static PerfilEntity Alterar(string nome, string email, string senhaHash, PerfilEnum perfil, Guid perfilId) {
        return new PerfilEntity {
            Nome = nome,
            Email = email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(senhaHash),
            Perfil = perfil,
            Id = perfilId
        };
    }
}




