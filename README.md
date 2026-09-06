# AtomicLibrary

Comprehensive, science-oriented C#/.NET 8 class library for atom, isotope, and periodic table properties, including a WinForms demo. The domain model is clearly separated from data storage:

```text
Element
   └── Isotope
          └── Atom
```

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Build and Test](#build-and-test)
- [Running the WinForms Demo](#running-the-winforms-demo)
- [Data Source Conventions](#data-source-conventions)
- [Contributing](#contributing)
- [License](#license)

## Features

- Complete periodic table with 118 elements (atomic number, symbol, name, atomic weight, physical properties)
- Isotope data including half-lives, natural abundances, and decay chains
- Clear separation of domain model (`Element`, `Isotope`, `Atom`) and data access via JSON loaders
- Physical constants and calculation functions (`AtomicLibrary.Physics`)
- WinForms demo application for interactive display, filtering, and detail views of elements and isotopes
- Tested with xUnit

## Project Structure

- `src/AtomicLibrary.Core` — Domain model (`Element`, `Isotope`, `Atom`, electron configuration)
- `src/AtomicLibrary.PeriodicTable` — Periodic table repository and JSON loader
- `src/AtomicLibrary.Isotopes` — Isotope repository, JSON loader, decay chain structure
- `src/AtomicLibrary.Physics` — Constants and physical calculations
- `src/AtomicLibrary.WinFormsDemo` — WinForms demo (display + filter + details)
- `data/` — `elements.json`, `isotopes.json`, `decay-chains.json`
- `tests/AtomicLibrary.Tests` — xUnit tests

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows to run the WinForms demo (WinForms is Windows-specific)

## Installation

```bash
git clone https://github.com/mjohne/AtomicLibrary.git
cd AtomicLibrary
dotnet restore AtomicLibrary.sln
```

## Build and Test

```bash
dotnet build AtomicLibrary.sln
dotnet test tests/AtomicLibrary.Tests/AtomicLibrary.Tests.csproj
```

## Running the WinForms Demo

```bash
dotnet run --project src/AtomicLibrary.WinFormsDemo
```

The demo loads `data/elements.json` and `data/isotopes.json` at startup, displays all 118 elements in a filterable table, and shows detailed data and known isotopes (including decay chains) for the selected element.

## Data Source Conventions

- Element data follows established reference values (e.g., IUPAC atomic weights, typical literature values for physical properties).
- Isotope data follows nuclide database conventions (e.g., NUBASE/NNDC NuDat), including half-lives and natural abundances where available.
- For experimentally uncertain/extremely short-lived superheavy nuclides/elements, fields may be `0` or `null`.

## Contributing

Contributions are welcome! For larger changes, please open an issue first to discuss what you would like to change. For smaller fixes or improvements, feel free to open a pull request directly.

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/my-feature`)
3. Commit your changes (`git commit -m 'Description of change'`)
4. Push the branch (`git push origin feature/my-feature`)
5. Open a pull request

## License

No license has been added to this project yet. Please contact the repository owner before using the code outside this repository.
