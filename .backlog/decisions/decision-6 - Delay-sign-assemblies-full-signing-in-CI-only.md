---
id: decision-6
title: Delay-sign assemblies; full signing in CI only
date: '2018-07-10'
status: accepted
---
## Context

Strong-name signing is required for .NET Framework compatibility and NuGet package trust. Committing the private signing key to source control would expose it to every contributor and fork, creating a security risk.

## Decision

Use delay signing locally: only `public.snk` (the public key) is committed to source control. Full signing is performed in CI using the `SIGNING_KEY` secret stored in GitHub Secrets. Developers can build and run tests locally without the private key; the fully signed assemblies are produced only on the CI server during the release pipeline.

## Consequences

- The private signing key is never exposed in source control.
- Local builds produce delay-signed assemblies that work for development and testing but not for direct NuGet consumption.
- Loss of the `SIGNING_KEY` secret would break releases and require re-establishing trust with a new key.
