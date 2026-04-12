---
id: TASK-9
title: Fix test-mutations workflow to use dotnet tool restore
status: Done
assignee:
  - piotrzajac
  - claude
created_date: '2026-04-12'
updated_date: '2026-04-12'
labels:
  - ci-cd
  - fix
dependencies:
  - TASK-3
priority: medium
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
TASK-3 added `dotnet-stryker` to the repository-level tool manifest (`dotnet-tools.json` at repo root) alongside Husky and CommitLint.Net. The `.github/workflows/test-mutations.yml` workflow was not updated accordingly and still contains a dedicated "install stryker.net" step that:

1. Runs `dotnet new tool-manifest` — creating a brand-new `.config/dotnet-tools.json` in the working directory, which conflicts with the repo-level manifest.
2. Runs `dotnet tool install --local dotnet-stryker` — redundantly re-installing a tool that is already declared in the repo manifest.

This step must be replaced with `dotnet tool restore`, which restores all tools (husky, commitlint.net, dotnet-stryker) from the existing manifest in a single, consistent step.
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria

<!-- AC:BEGIN -->
- [x] #1 The "💾 install stryker.net" step is removed from `test-mutations.yml`
- [x] #2 A `dotnet tool restore` step is added in its place, restoring tools from the repo-level manifest
- [x] #3 The "👾 test mutations" step is unchanged and still invokes `dotnet tool run dotnet-stryker`
<!-- AC:END -->

## Implementation Plan

<!-- SECTION:PLAN:BEGIN -->
In `.github/workflows/test-mutations.yml`, replace the multi-line PowerShell "💾 install stryker.net" step (which called `dotnet new tool-manifest` + `dotnet tool install`) with a single step that runs `dotnet tool restore`. The subsequent mutation-testing step needs no changes as it already invokes `dotnet tool run dotnet-stryker`.
<!-- SECTION:PLAN:END -->

## Implementation Notes

<!-- SECTION:NOTES:BEGIN -->
`dotnet tool restore` resolves the manifest by walking up the directory tree from the working directory, finding `dotnet-tools.json` at the repo root. No path argument is needed.

The old step used `dotnet new tool-manifest` which would create `.config/dotnet-tools.json` in the runner's working directory — a different path from the repo-root `dotnet-tools.json` — causing a manifest collision.
<!-- SECTION:NOTES:END -->
