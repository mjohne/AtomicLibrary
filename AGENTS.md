# AGENTS.md

Guidance for AI coding agents (Claude, Copilot, Codex, etc.) working in this repository.

## Project overview

AtomicLibrary is a C#/.NET 10 class library providing a science-oriented domain model
for atoms, isotopes, and the periodic table, plus a WinForms demo. The domain model
is deliberately separated from the JSON data storage:

```text
Element
   └── Isotope
          └── Atom
```

See [README.md](README.md) for the user-facing overview.

## Repository layout

- `src/AtomicLibrary.Core` — Domain model (`Element`, `Isotope`, `Atom`, `ElectronConfiguration`)
- `src/AtomicLibrary.PeriodicTable` — Periodic table repository + JSON loader (`ElementDataLoader`)
- `src/AtomicLibrary.Isotopes` — Isotope repository, JSON loader (`IsotopeDataLoader`), decay chain structure
- `src/AtomicLibrary.Physics` — Physical constants and calculations
- `src/AtomicLibrary.WinFormsDemo` — WinForms demo application (Windows only)
- `data/` — `elements.json`, `isotopes.json`, `decay-chains.json`
- `tests/AtomicLibrary.Tests` — xUnit test project
- `AtomicLibrary.sln` — Solution file at repo root

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Windows to run the WinForms demo (WinForms is Windows-specific); other projects
  and tests build/run on any .NET 10 platform.

## Build, test, and run

Always run commands from the repository root.

```bash
# Restore
dotnet restore AtomicLibrary.sln

# Build the whole solution
dotnet build AtomicLibrary.sln

# Run the xUnit tests
dotnet test tests/AtomicLibrary.Tests/AtomicLibrary.Tests.csproj

# Run the WinForms demo (Windows)
dotnet run --project src/AtomicLibrary.WinFormsDemo
```

When changing library code, prefer running the targeted test project rather than
the full solution to keep the feedback loop tight.

## Data loading conventions

- The WinForms demo and tests load data from `AppContext.BaseDirectory/data`.
  Both the demo csproj and the test csproj link `data/*.json` as `Content`
  with `CopyToOutputDirectory` so the files land next to the built assemblies.
- If you add new JSON data files under `data/`, update the corresponding
  `<Content Include="..." Link="data\..." />` entries in
  `src/AtomicLibrary.WinFormsDemo/AtomicLibrary.WinFormsDemo.csproj` and
  `tests/AtomicLibrary.Tests/AtomicLibrary.Tests.csproj` so they are copied to
  the output directory.
- Element data follows established reference values (e.g., IUPAC atomic weights).
- Isotope data follows nuclide database conventions (NUBASE/NNDC NuDat) for
  half-lives and natural abundances.
- For experimentally uncertain or extremely short-lived superheavy nuclides,
  numeric fields may be `0` or `null`.

## Coding conventions

- Target framework: .NET 10 (`net10.0`); the demo uses `net10.0-windows`.
- Follow the existing code style (standard .NET / C# conventions, nullable
  reference types where already enabled, file-scoped namespaces where used).
- Data loaders throw contextual `FormatException` messages on invalid input;
  keep validation explicit and preserve informative error messages.
- `ElectronConfiguration.Parse` rejects principal quantum numbers below 1 —
  keep such domain invariants in the parser/validators, not in callers.
- `GlobalSuppressions.cs` files use concrete `Justification` text for every
  `SuppressMessage`. Do not add suppressions with placeholder justifications.
- Do not introduce new lint/build/test tooling unless required by the task.
  Reuse the existing `dotnet` toolchain.

## Testing conventions

- Tests use xUnit and live in `tests/AtomicLibrary.Tests`.
- Tests rely on `data/*.json` being copied to the test output directory
  (see the `<Content Link="data\..." />` entries in the test csproj).
- Add tests alongside existing ones and mirror their naming and assertion
  style. New behavior in `Core`, `PeriodicTable`, `Isotopes`, or `Physics`
  should be covered here.

## Dependencies

- NuGet updates are managed by Dependabot (weekly, root `nuget` ecosystem —
  see `.github/dependabot.yml`). Prefer letting Dependabot bump package
  versions; only add or upgrade packages when strictly necessary for the task.

## Documentation

- `README.md` is the canonical English project README — keep it in sync when
  changing project structure, build steps, or public-facing behavior.
- `SECURITY.md` and `CODE_OF_CONDUCT.md` are the community/security policies.

## Pull request guidelines

- Keep changes minimal and focused on the issue being addressed.
- Do not commit build artifacts (`bin/`, `obj/`) or IDE/user files.
- Update `README.md` when your change affects setup, build, or usage.
- Ensure `dotnet build AtomicLibrary.sln` and
  `dotnet test tests/AtomicLibrary.Tests/AtomicLibrary.Tests.csproj` succeed
  before requesting review.

## License

This project is licensed under GPL-3.0. See [LICENSE](LICENSE). Any contributed
code must be compatible with that license.
