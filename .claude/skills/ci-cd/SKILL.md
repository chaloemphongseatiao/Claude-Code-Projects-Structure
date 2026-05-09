# Skill: CI/CD

## Purpose
Design and maintain automated pipelines that catch issues early and deploy reliably.

## Pipeline Stages

### 1. CI (on every PR)
```
lint → type-check → unit-tests → integration-tests → security-scan → build
```
All stages must pass before merge is allowed.

### 2. CD (on merge to main)
```
build → deploy-staging → smoke-test → [manual approval] → deploy-production → verify
```

## Pipeline Checklist

### CI
- [ ] Lint and type checking runs on every PR
- [ ] Unit tests run in parallel for speed
- [ ] Test coverage threshold enforced (fail if drops below baseline)
- [ ] Dependency vulnerability scan (`npm audit`)
- [ ] Docker image built and scanned for vulnerabilities
- [ ] Build artifact cached to speed up subsequent runs

### CD
- [ ] Staging deploy is fully automated
- [ ] Production deploy requires manual approval
- [ ] Health check runs after every deploy
- [ ] Automatic rollback on failed health check
- [ ] Secrets injected from vault/secret manager — never in pipeline config
- [ ] Deploy notifications sent to team channel

## Environment Strategy
| Environment | Trigger | Purpose |
|-------------|---------|---------|
| development | Local | Feature development |
| staging | Merge to main | Integration testing |
| production | Manual approval | Live users |

## Key Principles
- Pipeline config lives in code (`.github/workflows/`, `Jenkinsfile`, etc.)
- Builds are reproducible — same input always produces same output
- Secrets never appear in logs
- Flaky tests are tracked and fixed — not retried indefinitely
- Pipeline failures notify the team immediately
