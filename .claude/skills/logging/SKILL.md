# Skill: Logging

## Purpose
Implement structured, consistent logging that makes debugging and observability easy without leaking sensitive data.

## Principles
- Log EVENTS, not code flow — "user.login.failed" not "entered the if block"
- Structured JSON format — machines can parse it, humans can read it
- Never log sensitive data: passwords, tokens, credit cards, PII

## Log Levels
| Level | When to use |
|-------|------------|
| `error` | Something failed and needs attention |
| `warn` | Unexpected but handled — may need action |
| `info` | Key business events (user created, order placed) |
| `debug` | Detailed flow — only in development |

## Structured Log Format
```json
{
  "level": "error",
  "message": "Payment processing failed",
  "timestamp": "2026-05-09T10:30:00Z",
  "service": "payment-service",
  "traceId": "abc-123",
  "userId": "u_789",
  "orderId": "o_456",
  "error": {
    "code": "GATEWAY_TIMEOUT",
    "message": "Stripe API timed out after 5000ms"
  }
}
```

## Always Include
- `traceId` / `requestId` — for correlating logs across services
- `userId` — for tracing user-specific issues (anonymized if needed)
- `service` — which service generated the log
- `timestamp` — ISO 8601 UTC

## Never Log
- Passwords, secrets, API keys
- Full credit card numbers (last 4 only)
- Personal data without masking (emails → `u***@domain.com`)
- Raw request bodies from auth endpoints

## Checklist
- [ ] All errors logged at `error` level with full context
- [ ] Business events logged at `info` level
- [ ] No sensitive data in any log
- [ ] Trace ID propagated across service boundaries
- [ ] Log volume reviewed — no debug logs in production
