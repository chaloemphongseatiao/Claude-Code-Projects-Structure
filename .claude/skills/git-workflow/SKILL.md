# Skill: Git Workflow

## Purpose
Maintain a clean, readable git history with consistent commit messages and branching strategy.

## Branching Strategy
```
main          ← production-ready, protected
develop       ← integration branch
feature/xyz   ← new features (branch from develop)
fix/xyz       ← bug fixes (branch from main for hotfix, develop otherwise)
release/x.y.z ← release preparation
```

## Commit Message Format (Conventional Commits)
```
<type>(<scope>): <short summary>

[optional body — explain WHY, not WHAT]

[optional footer: BREAKING CHANGE, closes #issue]
```

### Types
| Type | When to use |
|------|------------|
| `feat` | New feature |
| `fix` | Bug fix |
| `refactor` | Code change, no feature/fix |
| `test` | Adding/updating tests |
| `docs` | Documentation only |
| `chore` | Build, deps, config |
| `perf` | Performance improvement |

### Examples
```
feat(auth): add JWT refresh token rotation
fix(api): return 404 when user not found instead of 500
refactor(db): extract query builder into repository class
```

## Pull Request Checklist
- [ ] Branch is up to date with base branch
- [ ] Commit messages follow convention
- [ ] No unrelated changes in the PR
- [ ] Tests added/updated for changes
- [ ] PR description explains the WHY
- [ ] No debug/console.log left in code

## Rules
- Never commit directly to `main` or `develop`
- Squash fixup commits before merging
- Delete branches after merging
- One logical change per PR
