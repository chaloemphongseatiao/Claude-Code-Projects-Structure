#!/bin/bash
# Deploy to production

set -e

ENV=${ENV:-production}
echo "Deploying to $ENV..."

# Run tests before deploy
npm run test

# Build
npm run build

# Deploy (replace with your deploy command)
# e.g. kubectl apply, aws ecs update, fly deploy, etc.
echo "Deploy step: add your deploy command here"

echo "Deployment complete."
