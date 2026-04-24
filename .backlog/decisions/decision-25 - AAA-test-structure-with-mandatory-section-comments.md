---
id: decision-25
title: AAA test structure with mandatory section comments
date: '2026-04-24'
status: accepted
category: testing
---
## Context

Tests without visible structural landmarks require readers to mentally parse setup code from the invocation under test from the assertion.

The question was whether to mandate the `// Arrange / // Act / // Assert` comment triad on every test method, including trivially short ones where the structure is apparent from a glance, or to leave it as an optional aid.

## Decision

Every test method must contain exactly three section comments in order:

```csharp
[Fact(DisplayName = "...")]
public void WhenX_ThenY()
{
    // Arrange
    var sut = new Subject();

    // Act
    var result = sut.DoSomething();

    // Assert
    Assert.Equal(expected, result);
}
```

Empty sections are permitted when nothing belongs there (no setup needed, or the act is implicit in the constructor call in the Assert line). An empty `// Arrange` or `// Act` comment is not noise - it is an intentional signal that the section was considered and found to have no content.

## Consequences

- The structure is scannable at a glance. Readers know where each phase begins without   parsing the code.
- Empty sections self-document the absence of setup or explicit invocation rather than leaving it ambiguous whether the section was forgotten.
- Some trivial tests feel over-structured. This is accepted: the overhead is three comment lines; the benefit is uniform scanability across the entire test suite.
