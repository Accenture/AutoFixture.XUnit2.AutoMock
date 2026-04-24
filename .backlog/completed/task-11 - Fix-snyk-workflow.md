---
id: TASK-11
title: Fix snyk workflow
status: Done
assignee:
  - claude
  - piotrzajac
created_date: '2026-04-12'
updated_date: '2026-04-12'
labels: [ci-cd, sec]
dependencies: []
priority: medium
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
Two issues need fixing in `.github/workflows/snyk.yml`:

### Issue 1 - Deprecated action

All three scan/monitor steps use `snyk/actions/dotnet@master`, which is officially
deprecated and no longer supported by Snyk (no .NET-specific replacement exists).

The recommended migration is `snyk/actions/setup@master` (installs the Snyk CLI only)
combined with explicit `run: snyk ...` commands. Since the workflow already runs on
`ubuntu-latest`, the Docker-based `setup` action works without any runner change.

### Issue 2 - Multiple SARIF runs under the same category

The single `upload-sarif` step points to the `snyk/` directory, which contains two
SARIF files (`opensource.sarif` and `code.sarif`). GitHub Code Scanning no longer
allows multiple SARIF runs uploaded under the same category (announced 2025-07-21),
causing the workflow to fail with:

> The CodeQL Action does not support uploading multiple SARIF runs with the same
> category. Please update your workflow to upload a single run per category.

**Fix:** replace the single directory upload with two steps, each pointing to a
specific file with a distinct `category`. The `category` parameter creates an
independent slot in the GitHub Advanced Security dashboard - uploads coexist and
neither overwrites the other.
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria

- [x] #1 All three `snyk/actions/dotnet@master` steps are replaced with `snyk/actions/setup@master` + `run:` commands
- [x] #2 The single `upload-sarif` directory step is replaced by two file-specific steps
- [x] #3 Each upload step specifies a distinct `category` (`snyk-opensource` and `snyk-code`)
- [x] #4 Both upload steps retain `if: ${{ always() }}` so results upload even when scans report findings
- [x] #5 The workflow runs without error
