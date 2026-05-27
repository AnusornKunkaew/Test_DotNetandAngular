# Setup Guide

## Environment Requirements

### Windows Setup

#### 1. Install .NET 10
```powershell
# Download installer from https://dotnet.microsoft.com/download/dotnet/10.0
# Or use winget
winget install Microsoft.DotNet.SDK.10
```

#### 2. Install PostgreSQL
```powershell
# Using chocolatey
choco install postgresql

# Or download from https://www.postgresql.org/download/windows/
# Default: Port 5432, Username: postgres
```

#### 3. Install Node.js & npm
```powershell
# Using chocolatey
choco install nodejs

# Or download from https://nodejs.org/
```

#### 4. Install Visual Studio Code Extensions (Recommended)
- C# Dev Kit
- Angular Language Service
- REST Client
- PostgreSQL

---

## Quick Start

### 1. Backend Setup

```powershell
# Navigate to Backend
cd Backend/IT04Solution

# Restore NuGet packages
dotnet restore

# Update database connection in appsettings.Development.json
# Set correct PostgreSQL credentials

# Run migrations
cd IT04Solution.Api
dotnet ef database update

# Start API server
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### 2. Frontend Setup

```powershell
# Navigate to Frontend
cd Frontend

# Install npm packages
npm install

# Start development server
npm start
```

**Expected Output:**
```
✔ Compiled successfully.

√ Compiled successfully.

→ Local:   http://localhost:4200/
  ➜  press h + enter to show help
```

### 3. PostgreSQL Setup

```powershell
# Connect to PostgreSQL
psql -U postgres

# Run setup script
\i 'Backend/IT04Solution/setup-database.sql'

# Verify table creation
\dt IT04_Employee

# Exit
\q
```

---

## Common Issues & Solutions

### Issue: "dotnet" is not recognized
**Solution:** 
- Restart terminal/PowerShell
- Verify .NET SDK installation: `dotnet --version`
- Add to PATH if needed

### Issue: PostgreSQL connection refused
**Solution:**
- Check PostgreSQL service is running: `services.msc`
- Verify connection string in `appsettings.json`
- Default: `Host=localhost;Port=5432;Database=IT04_Employee_Dev;Username=postgres;Password=postgres`

### Issue: CORS error in Angular
**Solution:**
- Ensure API is running at `https://localhost:5000`
- Verify CORS configuration in `Program.cs`
- Check EmployeeService API URL matches

### Issue: Angular build fails
**Solution:**
```powershell
# Clear npm cache
npm cache clean --force

# Reinstall dependencies
rm package-lock.json
npm install

# Try build again
npm run build
```

### Issue: Port already in use
**Solution:**
```powershell
# .NET API (port 5000)
# Find process using port
netstat -ano | findstr :5000
# Kill process
taskkill /PID <PID> /F

# Angular (port 4200)
# Use different port
ng serve --port 4300
```

---

## Development Workflow

### Visual Studio Code Setup

1. **Open Workspace**
   ```powershell
   code d:\Test_DotNet10_Angular
   ```

2. **Backend Debugging**
   - Set breakpoints in C# code
   - Press F5 or use Debug menu
   - Use VS Code REST Client for API testing

3. **Frontend Debugging**
   - Open DevTools (F12)
   - Use Angular DevTools extension
   - Set breakpoints in TypeScript

### REST Client Testing

Create `.http` file in Backend folder:
```http
### Create Employee
POST http://localhost:5000/api/employee/create
Content-Type: application/json

{
  "firstName": "Test",
  "lastName": "User",
  "email": "test@example.com",
  "phone": "0812345678",
  "birthDay": "1990-01-15",
  "occupation": "Engineer",
  "sex": "Male"
}

### Get All Employees
GET http://localhost:5000/api/employee/getAll

### Get Employee by ID
GET http://localhost:5000/api/employee/get/1

### Update Employee
PUT http://localhost:5000/api/employee/update/1
Content-Type: application/json

{
  "firstName": "Updated",
  "lastName": "User",
  "email": "updated@example.com",
  "phone": "0898765432",
  "birthDay": "1990-01-15",
  "occupation": "Manager",
  "sex": "Female"
}

### Delete Employee
DELETE http://localhost:5000/api/employee/delete/1
```

---

## Production Deployment

### Backend Deployment (.NET)
```powershell
# Build for production
dotnet build -c Release

# Publish
dotnet publish -c Release -o ./publish

# Run from publish folder
cd publish
dotnet IT04Solution.Api.dll
```

### Frontend Deployment (Angular)
```bash
npm run build

# Serve with HTTP server or deploy dist/ folder to CDN/web server
# Angular CLI: ng serve --prod
# Or copy dist/it04-app to web server
```

---

## Database Backup & Restore

### Backup Database
```powershell
$env:PGPASSWORD="postgres"
pg_dump -U postgres -d IT04_Employee -f "backup_$(Get-Date -Format 'yyyy-MM-dd_HH-mm-ss').sql"
$env:PGPASSWORD=""
```

### Restore Database
```powershell
$env:PGPASSWORD="postgres"
psql -U postgres -d IT04_Employee -f backup_file.sql
$env:PGPASSWORD=""
```

---

## Additional Resources

- [.NET 10 Documentation](https://learn.microsoft.com/dotnet/)
- [Angular 20 Documentation](https://angular.io/docs)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)

---

**Last Updated:** May 2026
