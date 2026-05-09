# Skill: Database

## Purpose
Design efficient schemas, write optimized queries, and manage migrations safely.

## Workflow

### 1. Schema design
- Name tables as plural nouns: `users`, `orders`, `products`
- Every table has a surrogate primary key (`id`)
- Store timestamps: `created_at`, `updated_at` on every table
- Normalize to 3NF by default — denormalize only with profiling evidence
- Use appropriate column types (don't store numbers as VARCHAR)

### 2. Indexing strategy
Add indexes on columns that are:
- Used in WHERE clauses frequently
- Used in JOIN conditions
- Used in ORDER BY on large tables

Avoid over-indexing — every index slows down writes.

### 3. Query optimization
- Use EXPLAIN ANALYZE to inspect query plan before optimizing
- Avoid SELECT * — name only the columns you need
- Avoid N+1: batch fetches with JOIN or IN clause
- Avoid functions on indexed columns in WHERE: `WHERE YEAR(created_at) = 2024` defeats the index — use range instead

### 4. Migrations
- Every schema change goes through a migration file (never edit DB directly)
- Migrations must be reversible (include a `down` function)
- For large tables, use non-locking migrations:
  - Add column as nullable first, backfill, then add constraint
  - Never add a NOT NULL column without a default on a live table
- Test migrations on a copy of production data before running in prod

### 5. Checklist
- [ ] Schema reviewed for normalization
- [ ] Indexes added for all queried columns
- [ ] EXPLAIN ANALYZE run on slow queries
- [ ] Migration is reversible
- [ ] Migration tested on realistic data volume
- [ ] No raw SQL outside the persistence layer
