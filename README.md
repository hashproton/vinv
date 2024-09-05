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

Here’s how you can integrate those examples into the "Testing" section of the `README.md`:

## Testing

### Test Naming Convention

All tests in this project follow the naming pattern:

```plaintext
ShouldExpectedBehavior_WhenCondition
```

This ensures that each test is descriptive and clear, specifying:

- **Expected Behavior**: What the method or function should do.
- **Condition**: Under what condition the expected behavior occurs.

### Examples

- `ShouldNotHaveValidationErrors_WhenCommandIsValid()`: Verifies that there are no validation errors when a command is valid.
- `ShouldHaveErrorForName_WhenNameIsEmpty()`: Checks that an error occurs when the `Name` field is empty.
- `ShouldHaveErrorForStatus_WhenStatusIsOutOfEnumRange()`: Ensures an error is returned when the `Status` is outside the allowed enumeration range.
- `ShouldReturnConflict_WhenTenantAlreadyExists()`: Verifies that a conflict (HTTP 409) response is returned when attempting to create a tenant that already exists.

This test naming convention ensures that tests are readable, consistent, and self-explanatory.
