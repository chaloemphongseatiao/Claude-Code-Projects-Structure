# Skill: API Design

## Purpose
Design consistent, versioned, and well-documented REST or GraphQL APIs.

## Workflow

### 1. Define the contract first
- Write the API spec (OpenAPI/Swagger) before coding
- Agree on request/response shape with consumers
- Define error response format upfront

### 2. REST conventions
- Use nouns for resources, not verbs: `/users` not `/getUsers`
- HTTP methods map to actions: GET (read), POST (create), PUT/PATCH (update), DELETE (remove)
- Plural resource names: `/orders`, `/products`
- Nest only one level deep: `/users/{id}/orders` — avoid `/users/{id}/orders/{id}/items`

### 3. Versioning
- Version in URL path: `/api/v1/users`
- Never break existing versions — add a new version instead

### 4. Response structure
```json
// Success
{ "data": { ... }, "meta": { "page": 1, "total": 42 } }

// Error
{ "error": { "code": "NOT_FOUND", "message": "User not found", "field": "id" } }
```

### 5. Status codes
| Scenario | Code |
|----------|------|
| Success (read) | 200 |
| Created | 201 |
| No content | 204 |
| Bad request / validation | 400 |
| Unauthorized | 401 |
| Forbidden | 403 |
| Not found | 404 |
| Conflict | 409 |
| Server error | 500 |

### 6. Checklist
- [ ] Spec written before implementation
- [ ] Consistent naming across all endpoints
- [ ] All error cases return structured error response
- [ ] Pagination on all list endpoints
- [ ] Auth requirements documented
- [ ] Breaking changes → new version
