---
name: task-start
description: Orient around a specific backlog task before writing code. Use this skill at the start of any implementation session to load task details, identify relevant architectural decisions, and suggest a branch name. Invoke as: task-start TASK-N.
---

# task-start

Load a backlog task and orient for implementation.

## Steps

**1. Load the task**

```bash
npx backlog task show TASK-N
```

Review: title, status, assignees, labels, dependencies, and every acceptance criterion.
If the task has unresolved dependencies (status not Done), stop and resolve them first.

**2. Search for related work**

```bash
npx backlog search "<keywords from task title>"
```

Check for duplicates or related tasks to avoid overlapping work.

**3. Identify relevant decisions**

Read the task description and labels, then read related decisions from `.backlog/decisions/`.

Quick mapping by change area:

| Change area | Key decisions to read |
| --- | --- |
| New Core attribute | 3, 8, 9, 17, 26, 27, 28, 29 |
| Test changes | 24, 25, 26 |
| CI / workflow | 10, 14, 15, 18, 19, 20, 21 |
| Documentation | 22, 27 |
| NuGet dependencies | 13 |
| Versioning | 12 |

**4. Suggest a branch name**

Format: `<type>/<kebab-summary>`

| Conventional Commits type | Use when |
| --- | --- |
| `feat/` | new attribute or feature |
| `fix/` | bug fix |
| `refactor/` | restructuring without behaviour change |
| `chore/` | maintenance or tooling |
| `ci/` | CI/CD workflow change |
| `docs/` | documentation only |

Example: `feat/pick-from-list-attribute`

**5. Mark as In Progress**

```bash
npx backlog task edit TASK-N --status "In Progress"
```

## Rules

- Never start writing code before reading all acceptance criteria in full
- If no backlog task exists for the work, suggest creating one first (`npx backlog task create ...`)
- Check `npx backlog task list --status "In Progress"` — avoid having two tasks In Progress simultaneously
