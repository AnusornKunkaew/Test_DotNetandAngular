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
