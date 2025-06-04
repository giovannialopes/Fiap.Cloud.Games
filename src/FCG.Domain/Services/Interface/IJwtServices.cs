namespace FCG.Domain.Services.Class;

public interface IJwtServices
{
    string GenerateToken(Guid userId, string role);
}
