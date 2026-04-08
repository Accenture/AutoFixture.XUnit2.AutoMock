# Contributing

Thanks for taking the time to contribute. This project is a C# NuGet library collection that bridges AutoFixture with multiple mocking libraries and exposes xUnit2 attributes to reduce test boilerplate.

## Local Setup

### Prerequisites

- .NET 8 SDK (minimum)
- .NET Framework 4.7.2 and 4.8 (Windows only, required for full `net472`/`net48` test coverage)

### Getting Started

```bash
git clone https://github.com/Accenture/AutoFixture.XUnit2.AutoMock.git
```

All build, test, and pack commands are in the [Build / Test / Pack](#build--test--pack) section below.

## Build / Test / Pack

### Build

```bash
dotnet build src/Objectivity.AutoFixture.XUnit2.AutoMock.sln
```

The build runs all analyzers with `TreatWarningsAsErrors=true` - any warning is a build failure.

### Test

Run all framework slices:

```bash
dotnet test src/Objectivity.AutoFixture.XUnit2.AutoMock.sln
```

Or run a single slice:

```bash
dotnet test src/Objectivity.AutoFixture.XUnit2.AutoMock.sln --framework net8.0
```

### Pack

NuGet packages are produced from the solution:

```bash
dotnet pack src/Objectivity.AutoFixture.XUnit2.AutoMock.sln --configuration Release
```

## Mutation Testing (Stryker.NET)

Mutation testing is executed via Stryker.NET. Install the tool globally once, then run it from the `src/` directory (the config is resolved relative to `src/`):

```bash
dotnet tool install -g dotnet-stryker
cd src
dotnet stryker -f ../stryker-config.yml
```

## Code Style

Code style is enforced by `.editorconfig` and a suite of analyzers (`StyleCop`, `Roslynator`, `SonarAnalyzer`, `xunit.analyzers`). A clean build - zero warnings - means all rules are satisfied. You do not need to run a separate linter step.

## Commit Message Conventions

Use [Conventional Commits](https://www.conventionalcommits.org/):

- Format: `type(scope): description`
- Examples:
  - `feat(core): add new attribute`
  - `fix(tests): correct GIVEN/WHEN/THEN naming`
  - `chore(ci): update build instructions`
  - `docs: clarify local development setup`

## Work Tracking

Non-trivial changes (new attributes, refactors, CI changes) require a plan and explicit approval before implementation. Work items are tracked in `.backlog` folder using `Backlog.md` - check it before starting to avoid duplicate effort.

## Pull Request Workflow

**Direct pushes to `master` are not allowed.** All changes must be introduced via a feature branch and a pull request.

1. Create a branch and implement your change.
2. For non-trivial changes, create a work item in `.backlog` (see [Work Tracking](#work-tracking)).
3. Open a PR against `master`.
4. Ensure your PR description and checklist are completed (see `.github/pull_request_template.md`).
5. Before requesting review, verify:
   - `dotnet build src/Objectivity.AutoFixture.XUnit2.AutoMock.sln` passes with no warnings
   - `dotnet test src/Objectivity.AutoFixture.XUnit2.AutoMock.sln` passes on all framework slices
   - (If applicable) coverage and mutation testing expectations remain acceptable

## AI-Assisted Development Workflow

If you are using an AI coding assistant, follow the repository guidance in:

- `AGENTS.md` (project context, architecture, conventions, and working rules)
- `CLAUDE.md` (assistant-specific instructions, including “propose before acting” for non-trivial changes)
