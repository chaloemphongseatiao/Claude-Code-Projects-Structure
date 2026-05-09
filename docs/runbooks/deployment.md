# Runbook: Deployment

## Purpose
Step-by-step guide for deploying the application to production safely.

## Prerequisites
- [ ] All tests passing on main branch
- [ ] PR reviewed and approved
- [ ] No active incidents in production
- [ ] Notify team in Slack before starting

## Steps

### 1. Pre-deployment
```bash
git checkout main && git pull
npm run test
npm run build
```

### 2. Deploy
```bash
# Set environment
export ENV=production

# Run deployment script
./tools/scripts/deploy.sh
```

### 3. Verify
- Check health endpoint: `GET /api/health`
- Verify key user flows work
- Monitor error rate for 10 minutes post-deploy

### 4. Rollback (if needed)
```bash
./tools/scripts/rollback.sh <previous-version>
```

## Contacts
| Role | Name | Contact |
|------|------|---------|
| On-call | - | - |
| Tech Lead | - | - |
