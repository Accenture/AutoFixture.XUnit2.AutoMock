---
id: decision-4
title: 'Publish Core bundled into module packages, not standalone'
date: '2018-07-17'
status: accepted
---
## Context

Core contains all shared infrastructure but has no value in isolation — it requires a mock
backend to function. Publishing it as a standalone NuGet package would invite consumers to
install it alone, producing a broken setup with no data generation capability.

## Decision

Do not publish `Objectivity.AutoFixture.XUnit2.Core` as a standalone NuGet package. Bundle
it into each of the three mock module packages via project reference so that installing any
one module brings Core along transparently.

## Consequences

- Consumers see exactly three packages on NuGet.org; no partial installations are possible.
- The NuGet dependency graph is clean: one package per mocking framework, nothing else required.
- Core is an implementation detail; its public API is effectively internal to the three published packages.
