using FCG.Domain.Dependency;
using FCG.Domain.Entities;
using FCG.Domain.Middleware;
using FCG.Domain.Shared.Config;
using FCG.Domain.Shared.Enum;
using FCG.Domain.Utils;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Dependency;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger com JWT
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fiap.Cloud.Games API", Version = "v1" });

    c.OrderActionsBy(apiDesc => {
        var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];
        return controllerName switch {
            "Perfil" => "01",
            "Login" => "02",
            "Carteira" => "03",
            "Jogos" => "04",
            "Biblioteca" => "05",
            "Promocao" => "06",
            _ => "07"
        };
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Description = "Insira o token JWT no formato: Bearer {seu token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Banco de Dados
builder.Services.AddDbContext<DbFcg>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Validação FluentValidation
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<PerfilRequestValidator>();

// Validação de ModelState
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.InvalidModelStateResponseFactory = context => {
        var errorMessage = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault();

        return new BadRequestObjectResult(errorMessage);
    };
});

// JWT Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

// JWT Auth
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Injeção de Dependências
builder.Services.AddServices();
builder.Services.AddRepositories();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Pipeline
app.UseMiddleware<ValidationMiddleware>();
app.UseMiddleware<ErrorsMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


// Executar migrations
using (var scope = app.Services.CreateScope()) {
    var dbContext = scope.ServiceProvider.GetRequiredService<DbFcg>();
    await dbContext.Database.MigrateAsync();

    var senha = "1234";
    string hash = BCrypt.Net.BCrypt.HashPassword(senha);

    if (!dbContext.USERS.Any()) {
        var usuarioInicial = new PerfilEntity {
            Nome = "Admin",
            Email = "admin@fcg.com",
            SenhaHash = hash,
            Perfil = PerfilEnum.Administrador,
            Habilitado = 1,
            DataCriacao = DateTime.UtcNow
        };

        dbContext.USERS.Add(usuarioInicial);

        await dbContext.SaveChangesAsync();
    }
}


await app.RunAsync();
