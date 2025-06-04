namespace FCG.Domain.Entities;

public class ILoggerEnt
{
    /// <summary>
    /// Identificador do log.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Mensagem do log.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Detalhes do erro.
    /// </summary>
    public string Error { get; private set; }

    /// <summary>
    /// Data e hora de criação do log.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Nome do host onde o log foi criado.
    /// </summary>
    public string Hostname { get; private set; }

    /// <summary>
    /// Código de identificação do log.
    /// </summary>
    public int CodeIdentify { get; private set; }

    /// <summary>
    /// Construtor privado para a entidade ILoggerEnt.
    /// </summary>
    private ILoggerEnt() { }

    /// <summary>
    /// Construtor privado para a entidade ILoggerEnt.
    /// </summary>
    /// <param name="message">Mensagem do log.</param>
    /// <param name="error">Detalhes do erro.</param>
    /// <param name="codeIdentify">Código de identificação do log.</param>
    private ILoggerEnt(string message, string error, int codeIdentify) {
        Message = message;
        Error = error;
        CreatedAt = DateTime.UtcNow;
        Hostname = Environment.MachineName;
        CodeIdentify = codeIdentify;
    }

    /// <summary>
    /// Método estático para criar uma nova instância de ILoggerEnt.
    /// </summary>
    /// <param name="message">Mensagem do log.</param>
    /// <param name="error">Detalhes do erro.</param>
    /// <param name="codeIdentify">Código de identificação do log.</param>
    /// <returns>Uma nova instância de ILoggerEnt.</returns>
    public static ILoggerEnt Create(string message, string error, int codeIdentify) {
        return new ILoggerEnt(message, error, codeIdentify);
    }
}

