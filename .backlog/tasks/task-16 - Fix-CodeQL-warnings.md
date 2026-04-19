---
id: TASK-16
title: Fix CodeQL warnings
status: Done
assignee:
  - piotrzajac
  - claude
created_date: '2026-04-19 14:23'
updated_date: '2026-04-19 15:13'
labels: []
dependencies: []
priority: medium
---

## Acceptance Criteria
<!-- AC:BEGIN -->
- [x] #1 Collapsible if statements combined (IgnoreVirtualMembersSpecimenBuilder)
- [x] #2 Default ToString() override added (AbstractTestClass in AutoDataAdapterAttributeTests)
- [x] #3 Change var to object to resolve incomparable-types Equals warning (ValuesRequestTests)
- [x] #4 Useless call to GetHashCode() as value is its own hash (ValuesRequestTests)
- [x] #5 AutoDataAdapterAttribute CustomizeFixture: foreach refactored to use Select projection.
- [x] #6 CustomizeWithAttributeTests: as+Assert.NotNull replaced with Assert.IsType<T> in Assert section (lines 136, 157)
- [x] #7 AutoDataAttributeProviderTests: same pattern applied (line 22)
<!-- AC:END -->
