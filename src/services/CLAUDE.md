# Services Module

## Purpose
Contains business logic and orchestration between the API layer and persistence layer.

## Conventions
- Services contain all business rules — no business logic in API handlers or repositories
- Each service handles one domain (UserService, OrderService, etc.)
- Services call repositories for data — never query DB directly
- Services are testable in isolation (inject dependencies)
