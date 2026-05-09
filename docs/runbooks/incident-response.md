# Runbook: Incident Response

## Severity Levels
| Level | Definition | Response Time |
|-------|-----------|---------------|
| P1 | Production down / data loss | Immediate |
| P2 | Major feature broken | < 30 min |
| P3 | Minor degradation | < 4 hours |

## Response Steps

### 1. Detect & Acknowledge
- Confirm the incident is real (not a false alert)
- Post in incident channel: "ACK: [brief description] — investigating"

### 2. Assess Impact
- How many users affected?
- Is data at risk?
- Assign severity level (P1/P2/P3)

### 3. Investigate
- Check recent deployments (`git log --oneline -10`)
- Check error logs and metrics
- Form a hypothesis before making changes

### 4. Mitigate
- Rollback deployment if recent change caused it
- Apply hotfix if root cause is known
- Scale resources if it's a traffic issue

### 5. Resolve & Document
- Confirm system is healthy
- Write incident summary: what happened, timeline, root cause, fix
- Schedule post-mortem within 48 hours

## Post-mortem Template
- **What happened:** ...
- **Timeline:** ...
- **Root cause:** ...
- **Fix applied:** ...
- **Prevention:** ...
