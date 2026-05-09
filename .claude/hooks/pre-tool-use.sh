#!/bin/bash
# Runs before each tool use — add guardrails here

TOOL_NAME="$1"

# Block destructive git commands without confirmation
if [[ "$TOOL_NAME" == "Bash" ]]; then
  INPUT="$2"
  if echo "$INPUT" | grep -qE "git (reset --hard|push --force|clean -f|branch -D)"; then
    echo "BLOCKED: Destructive git command requires explicit user confirmation." >&2
    exit 1
  fi
fi

exit 0
