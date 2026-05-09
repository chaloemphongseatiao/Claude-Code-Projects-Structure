# Skill: Debug

## Purpose
Systematically identify and resolve bugs using a structured root-cause analysis process.

## Usage
Invoke this skill when diagnosing unexpected behavior or failures.

## Workflow

### 1. Reproduce
- Confirm the bug is reproducible
- Identify the minimal input/state that triggers it
- Note the environment (OS, runtime version, config)

### 2. Locate
- Read the error message and stack trace carefully
- Trace the execution path from entry point to failure
- Narrow down to the smallest failing code unit

### 3. Hypothesize
- Form 1-3 hypotheses about the root cause
- Rank by likelihood
- Check assumptions with targeted log/print statements

### 4. Fix
- Address the root cause, not just the symptom
- Verify the fix does not break other behavior
- Add a regression test to prevent recurrence

### 5. Document
- Describe what caused the bug
- Note in commit message or PR description

## Debugging Checklist
- [ ] Error reproduced locally
- [ ] Stack trace fully read
- [ ] Root cause identified (not just symptom)
- [ ] Fix tested against original reproduction case
- [ ] Regression test added
