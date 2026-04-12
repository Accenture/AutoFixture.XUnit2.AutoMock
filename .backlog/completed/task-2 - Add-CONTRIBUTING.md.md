---
id: TASK-2
title: Add CONTRIBUTING.md
status: Done
assignee:
  - Cursor
  - piotrzajac
created_date: '2026-04-07 20:49'
updated_date: '2026-04-08 07:27'
labels:
  - doc
  - dx
dependencies: []
priority: medium
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
Create a CONTRIBUTING.md at the repository root that covers: local developer setup, how to run tests (all frameworks), how to run mutation tests, commit message conventions (Conventional Commits), PR process, and a note on AI-assisted development workflow with AGENTS.md.
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria
<!-- AC:BEGIN -->
- [x] #1 CONTRIBUTING.md exists at the repository root
- [x] #2 Covers: prerequisites, build, test, pack, commit conventions, PR workflow
- [x] #3 References AGENTS.md and CLAUDE.md for AI assistant context
<!-- AC:END -->

## Implementation Plan

<!-- SECTION:PLAN:BEGIN -->
Implementation plan (for TASK-2):

1. Review existing repo docs for consistency:
   - `AGENTS.md`
   - `CLAUDE.md`
   - `README.md`
   - `.github/pull_request_template.md` (if present)

2. Create `CONTRIBUTING.md` at the repository root covering the required topics:
   - Local developer setup / prerequisites (Windows note for net48/net472)
   - How to run tests for the solution and how framework targets matter
   - How to run mutation tests (dotnet-stryker): tool install + `dotnet stryker -f ../stryker-config.yml` from `src/`
   - Build and pack commands (`dotnet build`, `dotnet pack`)
   - Commit message conventions using Conventional Commits (include examples)
   - PR workflow (branching, opening PR, running required checks, keeping PR description aligned)
   - AI-assisted development workflow note that explicitly references `AGENTS.md` and `CLAUDE.md`

3. Validate acceptance criteria:
   - Confirm `CONTRIBUTING.md` exists at the repo root
   - Confirm it includes prerequisites, build/test/pack, commit conventions, PR workflow, and references the two AI-doc files

4. Run a quick build sanity check to avoid accidental repo breakage:
   - `dotnet build src/Objectivity.AutoFixture.XUnit2.AutoMock.sln`

5. Finalize the task in Backlog:
   - Mark acceptance criteria as checked
   - Add task notes + a PR-style final summary
   - Set task status to `Done`

6. Run test sanity check (as docs-only change guardrail): `dotnet test src/Objectivity.AutoFixture.XUnit2.AutoMock.sln --framework net8.0` (or equivalent fastest slice).

7. Update acceptance criteria status + finalize Backlog notes/summary; set task status to `Done`.
<!-- SECTION:PLAN:END -->

## Implementation Notes

<!-- SECTION:NOTES:BEGIN -->
Created repository-root `CONTRIBUTING.md` with the required developer setup + commands sections, mutation testing instructions, Conventional Commits examples, PR workflow pointer to `.github/pull_request_template.md`, and AI-assistant notes referencing `AGENTS.md` and `CLAUDE.md`.

Sanity checks executed after documentation update: `dotnet build src/Objectivity.AutoFixture.XUnit2.AutoMock.sln` (0 warnings) and `dotnet test src/Objectivity.AutoFixture.XUnit2.AutoMock.sln --framework net8.0` (268 + 24 tests per module slices; all passed).
<!-- SECTION:NOTES:END -->

## Final Summary

<!-- SECTION:FINAL_SUMMARY:BEGIN -->
Added `CONTRIBUTING.md` at the repository root with local developer setup, build/test/pack commands (including framework-slice guidance), mutation testing instructions (Stryker.NET), Conventional Commits examples, and PR workflow guidance (pointing to `.github/pull_request_template.md`). The doc also includes an AI-assisted development note referencing `AGENTS.md` and `CLAUDE.md`.

Sanity checks:
- `dotnet build src/Objectivity.AutoFixture.XUnit2.AutoMock.sln`
- `dotnet test src/Objectivity.AutoFixture.XUnit2.AutoMock.sln --framework net8.0` (all passed).
<!-- SECTION:FINAL_SUMMARY:END -->
