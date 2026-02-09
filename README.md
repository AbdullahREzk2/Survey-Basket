# SurveyBasket API

**SurveyBasket** is a backend-focused personal learning project built with **ASP.NET Core Web API**. The project is designed to simulate a **real-world polling system**, with a strong emphasis on **clean architecture**, **business rules**, and **analytics-ready data** rather than simple CRUD operations.

---

## Project Goals

* Practice real backend architecture beyond tutorial-level CRUD
* Apply clean separation of concerns (API / BLL / DAL)
* Implement secure authentication using JWT and refresh tokens
* Design flexible poll, question, and voting logic with clear business rules
* Build dashboard-ready statistics and analytics endpoints
* Work with PostgreSQL in a production-like environment

---

## Features

### Authentication

* User login
* JWT-based authentication
* Refresh token flow
* Revoke refresh token

---

### Polls

* Retrieve all polls
* Retrieve a poll by ID
* Create a poll
* Update a poll
* Soft delete a poll

---

### Questions & Answers

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

### Voting System

* Authenticated users can vote on polls
* Each user can select **only one answer per question**
* Voting is tied to the authenticated user (not anonymous)
* Votes are stored in a normalized structure (`Vote` / `VoteAnswer`)

---

### Dashboard & Statistics (Admin)

The project includes **analytics-ready endpoints** intended for an **admin dashboard** (admin authorization will be enforced later).

#### Supported Statistics

* **Poll statistics** (per poll)
* **Votes per day** for a poll
* **Votes per question** for a poll

These statistics are designed to support charts and dashboards on the frontend.

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
│   ├── Validation
│   └── Result Pattern
│
├── DAL (Data Access Layer)
│   ├── Entities
│   ├── Repositories
│   └── DbContext (EF Core)
```

Each layer communicates only through well-defined contracts, ensuring maintainability and testability.

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

### Design Patterns

* Repository Pattern
* Result Pattern
* DTO Pattern

---

## Business Logic Highlights

* Polls contain multiple questions
* Questions contain multiple answers
* Answers use **soft deletion** via `IsActive`
* Each user can vote **once per question**
* Votes are linked to users
* Update logic safely synchronizes answers
* Read operations return only active answers
* All business rules are enforced in the BLL layer

---

## API Endpoints (Overview)

### Authentication

```
POST   /api/auth/login
POST   /api/auth/refresh-token
POST   /api/auth/revoke-refresh-token
```

---

### Polls

```
GET    /api/polls
GET    /api/polls/{pollId}
POST   /api/polls
PUT    /api/polls/{pollId}
DELETE /api/polls/{pollId}
```

---

### Questions

```
GET    /api/polls/{pollId}/questions
GET    /api/polls/{pollId}/questions/{questionId}
POST   /api/polls/{pollId}/questions
PUT    /api/polls/{pollId}/questions/{questionId}
```

---

### Voting

```
GET    /api/polls/{pollId}/available-questions
POST   /api/polls/{pollId}/vote
```

* `available-questions` returns questions and **active answers** ready for voting
* `vote` allows an authenticated user to submit their answers

---

### Dashboard & Statistics (Admin)

```
GET /api/dashboard/polls/{pollId}/statistics
GET /api/dashboard/polls/{pollId}/votes-per-day
GET /api/dashboard/polls/{pollId}/votes-per-question
```

> These endpoints are intended for **admin dashboard usage** and will be protected by role-based authorization in future iterations.

---

## Error Handling

* Centralized **Result Pattern** for consistent API responses
* Business errors mapped to `ProblemDetails`
* Clear handling for:

  * Not found
  * Validation errors
  * Business rule violations

---

## Future Improvements

* Admin role & authorization policies
* Dashboard UI with charts
* Vote percentages per question
* Prevent duplicate voting at the database level
* Caching frequently accessed statistics

---

## Author

**Abdullah Rezk**
Backend-focused personal learning project using **ASP.NET Core Web API**
