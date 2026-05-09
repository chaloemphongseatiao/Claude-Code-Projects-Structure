# Prompt: Write Tests

Write tests for the following code.

## Instructions
- Cover the happy path first
- Cover all error and edge cases
- Each test must have a clear, descriptive name
- Tests must be independent — no shared mutable state
- Mock only external dependencies (DB, HTTP), not internal logic

## Test naming pattern
`test_<function>_<scenario>_<expected>`
Example: `test_createUser_withDuplicateEmail_throwsConflictError`

## Output
Provide complete, runnable test code with no placeholders.
