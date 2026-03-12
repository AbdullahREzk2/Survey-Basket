<div align="center">

# 🗳️ SurveyBasket API

**A production-ready polling system backend built with ASP.NET Core Web API**

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-10.0-512BD4?style=for-the-badge&logo=dotnet)](https://docs.microsoft.com/ef/)
[![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=for-the-badge&logo=jsonwebtokens)](https://jwt.io/)

[🚀 Live Demo](#-live-demo) • [📖 Features](#-features) • [🏗️ Architecture](#️-architecture) • [📡 API Endpoints](#-api-endpoints) • [⚙️ Getting Started](#️-getting-started)

</div>

---

## 🚀 Live Demo

| Service | URL | Credentials |
|---------|-----|-------------|
| 📄 API Documentation | [Scalar UI](http://survey--basket.tryasp.net/scalar/v1) | JWT Bearer Token |
| ⚙️ Hangfire Dashboard | [Jobs Dashboard](http://survey--basket.tryasp.net/jobs) | `admin` / `2003` |
| ❤️ Health Checks | [Health Status](http://survey--basket.tryasp.net/health) | Public |

---

## 📖 Features

### 🔐 Authentication & Security
- User registration with **email confirmation**
- JWT-based authentication with **refresh token** flow
- Revoke refresh tokens
- **Forget/Reset password** via email
- Role-based authorization with custom **permission system**
- Rate limiting per IP address

### 🗳️ Polls
- Create, update, soft-delete polls
- Retrieve all polls or by ID
- Poll lifecycle management (start/end dates)

### ❓ Questions & Answers
- Full CRUD for questions per poll
- Multiple answers per question
- **Smart answer sync on update:**
  - Missing answers → `IsActive = false` (soft delete)
  - Existing answers → reactivated
  - New answers → created
- Only **active answers** returned in read operations

### 🗳️ Voting System
- Authenticated users vote on active polls
- Each user can select **only one answer per question**
- Votes stored in normalized structure (`Vote` / `VoteAnswer`)
- Duplicate vote prevention

### 📊 Dashboard & Analytics (Admin)
- Poll statistics overview
- Votes per day chart data
- Votes per question breakdown
- Designed to power **frontend charts and dashboards**

### 👤 User & Role Management
- Full user management (create, update, toggle status)
- Role management with granular **permissions**
- Profile image upload via **Cloudinary**

### 📧 Email System
- HTML email templates
- Email confirmation on registration
- Welcome email after confirmation
- Password reset email
- Background email processing via **Hangfire**

### ⚙️ Background Jobs (Hangfire)
- Daily poll notification job
- All emails processed as background jobs
- Hangfire dashboard with authentication

### 📋 Logging (Serilog)
- Structured JSON logging
- Rolling file logs
- Request/response logging middleware
- Enriched with machine name, thread ID, and context

### ❤️ Health Checks
- Database connectivity check
- Hangfire server check
- Mail service check
- JSON health report endpoint

### Testing
| xUnit | Testing framework |
| Moq | Mocking library |
| FluentAssertions | Readable assertions |

---

## 🏗️ Architecture

The project follows a **3-Layer Clean Architecture**:

```
SurveyBasket/
│
├── SurveyBasket.API/          # Presentation Layer
│   ├── Controllers/           # HTTP Endpoints
│   ├── Infrastructure/        # DI, Middleware, Program.cs
│   ├── Health/                # Health check implementations
│   ├── Services/              # Email, Image services
│   └── Templates/Emails/      # HTML email templates
│
├── SurveyBasket.BLL/          # Business Logic Layer
│   ├── Services/              # Business services
│   ├── DTOs/                  # Request & Response contracts
│   ├── Validators/            # FluentValidation rules
│   ├── Mapping/               # Mapster configurations
│   └── Errors/                # Domain error definitions
│
├── SurveyBasket.DAL/          # Data Access Layer
│   ├── Entities/              # Domain models
│   ├── Repository/            # Repository implementations
│   ├── Persistence/           # DbContext & Migrations
│   └── Enums/                 # Domain enumerations
│
└── SurveyBasket.Shared/       # Shared contracts & utilities
    ├── Results/               # Result pattern
    └── Abstractions/          # Shared interfaces
```

---

## 🛠️ Tech Stack

| Category | Technology |
|----------|-----------|
| Framework | ASP.NET Core 10.0 Web API |
| Database | SQL Server *(migrated from PostgreSQL)* |
| ORM | Entity Framework Core 10.0 |
| Authentication | JWT + Refresh Tokens |
| Mapping | Mapster |
| Validation | FluentValidation |
| Background Jobs | Hangfire |
| Logging | Serilog |
| Image Storage | Cloudinary |
| API Docs | Scalar (OpenAPI) |
| Health Checks | AspNetCore.Diagnostics.HealthChecks |
| Rate Limiting | ASP.NET Core Built-in Rate Limiter |
| CQRS + MediatR | Feature-based request/response pipeline |
| xUnit + Moq | Unit Testing — 62 passing tests |

### Design Patterns
- ✅ Repository Pattern
- ✅ Result Pattern
- ✅ DTO Pattern
- ✅ Options Pattern
- ✅ Background Job Pattern
- ✅ CQRS Pattern (Commands & Queries)
- ✅ Mediator Pattern (MediatR)
- ✅ AAA Pattern (Unit Testing)



---

## 📡 API Endpoints

### 🔐 Authentication
```
POST   /api/auth/login
POST   /api/auth/refresh
POST   /api/auth/revoke-refresh-token
POST   /api/auth/register
GET    /api/auth/confirm-email
POST   /api/auth/resend-confirm-email
POST   /api/auth/forget-password
POST   /api/auth/reset-password
```

### 🗳️ Polls
```
GET    /api/polls
GET    /api/polls/{pollId}
POST   /api/polls
PUT    /api/polls/{pollId}
DELETE /api/polls/{pollId}
```

### ❓ Questions
```
GET    /api/polls/{pollId}/questions
GET    /api/polls/{pollId}/questions/{questionId}
POST   /api/polls/{pollId}/questions
PUT    /api/polls/{pollId}/questions/{questionId}
```

### 🗳️ Voting
```
GET    /api/polls/{pollId}/available-questions
POST   /api/polls/{pollId}/vote
```

### 📊 Dashboard
```
GET    /api/dashboard/polls/{pollId}/statistics
GET    /api/dashboard/polls/{pollId}/votes-per-day
GET    /api/dashboard/polls/{pollId}/votes-per-question
```

### 👤 Account
```
POST   /api/account/profile-image
GET   /api/account/userInfo
PUT   /api/account/update-User-Info
PUT   /api/account/change-password
```

### 👥 Users (Admin)
```
GET    /api/users
GET    /api/users/{userId}
POST   /api/users
PUT    /api/users/{userId}
PUT    /api/users/{userId}/toggle-status
```

### 🔑 Roles (Admin)
```
GET    /api/roles
GET    /api/roles/{roleId}
POST   /api/roles
PUT    /api/roles/{roleId}
```

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/sql-server)
- [Visual Studio 2022 v17.14+](https://visualstudio.microsoft.com/) or VS Code
- [Cloudinary Account](https://cloudinary.com/) *(free tier)*

### 1. Clone the repository
```bash
git clone https://github.com/AbdullahREzk2/SurveyBasket.git
cd SurveyBasket
```

### 2. Set up user secrets
```bash
cd SurveyBasket.API
dotnet user-secrets set "Jwt:key" "YourSuperSecretKeyAtLeast32CharactersLong"
dotnet user-secrets set "MailSettings:Password" "your-email-app-password"
dotnet user-secrets set "HangfireSettings:UserName" "admin"
dotnet user-secrets set "HangfireSettings:Password" "your-hangfire-password"
dotnet user-secrets set "CloudinarySettings:ApiKey" "your-cloudinary-api-key"
dotnet user-secrets set "CloudinarySettings:ApiSecret" "your-cloudinary-api-secret"
```

### 3. Update `appsettings.Development.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.; Database=SurveyBasket; Trusted_Connection=True; TrustServerCertificate=True;",
    "HangfireConnection": "Server=.; Database=SurveyBasket; Trusted_Connection=True; TrustServerCertificate=True;"
  },
  "CloudinarySettings": {
    "CloudName": "your-cloud-name"
  },
  "AppURL": {
    "baseUrl": "https://localhost:7007/"
  }
}
```

### 4. Apply migrations
```bash
dotnet ef database update --project SurveyBasket.DAL --startup-project SurveyBasket.API
```

### 5. Run the project
```bash
dotnet run --project SurveyBasket.API
```

Visit `https://localhost:7007/scalar/v1` for the API documentation.

---

## 🌍 Database Migration Note

This project was originally developed using **PostgreSQL** with `Npgsql.EntityFrameworkCore.PostgreSQL`. It was later migrated to **SQL Server** using `Microsoft.EntityFrameworkCore.SqlServer` to support deployment on shared hosting (ASPMonster). The migration required regenerating all EF Core migrations from scratch.

---

## 👨‍💻 Author

**Abdullah Rezk**
Backend-focused .NET developer

[![GitHub](https://img.shields.io/badge/GitHub-AbdullahREzk2-181717?style=for-the-badge&logo=github)](https://github.com/AbdullahREzk2)

---

<div align="center">
⭐ If you found this project helpful, please consider giving it a star!
</div>
