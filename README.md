# Habitus

## Descrição

Habitus é uma aplicação web de rastreamento de hábitos, desenvolvida como projeto acadêmico da disciplina de Desenvolvimento de Sistemas.

## Objetivo

O sistema permite cadastrar usuários, fazer login, criar hábitos, acompanhar hábitos por data, marcar hábitos como concluídos e visualizar métricas simples de desempenho diário.

## Tecnologias utilizadas

- C# .NET 8 Web API
- PostgreSQL
- Docker
- Entity Framework Core
- Swagger / OpenAPI
- HTML5
- CSS3
- JavaScript
- xUnit
- BCrypt.Net-Next
- Git e GitHub

## Funcionalidades

- Cadastro de usuário
- Login
- Hash de senha
- Cadastro de hábitos
- Listagem de hábitos
- Busca por data
- Edição de hábitos
- Exclusão de hábitos
- Marcar/desmarcar hábito como concluído
- Métricas diárias
- Testes automatizados

## Estrutura do projeto

```text
Habitus/
├── Habitus.Api/
├── Habitus.Frontend/
├── Habitus.Tests/
├── docs/
├── docker-compose.yml
├── README.md
```

## Como executar o projeto

### Requisitos necessários

- .NET 8 SDK
- Docker Desktop
- Git
- VS Code recomendado

### Clonar o repositório

```bash
git clone https://github.com/HugoDeleon21/habitus.git
cd habitus
```

### Subir o banco de dados

```bash
docker compose up -d
```

### Rodar a API

```bash
cd Habitus.Api
dotnet run
```

### Acessar o Swagger

```text
http://localhost:5112/swagger
```

### Abrir o frontend

Abra o arquivo `Habitus.Frontend/index.html` com a extensão Live Server no VS Code.

## Endpoints principais

- `POST /api/Auth/register`
- `POST /api/Auth/login`
- `POST /api/habitos`
- `GET /api/habitos`
- `GET /api/habitos/{id}`
- `GET /api/habitos/data/{data}`
- `PUT /api/habitos/{id}`
- `DELETE /api/habitos/{id}`
- `PATCH /api/habitos/{id}/concluir`
- `GET /api/habitos/metricas/dia/{data}`
- `GET /api/Health`

## Testes automatizados

Para executar os testes automatizados, rode o comando abaixo na raiz do projeto:

```bash
dotnet test
```

O projeto possui 6 testes automatizados usando xUnit.

## Banco de dados

O PostgreSQL roda em um container Docker definido no arquivo `docker-compose.yml`. O acesso ao banco de dados é feito pelo backend por meio do Entity Framework Core.

## Arquitetura

O backend está organizado de forma simples, separando responsabilidades em:

- Controllers
- Services
- DTOs
- Models
- Data
- Middlewares

## Autor

Desenvolvido por Hugo Deleon.
