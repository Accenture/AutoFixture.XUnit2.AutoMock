---
name: task-finish
description: Finalize a backlog task after implementation. Verifies acceptance criteria, checks changed files against architectural decisions, validates test quality, suggests a Conventional Commits message, and updates the task to Done. Use before committing. Invoke as: task-finish TASK-N.
---

# task-finish

Verify implementation completeness and prepare a clean commit.

## Steps

**1. Review acceptance criteria**

```bash
npx backlog task show TASK-N
```

Go through every AC item. If any item is not satisfied, stop and complete it before continuing.

**2. Map changed files to decisions**

```bash
git diff --name-only HEAD
```

| Changed file pattern | Decisions to verify |
| --- | --- |
| `src/Core/Attributes/*.cs` | 8, 9, 17, 26, 27, 28, 29 |
| `src/Core/*.cs` | 3, 26, 27, 28, 29 |
| `src/*Tests*/*.cs` | 24, 25, 26 |
| `src/AutoMoq/**`, `src/AutoFakeItEasy/**`, `src/AutoNSubstitute/**` | 4, 26, 27, 28 |
| `.github/workflows/*.yml` | 10, 14, 15, 18, 19, 20 |
| `AGENTS.md` / `CLAUDE.md` | 22, 27 |

**3. Verify test quality** (if test files were added or changed)

Check each new or modified test method for:

- `DisplayName` is set on every `[Fact]` and `[Theory]`
- DisplayName is UPPER CASE GIVEN/WHEN/THEN format:
  - Theory: `GIVEN <precondition> WHEN <action> THEN <outcome>`
  - Fact: `WHEN <action> THEN <outcome>`
- Method name mirrors DisplayName in PascalCase_WithUnderscores
- No `Test` suffix or prefix in method names
- Data-source attributes (`[AutoMockData]`, `[InlineAutoMockData]`) appear **above** `[Theory]`
- Tests have `// Arrange`, `// Act`, `// Assert` section comments
- No `new Fixture()` usage — `IFixture` injected via test method parameters

**4. Run validate**

Follow `.ai/skills/validate/SKILL.md` to confirm build and tests pass.

**5. Suggest a commit message**

Format: `type(scope): description` (imperative, lowercase, no trailing period)

- `type`: feat, fix, refactor, chore, ci, docs, test, perf
- `scope`: optional — module or area (e.g. `core`, `automoq`, `ci`)

Examples:

```text
feat(core): add PickFromList parameter attribute
test(automoq): consolidate duplicate Facts into Theories
docs: add Available Skills section to AGENTS.md
```

**6. Update task status**

Tick all AC checkboxes in the task file, then:

```bash
npx backlog task edit TASK-N --status "Done"
```

**7. Proceed to PR**

Follow `.ai/skills/create-branch-pr/SKILL.md` to open the pull request.

## Rules

- Do not commit until every AC is confirmed
- Do not mark Done with any AC checkbox unticked
- One logical change per commit; follow Conventional Commits
