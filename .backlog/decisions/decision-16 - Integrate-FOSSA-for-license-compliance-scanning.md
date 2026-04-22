---
id: decision-16
title: Integrate FOSSA for license compliance scanning
date: '2023-03-31'
status: accepted
---
## Context

Open-source libraries carry licensing obligations for both maintainers and consumers.
Transitive NuGet dependencies may introduce copyleft or commercially incompatible licenses
that must be detected before distribution. Manual license auditing does not scale as
the dependency graph grows.

## Decision

Integrate FOSSA into the CI pipeline to automatically scan all direct and transitive
NuGet dependencies for license compatibility on every build. FOSSA generates a compliance
report and raises alerts when new dependencies introduce problematic licenses.

## Consequences

- License violations are caught before packages are published to NuGet.org.
- A machine-readable compliance report is produced automatically.
- Reduces legal risk for enterprise consumers who have strict license policies.
