---
name: review-decisions
description: Audit changed files against all 29 recorded architectural decisions. Use before raising a pull request to catch decision violations before code review. Produces a compliance table with OK, Review, or Violation status per decision.
---

# review-decisions

Audit the current diff against all architectural decisions recorded in `.backlog/decisions/`.

## Steps

**1. List changed files**

```bash
git diff --name-only HEAD
```

**2. Read all applicable decisions**

All 29 decisions are in `.backlog/decisions/`. Read each one that applies to your changed files
(see mapping below). You do not need to read every decision for every change.

**3. Apply the decision mapping**

| Changed file pattern | Decisions to check |
| --- | --- |
| `src/Core/Attributes/*.cs` | 8, 9, 17, 26, 27, 28, 29 |
| `src/Core/**/*.cs` (non-attribute) | 3, 7, 26, 27, 28, 29 |
| `src/*Tests*/**/*.cs` | 24, 25, 26 |
| `src/AutoMoq/**`, `src/AutoFakeItEasy/**`, `src/AutoNSubstitute/**` | 4, 26, 27, 28 |
| `.github/workflows/*.yml` | 10, 14, 15, 18, 19, 20 |
| `AGENTS.md` / `CLAUDE.md` | 22, 27 |
| `Directory.Build.props` / `.editorconfig` | 26 |
| `dependabot.yml` | 13 |
| `GitVersion.yml` | 12 |

**4. Check compliance**

Common violations to watch for:

| Decision | What to check |
| --- | --- |
| 24 | Test `DisplayName` missing, or not UPPER CASE GIVEN/WHEN/THEN |
| 25 | Missing `// Arrange`, `// Act`, `// Assert` comments in any test body |
| 26 | `[SuppressMessage]` added without a justification comment |
| 27 | XML doc comment added where self-documenting name would suffice |
| 28 | New `public` attribute-derived type is not `sealed` |
| 29 | Null check uses `ArgumentNullException` or `?? throw` instead of `NotNull()` |

**5. Output a compliance table**

```text
| Decision | Title (short) | Applies | Status    | Note |
|----------|---------------|---------|-----------|------|
| 24       | BDD naming    | Yes     | OK        |      |
| 25       | AAA structure | Yes     | Violation | Missing // Act in FooTests.cs line 42 |
| 28       | Sealed attrs  | Yes     | OK        |      |
```

Status values:

- **OK** — change complies with the decision
- **Review** — advisory; human judgement needed; document reasoning in the PR description
- **Violation** — change breaks the decision; must fix before opening a PR

## Rules

- Violations are blocking — resolve them before opening a PR
- "Review" items are advisory; note your reasoning in the PR description if you proceed
- If a decision does not apply to any changed file, omit it from the table
