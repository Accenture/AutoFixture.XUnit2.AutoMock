---
id: decision-16
title: Publish symbols and enable SourceLink
date: '2023-10-13'
status: accepted
---
## Context

Developers using the library could not step into library code during debugging because no symbol packages were published. Stack traces showed only method names without source lines, making it difficult to diagnose failures inside the attribute pipeline.

## Decision

Publish `.snupkg` symbol packages to NuGet.org alongside the main `.nupkg` packages.
Enable `Microsoft.SourceLink.GitHub` so that debuggers automatically fetch the corresponding source code from GitHub at the exact commit matching the installed version.

## Consequences

- Consumers can step into library code in their debugger without any manual setup.
- Stack traces show full source file paths and line numbers for library frames.
- Symbol packages are published automatically as part of the same CI release step as the main packages.
