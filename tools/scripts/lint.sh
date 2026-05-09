#!/bin/bash
# Run all linting and type checks

set -e

echo "Running lint..."
npm run lint

echo "Running type check..."
npm run type-check

echo "All checks passed."
