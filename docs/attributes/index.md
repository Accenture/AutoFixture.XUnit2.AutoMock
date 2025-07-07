# Attributes Overview

This section describes the main attributes provided by AutoFixture.XUnit2.AutoMock for use in your xUnit tests:

## Test Method Attributes

- [AutoMockData](auto-mock-data-attribute.md): Provides auto-generated data specimens using AutoFixture and a mocking library.
- [InlineAutoMockData](inline-auto-mock-data-attribute.md): Combines inline values with auto-generated data specimens.
- [MemberAutoMockData](member-auto-mock-data-attribute.md): Uses static members as data sources, combined with auto-generated data.
- [IgnoreVirtualMembers](ignore-virtual-members-attribute.md): Disables generation of virtual members for a parameter or globally.
- [CustomizeWith](customize-with-attribute.md): Applies additional customization to a parameter.
- [CustomizeWith\<T>](customize-with-t-attribute.md): Generic version of CustomizeWith for ease of use.

## Data Filtering Attributes

- [Except](except-attribute.md): Ensures values from outside the specified list will be generated.
- [PickFromRange](pick-from-range-attribute.md): Ensures only values from a specified range will be generated.
- [PickNegative](pick-negative-attribute.md): Ensures only negative values will be generated.
- [PickFromValues](pick-from-values-attribute.md): Ensures only values from the specified list will be generated.
