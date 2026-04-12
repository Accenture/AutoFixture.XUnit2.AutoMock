---
id: TASK-10
title: Ensure dependabot updates NuGet packages across all projects in a single PR
status: To Do
assignee:
  - piotrzajac
  - claude
created_date: '2026-04-12 16:33'
updated_date: '2026-04-12 17:33'
labels:
  - fix
  - ci-cd
dependencies: []
priority: medium
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
Dependabot was only updating ungrouped packages (e.g. Microsoft.NET.Test.Sdk, Castle.Core) in a single project per run instead of all projects simultaneously. This triggered a recurring pattern: Dependabot bumps one project, then a manual "Align versions in all projects" commit is needed.

Root cause: `directories: ["**/*"]` creates one PR per matched directory per ungrouped package. Packages already covered by a group (xUnit, AutoFixture, Analyzers) correctly produced one cross-directory PR. Ungrouped shared packages were not consolidated:

| Package | Projects | Effect |
| --- | --- | --- |
| JetBrains.Annotations | 8 | 8 separate PRs |
| Microsoft.NETFramework.ReferenceAssemblies | 8 | 8 separate PRs |
| Castle.Core | 7 | 7 separate PRs |
| Microsoft.NET.Test.Sdk | 4 | 4 separate PRs |
| coverlet.msbuild | 4 | 4 separate PRs |
| Microsoft.SourceLink.GitHub | 4 | 4 separate PRs |

With Dependabot's default open-pull-requests-limit of 5, only some directories would receive a PR for a given package before the limit was reached — explaining why only AutoFakeItEasy.Tests was updated for Microsoft.NET.Test.Sdk 18.4.0.

Evidence from git log and closed PR history:

- ecee204 Bump Microsoft.NET.Test.Sdk from 18.3.0 to 18.4.0 (only AutoFakeItEasy.Tests)
- 23b7f3b fix(dependabot): Align versions in all projects (manual follow-up)
- Castle.Core 5.2.1 generated 6 separate per-directory PRs

Fix: Keep `directories: ["**/*"]` (discovery is working correctly) and add three new dependency groups under the nuget ecosystem entry:

- Testing: Microsoft.NET.Test.Sdk, coverlet.msbuild
- Common: Castle.Core, JetBrains.Annotations, Microsoft.SourceLink.GitHub, Microsoft.NETFramework.ReferenceAssemblies
- Other: `*` catch-all — consolidates any package not matched by a named group into a single PR, guarding against future ungrouped shared packages
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria
<!-- AC:BEGIN -->
- [ ] #1 xUnit, AutoFixture, Analyzers, Testing, Common, and Other groups each produce a single cross-directory PR
- [ ] #2 No more manual 'Align versions in all projects' follow-up commits are needed
- [ ] #3 `directories: ["**/*"]` is preserved (discovery was already working correctly)
<!-- AC:END -->

## Implementation Plan

<!-- SECTION:PLAN:BEGIN -->
In .github/dependabot.yml, add three groups under the nuget ecosystem entry (order matters — Other must be last so named groups take priority):

  Testing:
    patterns:
      - "Microsoft.NET.Test.Sdk"
      - "coverlet.msbuild"
  Common:
    patterns:
      - "Castle.Core"
      - "JetBrains.Annotations"
      - "Microsoft.SourceLink.GitHub"
      - "Microsoft.NETFramework.ReferenceAssemblies"
  Other:
    patterns:
      - "*"

The change is already applied to the working tree (not yet committed). Review and commit when ready.
<!-- SECTION:PLAN:END -->
