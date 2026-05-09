# Skill: Frontend Design

## Purpose
Design and implement clean, consistent, and accessible frontend UI components.

## Usage
Invoke this skill when building or reviewing UI components and layouts.

## Workflow

### 1. Understand the design intent
- Review Figma/mockup if available
- Identify the component's purpose and user interaction
- Note responsive breakpoints required

### 2. Component structure
- Break UI into smallest reusable components
- Define props/interface clearly before coding
- Keep components focused — one responsibility each

### 3. Styling conventions
- Use design tokens (colors, spacing, typography) — no raw hex or magic numbers
- Mobile-first responsive design
- Consistent spacing scale (e.g. 4px base unit)

### 4. Accessibility (a11y)
- [ ] Semantic HTML elements (`<button>`, `<nav>`, `<main>`, etc.)
- [ ] ARIA labels where needed
- [ ] Keyboard navigable (tab order, focus styles)
- [ ] Color contrast ratio ≥ 4.5:1 (WCAG AA)
- [ ] Alt text on all images

### 5. Quality checklist
- [ ] Matches design spec / Figma mockup
- [ ] Responsive across mobile, tablet, desktop
- [ ] No hardcoded colors or spacing
- [ ] Loading, empty, and error states handled
- [ ] No layout shift on data load
- [ ] Cross-browser tested (Chrome, Firefox, Safari)

## File Organization
```
src/
└── components/
    └── ComponentName/
        ├── index.tsx        ← main component
        ├── ComponentName.module.css  ← scoped styles
        └── ComponentName.test.tsx   ← unit tests
```
