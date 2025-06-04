namespace FCG.Domain.Entities;

public class BibliotecaEntity
{
    public Guid Id { get; set; }
    public Guid JogoId { get; set; }
    public Guid UsuarioId { get; set; }

    public static BibliotecaEntity Criar(Guid jogoid, Guid usuarioid) {
        return new BibliotecaEntity {
            Id = Guid.NewGuid(),
            JogoId = jogoid,
            UsuarioId = usuarioid

        };
    }
}
