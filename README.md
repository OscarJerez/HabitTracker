# HabitTracker API

A clean architecture .NET 8 Web API for tracking habits, built with **CQRS**, **MediatR**, **Entity Framework Core**, and **JWT Authentication**.  
This project demonstrates professional backend development practices such as clean architecture, separation of concerns, and testability.

---

## Features

- **User Authentication**
  - Register users (with secure password hashing using BCrypt).
  - Login with JWT authentication.
  - Role-based authorization.

- **Habit Management (CRUD)**
  - Create new habits.
  - Retrieve habits by user.
  - Update existing habits.
  - Soft-delete habits (marked as deleted, not removed from database).

- **Clean Architecture**
  - Separation of concerns with projects:
    - `HabitTracker.API` → Endpoints and middleware.
    - `HabitTracker.Application` → CQRS handlers, DTOs, validation, MediatR pipeline behaviors.
    - `HabitTracker.Infrastructure` → EF Core DbContext, repositories, identity, JWT generation.
    - `HabitTracker.Domain` → Entities (`Habit`, `HabitUser`).

---

## Technologies Used

- **.NET 8**
- **Entity Framework Core**
- **MediatR (CQRS)**
- **FluentValidation**
- **JWT Authentication**
- **BCrypt.Net-Next** (password hashing)
- **Swagger / OpenAPI** (for testing endpoints)
- **SQL Server Express** (database)

---

## Database

- **SQL Server Express instance**  
- Contains two main tables:
  - `Users` (HabitUser)
  - `Habits` (linked to users by `UserId`)  

Soft delete is implemented using the `IsDeleted` flag in `Habits`, with a global EF Core query filter.

---

## Endpoints

### Authentication
- `POST /api/auth/register` → Register new user  
- `POST /api/auth/login` → Login and receive JWT  

### Habits
- `POST /api/habit` → Create a new habit  
- `GET /api/habit/{userId}` → Get habits for a specific user  
- `PUT /api/habit/{habitId}` → Update habit  
- `DELETE /api/habit/{habitId}` → Soft delete habit  

---

## How to Run

1. Clone repository:
   ```bash
   git clone https://github.com/OscarJerez/HabitTracker-Final.git
   cd HabitTracker-Final
By Oscar Jerez
