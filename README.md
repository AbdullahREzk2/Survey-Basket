# SurveyBasket API

**SurveyBasket** is a personal learning backend project built with **ASP.NET Core Web API**. The project focuses on designing a **clean, scalable backend** for managing polls, questions, and answers using a **3-Layer Architecture** and real-world backend practices.

---

## Project Goals

* Practice real backend architecture (beyond tutorial-style CRUD)
* Apply clean separation of concerns (API / BLL / DAL)
* Implement secure authentication using JWT and refresh tokens
* Design flexible poll and question logic with soft deletion
* Work with PostgreSQL in a production-like environment

---

## Features

### Authentication

* User login
* Refresh token flow
* Revoke refresh token
* JWT-based authentication

### Polls

* Retrieve all polls
* Retrieve a poll by ID
* Create a poll
* Update a poll
* Delete a poll

### Questions

* Retrieve all questions for a poll
* Retrieve a specific question for a poll
* Add a question with multiple answers
* Update a question and its answers

**Update behavior:**

* Answers are **never physically deleted**
* Missing answers → `IsActive = false`
* Existing answers → reactivated
* New answers → created
* Only **active answers** are returned in read operations

---

## Architecture (3-Layer)

The project follows a classic **3-Layer Architecture**, ensuring clear separation of responsibilities:

```
SurveyBasket
│
├── API (Presentation Layer)
│   ├── Controllers
│   ├── Filters & Middleware
│   └── HTTP Endpoints
│
├── BLL (Business Logic Layer)
│   ├── Services
│   ├── DTOs (Contracts)
│   ├── Business Rules
│   └── Validation & Result Pattern
│
├── DAL (Data Access Layer)
│   ├── Entities
│   ├── Repositories
│   └── DbContext (EF Core)
```

Each layer is isolated and communicates only through defined contracts.

---

## Tech Stack

* **Language:** C#
* **Framework:** ASP.NET Core Web API
* **Architecture:** 3-Layer (API / BLL / DAL)
* **Database:** PostgreSQL
* **ORM:** Entity Framework Core
* **Mapping:** Mapster
* **Authentication:** JWT + Refresh Tokens
* **API Documentation:** Scalar
* **Design Patterns:**

  * Repository Pattern
  * Result Pattern
  * DTO Pattern

---

## Business Logic Highlights

* Polls contain multiple questions
* Questions contain multiple answers
* Answers use **soft deletion** via `IsActive`
* Update logic safely synchronizes answers
* Read APIs expose only active answers
* Business rules are enforced in the BLL layer

---

## API Endpoints (Overview)

### Authentication

```
POST   /api/auth/login
POST   /api/auth/refresh-token
POST   /api/auth/revoke-refresh-token
```

### Polls

```
GET    /api/polls
GET    /api/polls/{pollId}
POST   /api/polls
PUT    /api/polls/{pollId}
DELETE /api/polls/{pollId}
```

### Questions

```
GET    /api/polls/{pollId}/questions
GET    /api/polls/{pollId}/questions/{questionId}
POST   /api/polls/{pollId}/questions
PUT    /api/polls/{pollId}/questions/{questionId}
```

---

## Error Handling

* Centralized **Result Pattern** for consistent responses
* Business errors mapped to `ProblemDetails`
* Clear handling for:

  * Not found
  * Validation errors
  * Business rule violations

---

## Author

**Abdullah Rezk**
,,Backend-focused personal learning project using ASP.NET Core
