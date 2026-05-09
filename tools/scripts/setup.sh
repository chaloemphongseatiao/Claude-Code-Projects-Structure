#!/bin/bash
# Initial project setup

set -e

echo "Setting up project..."

# Install dependencies
npm install

# Copy env template if not exists
if [ ! -f .env ]; then
  cp .env.example .env
  echo ".env created from .env.example — fill in your values"
fi

# Run database migrations
npm run db:migrate

echo "Setup complete."
