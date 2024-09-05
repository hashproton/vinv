# vinv

## EF Core

### Generating Migrations

To generate new migrations, run the following command in the root of the project:

```bash
dotnet ef migrations add <migration_name> -p .\src\Infra\ -s .\src\Presentation\Api\
```

- `-p .\src\Infra\`: Specifies the project where the migrations will be created.
- `-s .\src\Presentation\Api\`: Specifies the startup project.

### Applying Migrations

After generating a migration, apply it to the database using:

```bash
dotnet ef database update -p .\src\Infra\ -s .\src\Presentation\Api\
```

This will update your database schema with the latest changes.

## Services

### Running Services

To run the necessary services, use the `docker-compose.yml` file located in the project root.

### Services Overview

- **pgAdmin**:
  - Access: `http://localhost:8888`
  - Port: `8888`
  - Credentials:
    - **Email**: `dev@dev.com`
    - **Password**: `dev`
- **PostgreSQL**:
  - Port: `5432`
  - Credentials:
    - **User**: `dev`
    - **Password**: `dev`

### Steps to Start Services

1. Ensure Docker is installed and running on your system.
2. Run the following command to start services in detached mode:

```bash
docker compose up -d
```
