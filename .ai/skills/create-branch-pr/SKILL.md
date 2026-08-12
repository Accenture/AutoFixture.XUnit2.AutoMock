---
name: create-branch-pr
description: Create a feature branch and open a pull request following repository conventions. Validates branch naming, prefills the PR description from the project template, and walks through the full PR checklist. Use when ready to publish changes. Invoke as: create-branch-pr [TASK-N].
---

# create-branch-pr

Create a properly named branch and open a PR with the standard description template.

See `assets/pr-template.md` for the full prefilled PR description.

## Steps

**1. Confirm the branch name**

Format: `<type>/<kebab-description>`

The type must match the Conventional Commits type of the primary commit:

| Type | Use when |
| --- | --- |
| `feat/` | new attribute or feature |
| `fix/` | bug fix |
| `refactor/` | code restructuring without behaviour change |
| `chore/` | maintenance, tooling, dependency updates |
| `ci/` | CI/CD workflow changes |
| `docs/` | documentation only |

Examples: `feat/pick-from-list-attribute`, `fix/enumerable-allocation`, `docs/add-skills`

**2. Create and push the branch**

```bash
git checkout -b <branch-name>
git push -u origin <branch-name>
```

**3. Walk through the PR checklist**

Confirm each item before opening the PR:

- [ ] Commit messages follow Conventional Commits (`type(scope): description`)
- [ ] `dotnet build src/Objectivity.AutoFixture.XUnit2.AutoMock.sln` passes with no warnings
- [ ] `dotnet test src/Objectivity.AutoFixture.XUnit2.AutoMock.sln` passes on all framework slices
- [ ] Code coverage is not degraded (Codecov verifies on CI)
- [ ] Mutation score is not degraded (Stryker verifies on CI)
- [ ] New tests follow GIVEN/WHEN/THEN naming and AAA structure
- [ ] No new `[SuppressMessage]` without a justification comment
- [ ] No `// TODO:` comments added — open a GitHub issue instead
- [ ] No new dependencies incompatible with the MIT license

**4. Open the PR**

Use the prefilled template from `assets/pr-template.md`:

```bash
gh pr create \
  --title "<type>(scope): <description>" \
  --body "$(cat .ai/skills/create-branch-pr/assets/pr-template.md)"
```

In the PR body:

- Replace `Closes #` with the actual GitHub issue number if applicable
- Add a label to the PR after creation (`gh pr edit <number> --add-label "<label>"`)
- The `@coderabbitai summary` placeholder will be filled automatically by CodeRabbit on review

**5. Link the backlog task** (if a TASK-N was provided)

```bash
npx backlog task edit TASK-N --status "Done"
```

## Rules

- Direct pushes to `master` are not allowed — always use a feature branch
- All checklist items must be confirmed before opening the PR
- The PR title must follow Conventional Commits format
