# Fiap.Cloud.Games

Sistema desenvolvido em C# para gerenciamento de perfis de usuários, jogos, promoções, biblioteca de jogos e saldo de carteira em uma plataforma de games. O projeto utiliza arquitetura em camadas com API RESTful, oferecendo funcionalidades administrativas e de usuários autenticados.

## Sumário

- [Descrição](#descrição)
- [Funcionalidades](#funcionalidades)
- [Como Executar](#como-executar)
- [Exemplos de Uso](#exemplos-de-uso)
- [Contribuição](#contribuição)
- [Licença](#licença)

## Descrição

O Fiap.Cloud.Games é uma aplicação para gestão de jogos, perfis de usuários, promoções, biblioteca de jogos adquiridos por cada usuário e controle de saldo em carteira. Administradores podem cadastrar e gerenciar jogos e promoções, enquanto usuários podem criar perfis, adquirir jogos, consultar sua biblioteca e verificar o saldo da carteira, sempre respeitando regras de autenticação e autorização.

## Funcionalidades

- **Perfis de Usuário**
  - Cadastro de novos perfis.
  - Alteração e exclusão (desativação) de perfis existentes.
  - Autenticação via JWT e controle de acesso por perfil (Administrador/Usuário).

- **Jogos**
  - Cadastro de novos jogos.
  - Alteração e exclusão de jogos.
  - Compra de jogos por usuário autenticado.
  - Consulta de jogos por nome ou listagem completa.

- **Biblioteca do Usuário**
  - **Rota exclusiva para listagem dos jogos adquiridos por cada usuário.**
  - Permite ao usuário visualizar todos os jogos que possui na plataforma.

- **Promoções**
  - Cadastro de promoções para jogos.
  - Consulta de promoções ativas.

- **Carteira**
  - **Rota exclusiva para consulta do saldo disponível do usuário.**
  - Permite ao usuário visualizar quanto de saldo ainda possui para compras na plataforma.

## Como Executar

1. **Pré-requisitos**:
   - [.NET 7.0+](https://dotnet.microsoft.com/download)
   - **SQL Server** instalado e configurado (obrigatório)

2. **Clonando o repositório**:
   ```bash
   git clone https://github.com/giovannialopes/Fiap.Cloud.Games.git
   cd Fiap.Cloud.Games
   ```

3. **Configuração**:
   - Configure a string de conexão no arquivo `appsettings.json` da API apontando para seu SQL Server.

4. **Executando a API**:
   ```bash
   dotnet build
   dotnet run --project src/FCG.Api
   ```

5. **Seed de Usuário Administrador**

   Ao rodar a aplicação, já existe um usuário administrador criado automaticamente no código, conforme está no arquivo `Program`:
   ```csharp
   Email = "admin@fcg.com"
   var senha = "1234";
   ```
   Use esse e-mail e senha para autenticação inicial de administrador.

6. **Acesso**:
   - A API estará disponível em `http://localhost:5000` (ajuste conforme sua configuração).

## Exemplos de Uso

### Cadastro de Perfil (Administrador)
```http
POST /criar/perfil
Content-Type: application/json
Authorization: Bearer {token_admin}

{
  "nome": "João",
  "email": "joao@email.com",
  "senhaHash": "senha123",
  "perfilEnum": "Administrador"
}
```

### Cadastro de Jogo
```http
POST /cadastrar/jogos
Authorization: Bearer {token_admin}
{
  "nome": "Game X",
  "descricao": "Jogo de aventura",
  "preco": 59.90,
  "tipo": "Aventura",
  "quantidade": 100
}
```

### Compra de Jogo
```http
POST /comprar/jogos
Authorization: Bearer {token_usuario}
{
  "nomeJogo": "Game X",
  "quantidade": 1
}
```

### Consulta da Biblioteca do Usuário
```http
GET /usuario/biblioteca
Authorization: Bearer {token_usuario}
```
> Retorna todos os jogos que o usuário autenticado possui.

### Consulta do Saldo da Carteira
```http
GET /usuario/carteira
Authorization: Bearer {token_usuario}
```
> Retorna o saldo atual disponível para o usuário autenticado.
