---
id: TASK-17
title: Consolidate duplicate Facts into Theories
status: Done
assignee:
  - claude
  - piotrzajac
created_date: '2026-04-21 08:21'
updated_date: '2026-04-21 11:06'
labels: []
dependencies: []
priority: low
---

## Acceptance Criteria
<!-- AC:BEGIN -->
- [x] #1 Identify all [Fact] methods across the all test projects that test the same behavior with different inputs
- [x] #2 Replace duplicate Facts with a single [Theory] using [InlineData] or [MemberData]/[TheoryData] as appropriate
<!-- AC:END -->
