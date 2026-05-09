# Skill: Security Review

## Purpose
Audit code for security vulnerabilities before merging or releasing.

## Workflow

### 1. Input Validation
- [ ] All user input validated at system boundaries
- [ ] No raw user input passed to DB queries, shell commands, or eval
- [ ] File upload types and sizes validated
- [ ] Request size limits enforced

### 2. Authentication & Authorization
- [ ] All protected routes require authentication
- [ ] Authorization checked at the data level, not just route level
- [ ] Tokens have expiry and are invalidated on logout
- [ ] Passwords hashed with bcrypt/argon2 (never md5/sha1)

### 3. Injection Vulnerabilities
- [ ] SQL: parameterized queries used everywhere (no string concatenation)
- [ ] NoSQL: input sanitized before query construction
- [ ] XSS: output encoded before rendering in HTML
- [ ] Command injection: no user input in shell commands

### 4. Sensitive Data Exposure
- [ ] No secrets in source code or git history
- [ ] Sensitive config from environment variables only
- [ ] API responses don't leak internal fields (e.g. password hash, internal IDs)
- [ ] HTTPS enforced everywhere

### 5. Dependencies
- [ ] Run `npm audit` / `pip audit` — no high/critical vulnerabilities
- [ ] Dependencies are pinned to specific versions
- [ ] No abandoned or unmaintained packages

### 6. OWASP Top 10 Quick Check
- A01 Broken Access Control — authorization on every sensitive action?
- A02 Cryptographic Failures — sensitive data encrypted at rest and in transit?
- A03 Injection — all inputs parameterized?
- A05 Security Misconfiguration — debug mode off? default credentials changed?
- A07 Auth Failures — brute force protection? secure session management?
- A09 Outdated Components — dependencies up to date?

## Output Format
Report findings as:
- **CRITICAL** — must fix before merge
- **HIGH** — fix before release
- **MEDIUM** — fix in next sprint
- **INFO** — awareness only
