---
id: decision-8
title: Share fixture across member data rows by default
date: '2017-01-20'
status: accepted
---
## Context

`[MemberAutoMockData]` combines static member data with auto-generated parameters. When a
member provides multiple data rows, each row could receive its own fresh `IFixture` instance
(producing different mock objects per row) or share one fixture across all rows (preserving
mock identity). The right default was non-obvious.

## Decision

Default `MemberAutoDataBaseAttribute.ShareFixture = true`. The same `IFixture` instance —
with all its customizations and registered mock objects — is reused across every data row
produced by the member. Users can opt out with `ShareFixture = false` when independent
fixtures per row are required.

## Consequences

- Related test cases receive consistent mock instances, which is the expected behavior in the majority of scenarios where member data provides complementary inputs.
- This is the unique differentiator of `[MemberAutoMockData]` over plain `[MemberData]` with manual fixture setup.
- Tests that need isolation between rows require explicit `ShareFixture = false`.
