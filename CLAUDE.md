# Claude Code Project

## Project Overview
A modular repository structure designed for building Claude Code projects with structured AI context, reusable skills, and automated development workflows.

## Key Components
- **CLAUDE.md**: Project memory and instructions for Claude.
- **.claude/skills/**: Reusable AI workflows for coding tasks.
- **.claude/hooks/**: Guardrails and automation checks.
- **docs/**: Architecture decisions and documentation.
- **src/**: Core application modules.

## Best Practices
- Keep CLAUDE.md focused and structured
- Use skills for reusable AI workflows
- Use hooks for automation and checks
- Document architecture decisions
- Maintain modular repository design

## Repository Structure
```
claude_code_project/
├── CLAUDE.md
├── README.md
├── docs/
│   ├── architecture.md
│   ├── decisions/
│   └── runbooks/
├── .claude/
│   ├── settings.json
│   ├── hooks/
│   └── skills/
│       ├── code-review/
│       ├── refactor/
│       └── release/
├── tools/
│   ├── scripts/
│   └── prompts/
└── src/
    ├── api/
    └── persistence/
```
