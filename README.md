# IT04 Employee Management System

A modern full-stack application built with Clean Architecture principles using .NET 10 Backend, Angular 20 Frontend, and PostgreSQL Database.

## 📋 Project Overview

This application implements an IT 04-1 Employee Management Form with the following features:
- Complete employee information management (CRUD operations)
- Form validation (Email, Phone, Birth Date formats)
- Profile image storage (Base64 encoding)
- RESTful API backend
- Responsive Angular frontend
- PostgreSQL database

## 🏗️ Architecture

### Backend (.NET 10) - Clean Architecture
The backend is organized into layers following Clean Architecture principles:

```
Backend/IT04Solution/
├── IT04Solution.Domain/          # Domain Layer - Business Entities
│   └── Entities/
│       └── Employee.cs
├── IT04Solution.Application/     # Application Layer - Business Logic
│   ├── DTOs/
│   │   └── EmployeeDtos.cs
│   ├── Services/
│   │   ├── EmployeeService.cs
│   │   └── ValidationService.cs
│   └── Interfaces/
│       └── IEmployeeService.cs
├── IT04Solution.Infrastructure/  # Infrastructure Layer - Data Access
│   ├── Data/
│   │   └── EmployeeDbContext.cs
│   └── Repositories/
│       └── EmployeeRepository.cs
├── IT04Solution.Api/             # Presentation Layer - API Controllers
│   ├── Controllers/
│   │   └── EmployeeController.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
└── IT04Solution.Tests/           # Unit Tests
```

**Layer Responsibilities:**
- **Domain**: Core business entities with no dependencies
- **Application**: Business logic, validation, orchestration
- **Infrastructure**: Data persistence, external services
- **Presentation**: HTTP endpoints, request/response handling

### Frontend (Angular) - Separation of Concerns
The frontend follows SOC with feature-based modules:

```
Frontend/src/app/
├── shared/                       # Shared components & services
│   ├── components/
│   └── services/
├── modules/
│   └── it04-1/                   # Feature Module
│       ├── components/
│       │   ├── it04-form.component.ts
│       │   ├── it04-form.component.html
│       │   └── it04-form.component.scss
│       ├── services/
│       │   ├── employee.service.ts
│       │   └── validation.service.ts
│       └── models/
│           └── employee.model.ts
└── app.component.ts              # Root component
```

### Database (PostgreSQL)
- Structured schema with constraints
- Indexed for performance (email, phone, created_at)
- BYTEA column for profile image storage

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK
- Node.js (v20+) & npm
- PostgreSQL (v12+)
- Visual Studio Code or Visual Studio 2022

### Backend Setup

1. **Install .NET 10 SDK**
   ```bash
   # Download from https://dotnet.microsoft.com/en-us/download/dotnet/10.0
   ```

2. **Configure Database Connection**
   Edit `Backend/IT04Solution/IT04Solution.Api/appsettings.Development.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=IT04_Employee_Dev;Username=postgres;Password=YOUR_PASSWORD"
   }
   ```

3. **Create Database**
   ```bash
   # Run PostgreSQL and execute setup-database.sql
   psql -U postgres -f Backend/IT04Solution/setup-database.sql
   ```

4. **Run Migrations**
   ```bash
   cd Backend/IT04Solution/IT04Solution.Api
   dotnet ef database update
   ```

5. **Start API Server**
   ```bash
   cd Backend/IT04Solution/IT04Solution.Api
   dotnet run
   ```
   API will be available at `https://localhost:5000` (HTTPS) or `http://localhost:5001` (HTTP)

### Frontend Setup

1. **Install Dependencies**
   ```bash
   cd Frontend
   npm install
   ```

2. **Start Development Server**
   ```bash
   npm start
   # or
   ng serve
   ```
   Frontend will be available at `http://localhost:4200`

3. **Build for Production**
   ```bash
   npm run build
   # or
   ng build --configuration production
   ```

## 📝 API Documentation

### Endpoints

