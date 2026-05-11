---
id: TASK-20
title: Create AI agent skills for repository workflows
status: Done
assignee:
  - claude
  - piotrzajac
created_date: '2026-05-08 21:32'
updated_date: '2026-05-08 21:47'
labels:
  - agent
  - skills
dependencies: []
priority: medium
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
The repository has comprehensive conventions in AGENTS.md (29 decisions, BDD test naming, AAA
structure, extension model) but no reusable skill prompts for AI agents. Every new session
repeats the same orientation work. Skills encode these repeated workflows as self-contained
prompt files, reducing per-session setup time and keeping every agent consistent.

### Storage layout

```text
.ai/skills/
├── task-start/
│   └── SKILL.md
├── task-finish/
│   └── SKILL.md
├── validate/
│   └── SKILL.md
├── add-attribute/
│   ├── SKILL.md
│   └── references/
│       └── extension-model.md
├── review-decisions/
│   └── SKILL.md
├── new-test/
│   ├── SKILL.md
│   └── assets/
│       └── test-template.md
└── create-branch-pr/
    ├── SKILL.md
    └── assets/
        └── pr-template.md
```

Each `SKILL.md` follows the [agentskills.io open standard](https://agentskills.io/home) with
required `name` and `description` frontmatter and step-by-step Markdown body (<500 lines).

Claude Code thin wrappers in `.claude/commands/*.md` point to the canonical `.ai/skills/`
files to avoid duplication while providing native `/slash-command` support.

### Skill definitions

**`task-start` (arg: TASK-N)** — Before writing code: load task and ACs from backlog, surface
related decisions, suggest branch name, remind to mark In Progress.

**`task-finish` (arg: TASK-N)** — After implementation: confirm all ACs, map changed files
to decisions, verify test naming (decision-24) and structure (decision-25), suggest
Conventional Commits message, update task to Done, show PR checklist.

**`validate` (no arg)** — Pre-commit: `dotnet build` (0 warnings), `dotnet test --framework
net10.0`, scan test files for missing `DisplayName`, `Test` suffix/prefix, wrong attribute
ordering.

**`add-attribute` (arg: AttributeName)** — Scaffold a new Core parameter attribute: sealed
class (decision-28), `NotNull()` guard (decision-29), StyleCop-compliant usings, matching
test class, AGENTS.md table update.

**`review-decisions` (no arg)** — Audit `git diff HEAD` against all 29 decisions; output
compliance table with OK / Review / Violation per decision.

**`new-test` (arg: SubjectClass)** — Guide writing a test: BDD naming template
(decision-24), AAA scaffold (decision-25), `[Collection]`/`[Trait]` boilerplate,
attribute-ordering reminder, no `new Fixture()`.

**`create-branch-pr` (arg: TASK-N optional)** — Guide branch creation and PR opening:
validate branch name convention, prefill PR description from `.github/pull_request_template.md`
(with `@coderabbitai summary` and `Closes #`), walk through full checklist, suggest
`gh pr create` command.
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria
<!-- AC:BEGIN -->
- [x] #1 `.ai/skills/` directory exists with 7 skill subdirectories
- [x] #2 Each skill directory contains a valid `SKILL.md` with `name` and `description` frontmatter following agentskills.io spec
- [x] #3 `task-start` skill loads backlog task, surfaces related decisions, suggests branch name
- [x] #4 `task-finish` skill confirms ACs, checks decisions, suggests commit message, updates task status
- [x] #5 `validate` skill runs dotnet build and test and scans for test convention violations
- [x] #6 `add-attribute` skill scaffolds sealed attribute class and test following decisions 26-29
- [x] #7 `review-decisions` skill maps changed files to decisions and outputs a compliance table
- [x] #8 `new-test` skill scaffolds BDD/AAA test following decisions 24-25
- [x] #9 `create-branch-pr` skill guides branch and PR creation using the `.github/pull_request_template.md` checklist
- [x] #10 `.claude/commands/` contains 7 thin wrapper `.md` files pointing to corresponding `.ai/skills/` files
- [x] #11 AGENTS.md has an "Available Skills" section listing all 7 skills with invocation instructions for Claude Code and other agents
<!-- AC:END -->
