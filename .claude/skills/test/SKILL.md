# Skill: Test

## Purpose
Generate and review unit, integration, and end-to-end tests for code changes.

## Usage
Invoke this skill when writing new tests or reviewing test coverage.

## Workflow

### 1. Analyze the code
- Identify all public functions and their edge cases
- Note dependencies that need mocking
- Check existing test patterns in the codebase

### 2. Write tests
- Unit tests: test each function in isolation
- Integration tests: test interactions between modules
- Edge cases: empty input, nulls, boundary values, error paths

### 3. Test quality checklist
- [ ] Covers the happy path
- [ ] Covers error/failure paths
- [ ] Covers edge cases (empty, null, boundary)
- [ ] Tests are independent (no shared state)
- [ ] Test names clearly describe the scenario
- [ ] No implementation details leaked into tests
- [ ] Mocks are minimal and necessary

## Naming Convention
```
test_<function>_<scenario>_<expected_result>
// e.g. test_createUser_withDuplicateEmail_throwsConflictError
```
