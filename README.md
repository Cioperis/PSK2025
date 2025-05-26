# PSK2025 Emotional Support Project

## 🛠️ Status

[![CI](https://github.com/Cioperis/PSK2025/actions/workflows/CI.yml/badge.svg)](https://github.com/Cioperis/PSK2025/actions/workflows/CI.yml)
[![Vulnerability Check](https://github.com/Cioperis/PSK2025/actions/workflows/Vuln-Check.yml/badge.svg)](https://github.com/Cioperis/PSK2025/actions/workflows/Vuln-Check.yml)

## 🚀 Technologies Used
- .NET 9 with .NET Aspire
- React Vite (Frontend Framework)
- Serilog (Structured Logging)
- PostgreSQL (Database)
- MongoDB (NoSQL/ for audit)
- Hangfire (Background and scheduled job processing)
- JWT Authentication (User authentication & authorization)
- Redis (Distributed cache and Hangfire job storage option)
- Entity Framework Core (ORM)
- CI + Vulnerability check
- RabbitMQ (Message broker for asynchronous communication and task queuing)

## 🏗️ Project Structure and Layers
This project follows a layered architecture model following an orchestrational software pattern:

- **ApiService layer** (Repos/Services/API Controllers): Manages HTTP requests and service/repository logic.
- **ServiceDefaults layer**: Contains shared models, DTOs, utilities, and helpers.
- **MigrationService layer**: Handles database operations using Entity Framework Core.
- **AppHost layer**: Orchestrational layer. Currently starts up the API, database and React frontend.
- **Web layer**: React-based frontend application.
- **AutoMessageService**: Email notifications.

## 🛠️ How to Launch the Project

### 🚀 Run the application 
1. **Clone the Repository**:
    ```bash
    git clone <repository-url>
    ```

2. **Open the Solution**:
    - Open `PSK.sln` with Visual Studio 2022+.

3. **Restore Dependencies**:
    - Restore NuGet packages (Visual Studio usually prompts this automatically).

4. **Go to migration service layer**:
    ```bash
    cd .\PSK.MigrationService\
    ```

5. **Apply Migrations** (if needed):
    ```bash
    dotnet ef database update
    ```

6. **Run the Application**:
    - Press `F5` or use the `dotnet run` command.

## 🌳 Branching Strategy

Following SCRUM-based branch naming conventions:

- **Feature Branches**:
  - Naming: `f/SCRUM-{task_number}`
  - Example: `f/SCRUM-101`

- **Hotfix Branches**:
  - Naming: `h/SCRUM-{task_number}`
  - Example: `h/SCRUM-102`

- **Main Branch**:
  - `main` branch must always contain working stable code.

- **Merge Requests (MRs)**:
  - All branches must go through Pull Requests with at least 1 approval before merging.

## 📋 Additional Notes
- Follow consistent commit message format:
  ```
  SCRUM-{task_number}: short description
  ```
- Ensure working and stable code.
- Apply database migrations carefully and track changes properly.