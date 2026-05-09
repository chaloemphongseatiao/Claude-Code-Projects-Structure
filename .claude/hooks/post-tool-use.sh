#!/bin/bash
# Runs after each tool use — add logging or notifications here

TOOL_NAME="$1"
EXIT_CODE="$2"

# Log failed tool calls
if [[ "$EXIT_CODE" != "0" ]]; then
  echo "[hook] Tool '$TOOL_NAME' failed with exit code $EXIT_CODE" >> .claude/hooks/tool-errors.log
fi

exit 0
