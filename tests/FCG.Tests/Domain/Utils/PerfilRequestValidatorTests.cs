using FCG.Domain.Shared.DTO;
using FCG.Domain.Utils;
using FluentValidation.TestHelper;

namespace FCG.Tests.Domain.Utils;

public class PerfilRequestValidatorTests
{
    private readonly PerfilRequestValidator _validator = new();

    [Fact]
    public void Deve_Aceitar_PerfilRequest_Valido() {
        var model = new PerfilDto.PerfilRequest {
            Nome = "Usuário Válido",
            Email = "usuario@teste.com",
            SenhaHash = "senha123"
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Deve_Falhar_Quando_Nome_Eh_Vazio() {
        var model = new PerfilDto.PerfilRequest {
            Nome = "",
            Email = "usuario@teste.com",
            SenhaHash = "senha123"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Nome)
            .WithErrorMessage("O nome é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Nome_Maior_Que_100() {
        var model = new PerfilDto.PerfilRequest {
            Nome = new string('a', 101),
            Email = "usuario@teste.com",
            SenhaHash = "senha123"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Nome)
            .WithErrorMessage("O nome não pode ter mais de 100 caracteres.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Email_Eh_Vazio() {
        var model = new PerfilDto.PerfilRequest {
            Nome = "Usuário",
            Email = "",
            SenhaHash = "senha123"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("O e-mail é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Email_Eh_Invalido() {
        var model = new PerfilDto.PerfilRequest {
            Nome = "Usuário",
            Email = "email-invalido",
            SenhaHash = "senha123"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("O e-mail deve ser válido.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Senha_Eh_Vazia() {
        var model = new PerfilDto.PerfilRequest {
            Nome = "Usuário",
            Email = "usuario@teste.com",
            SenhaHash = ""
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.SenhaHash)
            .WithErrorMessage("A senha é obrigatória.");
    }
}
