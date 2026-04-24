---
id: TASK-14
title: 'feat(docs): Migrate README into structured /docs site served via GitHub Pages'
status: To Do
assignee: []
created_date: '2026-04-12'
labels:
  - doc
dependencies: []
priority: low
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
All documentation currently lives in `README.md` - a single flat file covering installation, all attributes, data filtering attributes, and tips & tricks. As the library grows this becomes hard to navigate and limits discoverability.

**Goal:** Migrate `README.md` content into a structured documentation site under a `/docs` folder, served via GitHub Pages with a dedicated GitHub Actions workflow.

**Phase 1 - Investigation (required before implementation):**
Evaluate and select a documentation tool. Candidates:

| Tool | Runtime | Notes |
| --- | --- | --- |
| [MkDocs + Material](https://squidfunk.github.io/mkdocs-material/) | Python | Markdown-native, mature, excellent search, widely used in OSS |
| [VitePress](https://vitepress.dev/) | Node.js | Vue-powered, fast, clean default theme |
| [Docusaurus](https://docusaurus.io/) | Node.js | React-powered, more opinionated, better for larger doc sets |

Selection criteria: Markdown-native, GitHub Pages compatible, minimal config, good built-in search, low maintenance burden, minimal CI runtime dependency.

**Phase 2 - Implementation:**

1. **Create `/docs` folder** - break `README.md` content into logical pages, e.g.:
   - Getting started (installation, quick example)
   - Attributes: `AutoMockData`, `InlineAutoMockData`, `MemberAutoMockData`
   - Parameter attributes: `Frozen`, `IgnoreVirtualMembers`, `CustomizeWith`
   - Data filtering attributes: `Except`, `PickFromRange`, `PickFromValues`, `PickNegative`
   - Tips & tricks
2. **Add tool config** (e.g. `mkdocs.yml`, `vitepress.config.ts`) at the repo root or `/docs`.
3. **Add GitHub Actions workflow** under `.github/workflows/docs.yml` - build and deploy to GitHub Pages on push to `master` using `actions/upload-pages-artifact` + `actions/deploy-pages`. **Requires explicit approval before implementation** (per `AGENTS.md`).
4. **Slim down `README.md`** - keep project summary, badges, and a link to the full docs site.

**Constraints:**

- Docs build workflow can run on `ubuntu-latest` (CI currently runs on `windows-latest` for .NET Framework - docs are independent).
- Changes to `.github/workflows/` require explicit approval before implementation.

<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria

<!-- AC:BEGIN -->
- [ ] #1 A documentation tool is selected and justified based on the evaluation criteria above
- [ ] #2 `/docs` folder exists with content migrated from `README.md` into logical pages
- [ ] #3 A GitHub Actions workflow builds and deploys the site to GitHub Pages on push to `master`
- [ ] #4 GitHub Pages is enabled on the repository and the site is publicly accessible
- [ ] #5 `README.md` is slimmed down to a project summary with a link to the docs site
<!-- AC:END -->

## Files Affected

- `/docs/` (new folder)
- `README.md` (slimmed down)
- `.github/workflows/docs.yml` (new - requires explicit approval)
- Tool-specific config file at repo root (e.g. `mkdocs.yml`, `vitepress.config.ts`)
