# Frontend Module

## Purpose
UI layer — components, pages, and client-side logic.

## Structure
```
frontend/
├── components/   ← reusable UI components
├── pages/        ← page-level components (one per route)
├── hooks/        ← custom React hooks
├── styles/       ← global styles, design tokens
└── assets/       ← images, fonts, icons
```

## Conventions
- One component per file, named same as the folder
- Pages only compose components — no business logic in pages
- Business logic goes in hooks, not components
- No direct API calls in components — use hooks or services
