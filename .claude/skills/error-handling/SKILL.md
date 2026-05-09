# Skill: Error Handling

## Purpose
Implement consistent, informative error handling across the entire codebase without exposing internals to users.

## Workflow

### 1. Classify errors
| Type | Example | Action |
|------|---------|--------|
| Validation | Missing required field | Return 400, tell user what's wrong |
| Not found | User ID doesn't exist | Return 404 |
| Auth | Invalid token | Return 401/403 |
| Business logic | Insufficient balance | Return 409, meaningful message |
| Unexpected | DB connection lost | Return 500, log full details internally |

### 2. Never expose internals
- No stack traces to end users
- No raw DB errors (e.g. SQL constraint messages)
- No internal file paths or variable names

### 3. Error structure
Define one error type for the whole codebase:
```typescript
class AppError extends Error {
  constructor(
    public code: string,       // machine-readable: "USER_NOT_FOUND"
    public message: string,    // human-readable: "User not found"
    public statusCode: number, // HTTP status
    public field?: string      // optional: which field caused it
  ) { super(message) }
}
```

### 4. Centralized error handler
- Catch all unhandled errors in one place (middleware/interceptor)
- Log full error details internally (stack, context, user ID)
- Return only safe, structured response to caller

### 5. Async errors
- Always `await` promises inside try/catch
- Never let unhandled promise rejections propagate silently

### 6. Checklist
- [ ] All errors use the shared AppError class
- [ ] No raw errors reach the API response
- [ ] Unexpected errors are logged with full context
- [ ] Validation errors name the offending field
- [ ] Tests cover error paths, not just happy paths
