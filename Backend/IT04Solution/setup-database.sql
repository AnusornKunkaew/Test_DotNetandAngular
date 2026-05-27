CREATE TABLE IF NOT EXISTS public.employees (
    id SERIAL PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone VARCHAR(20) NOT NULL,
    birth_day DATE NOT NULL,
    occupation VARCHAR(100) NOT NULL,
    profile_image BYTEA,
    sex VARCHAR(10) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP,
    CONSTRAINT chk_sex CHECK (sex IN ('Male', 'Female'))
);

CREATE INDEX IF NOT EXISTS idx_employees_email ON public.employees(email);
CREATE INDEX IF NOT EXISTS idx_employees_phone ON public.employees(phone);
CREATE INDEX IF NOT EXISTS idx_employees_created_at ON public.employees(created_at DESC);

GRANT ALL PRIVILEGES ON DATABASE "IT04_Employee" TO postgres;
GRANT ALL PRIVILEGES ON SCHEMA public TO postgres;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO postgres;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO postgres;

