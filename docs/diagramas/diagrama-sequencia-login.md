# Diagrama de Sequência de Login

Este diagrama descreve o fluxo de autenticação quando o usuário realiza login no Habitus.

```mermaid
sequenceDiagram
    actor Usuario as Usuário
    participant Frontend
    participant AuthController
    participant AuthService
    participant AppDbContext
    database Banco as Banco de Dados

    Usuario->>Frontend: Informa email e senha
    Frontend->>AuthController: POST /api/Auth/login
    AuthController->>AuthService: LoginAsync(request)
    AuthService->>AppDbContext: Consulta usuário por email
    AppDbContext->>Banco: Executa consulta
    Banco-->>AppDbContext: Retorna dados do usuário
    AppDbContext-->>AuthService: Retorna usuário encontrado
    AuthService->>AuthService: Valida senha com BCrypt

    alt Credenciais válidas
        AuthService-->>AuthController: Retorna sucesso
        AuthController-->>Frontend: HTTP 200 com dados do usuário
        Frontend-->>Usuario: Exibe dashboard
    else Credenciais inválidas
        AuthService-->>AuthController: Retorna erro
        AuthController-->>Frontend: HTTP 401
        Frontend-->>Usuario: Exibe mensagem de erro
    end
```
