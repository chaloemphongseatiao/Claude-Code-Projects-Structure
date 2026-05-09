# ADR-001: Modular Claude Code Project Structure

**Date:** 2026-05-09
**Status:** Accepted

## Context
Need a standardized structure for Claude Code projects that supports reusable AI workflows, automation hooks, and clear separation of concerns across modules.

## Decision
Adopt a modular structure with:
- `CLAUDE.md` as the single source of AI context
- `.claude/skills/` for reusable workflow definitions
- `.claude/hooks/` for automation guardrails
- `docs/` for architecture and runbooks
- `src/` split by layer (api, services, persistence, utils)

## Consequences

### Positive
- AI context stays focused and minimal
- Skills are reusable across sessions
- Architecture decisions are documented and traceable

### Negative
- Requires discipline to keep CLAUDE.md up to date
- New team members must learn the skills system

### Neutral
- All modules maintain their own CLAUDE.md for local context
