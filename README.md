# Empires of History V2

A cross-platform grand strategy game with a data-driven, modular architecture.

## Architecture Principles

- **Data-Driven**: All content comes from JSON databases
- **Framework-First**: Build reusable systems before content
- **Type-Safe**: Full TypeScript implementation
- **Scalable**: Modular design for easy expansion
- **No Hardcoding**: All historical events, facts, and content loaded from data

## Structure

- `/src/data/` - Data schemas and interfaces
- `/src/loaders/` - Content loading and validation
- `/src/core/` - Core game systems
- `/src/tests/` - Unit tests

## Development

```bash
npm install
npm run build
npm run test
```
