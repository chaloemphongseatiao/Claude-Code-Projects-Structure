# Web Service — Backend Memory

REST API + SignalR for LOMA-LOTTO (.NET 9).

## Tech stack
- .NET 9.0 (Nullable + ImplicitUsings on)
- Entity Framework Core 9 (SQL Server provider)
- AutoMapper 12 (DI extensions)
- Serilog (AspNetCore)
- Swashbuckle (Swagger / OpenAPI)
- (Optional) ClosedXML, SkiaSharp, MailKit, SignalR

## Expected folder layout (after scaffold)
```
LOMA-LOTTO-WEB-SERVICE/
├── Controllers/        # API endpoints — all business logic lives here (no service layer)
├── Data/               # DbContext(s)
├── Models/
│   ├── Entities/       # EF entities
│   ├── Requests/       # Request DTOs
│   ├── Responses/      # Response DTOs
│   ├── Shared/         # ApiResponse<T>, PagedApiResponse<T>, PagedResult<T>   (shipped)
│   └── Settings/       # Strongly-typed config
├── Helpers/            # ApiResponseHelper, PagedApiResponseHelper             (shipped)
├── Extensions/         # QueryableExtensions, ModelStateExtensions             (shipped)
├── Hubs/               # SignalR hubs
└── Providers/
```

Files marked **(shipped)** come from the template — leave them in place and reuse.

## Patterns to follow

### Architecture
- **Controller-only.** No service / repository layer. Business logic and EF queries live directly in controllers.
- **DI**: constructor injection only. `DbContext` is injected directly into controllers.
- **Region layout** inside every controller: `#region GET`, `#region CREATE`, `#region UPDATE`, `#region DELETE`, `#region Private`.

### Naming
| Kind | Pattern | Example |
|---|---|---|
| Controller | `{Entity}Controller.cs` | `ContactsController` |
| Request DTO | `{Action}{Entity}Request.cs` | `CreateContactRequest`, `GetContactListRequest` |
| Response DTO | `{Entity}{Type}Response.cs` | `ContactListResponse`, `ContactDetailResponse` |
| Controller method | `Get{X}`, `Create{X}`, `Update{X}`, `Delete{X}` | `GetContactList`, `CreateContact` |
| Private helper | `Build{X}DetailAsync`, `Validate{X}Async` | `BuildContactDetailAsync` |

### Return types
- List endpoint → `ActionResult<PagedApiResponse<T>>`
- Single / create / update / delete → `ActionResult<ApiResponse<T>>`
- Never return a raw object — always wrap it.

### Async
- Every method is `async Task<ActionResult<...>>`, name ends with `Async`.
- Accept `CancellationToken` and thread it through every EF call.

### EF Core
- `AsNoTracking()` for reads.
- `.Select(...)` projection straight into the response DTO (no fetch-entity-then-map detour).
- Use `.FirstOrDefaultAsync`, `.AnyAsync`, `.CountAsync` for existence / count.
- Paging via the shipped `GetPagedAsync(page, pageSize, ct)` extension.

### Validation
- Data annotations on Request DTOs: `[Required]`, `[MaxLength]`, etc.
- Each request has its own `.Normalize()` method (trim, normalize) — call it before use.
- In controllers, check `ModelState.IsValid` first → then business validation.

### Error handling (try/catch in every controller method)
```csharp
catch (OperationCanceledException)   → StatusCode(499, ...)   // client cancelled
catch (DbUpdateConcurrencyException) → Conflict (409)
catch (DbUpdateException)            → Conflict (409)
catch (Exception ex)                 → BadRequest (400) + ex.Message (optional)
```
User-facing error messages are in **Thai**.

### Program.cs / startup
- Wire Serilog at startup.
- Register `DbContext`(s) from `appsettings` connection strings.
- AutoMapper.
- CORS allow-list (explicit origins).
- Swagger.
- JSON: `JsonNamingPolicy.CamelCase` + `DefaultIgnoreCondition.Never` (nulls included).
- Middleware order: `Swagger → ForwardedHeaders → CORS → HttpsRedirection → Authentication → Authorization → MapControllers`.

## Skeleton: controller method
```csharp
[HttpGet]
public async Task<ActionResult<PagedApiResponse<ContactListResponse>>> GetContactList(
    [FromQuery] GetContactListRequest request,
    CancellationToken cancellationToken)
{
    if (!ModelState.IsValid)
        return BadRequest(ApiResponseHelper.Fail<IEnumerable<ContactListResponse>>(
            ModelState.GetErrorMessages()));

    try
    {
        request.Normalize();

        var query = _db.Contacts.AsNoTracking()
            .Where(c => /* filter from request */);

        var paged = await query
            .Select(c => new ContactListResponse {
                Id = c.Id,
                Name = c.Name,
            })
            .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

        return Ok(PagedApiResponseHelper.Ok(
            paged.Results, paged.CurrentPage, paged.PageSize, paged.RowCount, "ดึงข้อมูลสำเร็จ"));
    }
    catch (OperationCanceledException)
    {
        return StatusCode(499, ApiResponseHelper.Fail<IEnumerable<ContactListResponse>>("ยกเลิกการทำงาน"));
    }
    catch (Exception ex)
    {
        return BadRequest(ApiResponseHelper.Fail<IEnumerable<ContactListResponse>>(ex.Message));
    }
}
```
