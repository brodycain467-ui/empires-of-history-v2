# Data Templates

## Purpose
Defines documentation-only JSON template shapes for major database types.

## Ownership
Owned by Data Architecture documentation (Phase 4.95 Architecture Lock).

## Related Systems
Used by content authors, validators, and future editing tools before data enters runtime loaders.

## Future Expansion
Future phases add strict JSON Schema links, validation examples, and migration notes per template.

## File Naming Convention
Use `<domain>.template.json` in lowercase snake_case.

## Notes on placeholders
Each template contains one placeholder record (empty strings, 0, null, or []) to document required fields only.
