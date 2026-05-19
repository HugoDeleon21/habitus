# Diagrama de Sequência de Criação de Hábito

Este diagrama descreve o fluxo de criação de um novo hábito pelo usuário.

```mermaid
sequenceDiagram
    actor Usuario as Usuário
    participant Frontend
    participant HabitosController
    participant HabitoService
    participant AppDbContext
    database Banco as Banco de Dados

    Usuario->>Frontend: Preenche nome, descrição e data
    Frontend->>HabitosController: POST /api/habitos
    HabitosController->>HabitoService: CriarAsync(request)
    HabitoService->>HabitoService: Valida dados e UsuarioId
    HabitoService->>AppDbContext: Adiciona hábito
    AppDbContext->>Banco: Salva alterações
    Banco-->>AppDbContext: Confirma gravação
    AppDbContext-->>HabitoService: Retorna hábito salvo
    HabitoService-->>HabitosController: Retorna hábito criado
    HabitosController-->>Frontend: HTTP 201 com hábito criado
    Frontend-->>Usuario: Atualiza lista de hábitos
```
