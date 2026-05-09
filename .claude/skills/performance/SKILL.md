# Skill: Performance

## Purpose
Identify and eliminate performance bottlenecks through profiling before optimizing.

## Rule: Measure first, optimize second
Never optimize without data. Profiling tells you WHERE the bottleneck is — guessing wastes time.

## Workflow

### 1. Establish baseline
- Define the metric: latency (p50/p95/p99), throughput (req/s), memory (MB)
- Record the current value before any changes
- Set a target: "reduce p95 from 800ms → under 200ms"

### 2. Profile
- **Backend**: use language profiler (py-spy, pprof, Node --prof)
- **Frontend**: Chrome DevTools → Performance tab, Lighthouse
- **Database**: EXPLAIN ANALYZE on slow queries
- Look for: hot loops, N+1 queries, large allocations, synchronous I/O

### 3. Common bottlenecks & fixes
| Bottleneck | Fix |
|-----------|-----|
| N+1 DB queries | Eager load / batch fetch |
| Missing DB index | Add index on queried columns |
| Large payload | Paginate, compress (gzip/brotli), lazy load |
| Repeated computation | Cache result (in-memory / Redis) |
| Blocking I/O | Make async, use connection pooling |
| Re-renders (React) | Memoize with useMemo/useCallback, virtualize lists |

### 4. Cache strategy
- Cache at the right level: DB query → service layer → HTTP (CDN/browser)
- Define TTL based on how stale the data can safely be
- Always plan cache invalidation before caching

### 5. Checklist
- [ ] Baseline metric recorded
- [ ] Profiler output reviewed (not guesswork)
- [ ] Root cause identified
- [ ] Fix applied to root cause
- [ ] Improvement measured against baseline
- [ ] No correctness regressions introduced
