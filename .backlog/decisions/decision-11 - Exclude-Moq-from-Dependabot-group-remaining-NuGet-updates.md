---
id: decision-11
title: Exclude Moq from Dependabot; group remaining NuGet updates
date: '2023-09-21'
status: accepted
---
## Context

Moq introduced SponsorLink in a controversial update that embedded telemetry and caused
significant community backlash. Automatically upgrading Moq via Dependabot could silently
introduce this behavior without a deliberate review.

## Decision

Explicitly exclude `Moq` from Dependabot NuGet updates — it requires manual review and a
deliberate upgrade decision. Group all other NuGet updates into logical Dependabot groups:
`xUnit`, `AutoFixture`, `Analyzers`, `Testing`, `Common`, and `Other`. GitHub Actions
dependencies are also grouped and updated weekly with a `chore(github-actions):` prefix.

## Consequences

- Moq version is frozen until explicitly and consciously upgraded.
- Grouped updates reduce weekly PR noise from many individual bumps to a handful of grouped PRs.
- Any future Moq upgrade must be reviewed for licensing, telemetry, and breaking changes before merging.