#### Create Employee
- **POST** `/api/employee/create`
- **Request Body:**
  ```json
  {
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "phone": "0812345678",
    "birthDay": "1990-01-15",
    "occupation": "Engineer",
    "profileImageBase64": "data:image/jpeg;base64,...",
    "sex": "Male"
  }
  ```
- **Response:**
  ```json
  {
    "success": true,
    "message": "save data success",
    "data": {
      "id": 1,
      "firstName": "John",
      ...
    }
  }
  ```

#### Get Employee
- **GET** `/api/employee/get/{id}`

#### Get All Employees
- **GET** `/api/employee/getAll`

#### Update Employee
- **PUT** `/api/employee/update/{id}`

#### Delete Employee
- **DELETE** `/api/employee/delete/{id}`

## ✅ Validation Rules

### Client-Side (Angular)
- Email: Valid email format (RFC 5322)
- Phone: 9-10 digits (Thai phone format)
- Birth Day: Valid date, not in future
- Sex: Must be 'Male' or 'Female'
- All fields: Required

### Server-Side (.NET)
- Duplicate email check (unique constraint)
- Phone format validation (9-10 digits)
- Birth date validation
- All required field validation

## 🗄️ Database Schema

### Employees Table
| Column | Type | Constraints |
|--------|------|-------------|
| id | SERIAL | PRIMARY KEY |
| first_name | VARCHAR(100) | NOT NULL |
| last_name | VARCHAR(100) | NOT NULL |
| email | VARCHAR(255) | NOT NULL, UNIQUE |
| phone | VARCHAR(20) | NOT NULL |
| birth_day | DATE | NOT NULL |
| occupation | VARCHAR(100) | NOT NULL |
| profile_image | BYTEA | NULL |
| sex | VARCHAR(10) | NOT NULL, CHECK IN ('Male', 'Female') |
| created_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP |
| updated_at | TIMESTAMP | NULL |

## 🧪 Testing

### Backend Unit Tests
```bash
cd Backend/IT04Solution/IT04Solution.Tests
dotnet test
```

### Frontend Tests
```bash
cd Frontend
npm run test
# or
ng test
```

## 📦 Dependencies

### Backend (.NET 10)
- **Npgsql.EntityFrameworkCore.PostgreSQL**: PostgreSQL database provider
- **Microsoft.EntityFrameworkCore**: ORM for data access
- **Microsoft.AspNetCore.OpenApi**: OpenAPI support
- **Swashbuckle.AspNetCore**: Swagger/OpenAPI UI

### Frontend (Angular 20)
- **@angular/forms**: Reactive Forms
- **@angular/common**: Common directives
- **@angular/platform-browser**: Browser APIs
- **rxjs**: Reactive programming

## 🔒 Security Considerations

- Email constraint ensures no duplicate records
- Input validation on both client and server
- HTTPS enabled in production
- CORS configured for Angular frontend
- SQL injection prevention via EF Core parameterized queries

## 📱 UI Features

- ✅ Responsive design (desktop & mobile)
- ✅ Real-time form validation
- ✅ File upload for profile image (Base64 encoding)
- ✅ Clear error messages
- ✅ Success notification with generated ID
- ✅ Clear button to reset form
- ✅ Professional styling with SCSS

## 🚦 Project Status

- ✅ Backend API complete
- ✅ Frontend UI complete
- ✅ Database schema ready
- ⏳ Integration testing in progress
- ⏳ Documentation complete

## 📝 License

This project is provided as-is for educational purposes.

## 💬 Support

For issues or questions, please refer to the documentation or create an issue in the repository.

---

**Created by:** Senior Developer Team  
**Technology Stack:** .NET 10, Angular 20, PostgreSQL 12+  
**Architecture:** Clean Architecture + Separation of Concerns  
**Last Updated:** May 2026
#   T e s t _ D o t N e t a n d A n g u l a r  
 "# Test_DotNetandAngular" 
#   T e s t _ D o t N e t a n d A n g u l a r  
 #   T e s t _ D o t N e t a n d A n g u l a r  
 #   T e s t _ D o t N e t a n d A n g u l a r  
 