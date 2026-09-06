# AtomicLibrary

Wissenschaftlich orientierte C#/.NET-8-Klassenbibliothek für Atomeigenschaften mit Trennung von Domänenmodell und Datenspeicherung:

```text
Element
   └── Isotope
          └── Atom
```

## Projektstruktur

- `src/AtomicLibrary.Core` — Domänenmodell (`Element`, `Isotope`, `Atom`, Elektronenkonfiguration)
- `src/AtomicLibrary.PeriodicTable` — Periodensystem-Repository und JSON-Loader
- `src/AtomicLibrary.Isotopes` — Isotopen-Repository, JSON-Loader, Zerfallskettenstruktur
- `src/AtomicLibrary.Physics` — Konstanten und physikalische Berechnungen
- `src/AtomicLibrary.WinFormsDemo` — WinForms-Demo (Anzeige + Filter + Details)
- `data/` — `elements.json`, `isotopes.json`, `decay-chains.json`
- `tests/AtomicLibrary.Tests` — xUnit-Tests

## Datenquellen-Konventionen

- Elementdaten orientieren sich an etablierten Referenzwerten (u. a. IUPAC-Atomgewichte, typische Literaturwerte für physikalische Eigenschaften).
- Isotopendaten orientieren sich an Nukliddatenbanken-Konventionen (z. B. NUBASE/NNDC NuDat, inkl. Halbwertszeiten und natürlicher Häufigkeiten soweit verfügbar).
- Für experimentell unsichere/extrem kurzlebige superschwere Nuklide/Elemente können Felder als `0` oder `null` vorliegen.

## Bauen und Testen

```bash
dotnet build AtomicLibrary.sln
dotnet test tests/AtomicLibrary.Tests/AtomicLibrary.Tests.csproj
```

## WinForms-Demo starten

```bash
dotnet run --project src/AtomicLibrary.WinFormsDemo
```

Die Demo lädt `data/elements.json` und `data/isotopes.json` beim Start, zeigt alle 118 Elemente in einer filterbaren Tabelle und stellt für das ausgewählte Element Detaildaten und bekannte Isotope dar.
