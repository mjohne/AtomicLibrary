# AtomicLibrary

[🇬🇧 English version](README.en.md)

Umfangreiche, wissenschaftlich orientierte C#/.NET-8-Klassenbibliothek für Atom-, Isotop- und Periodensystem-Eigenschaften inkl. WinForms-Demo. Das Domänenmodell ist klar von der Datenspeicherung getrennt:

```text
Element
   └── Isotope
          └── Atom
```

## Inhaltsverzeichnis

- [Features](#features)
- [Projektstruktur](#projektstruktur)
- [Voraussetzungen](#voraussetzungen)
- [Installation](#installation)
- [Bauen und Testen](#bauen-und-testen)
- [WinForms-Demo starten](#winforms-demo-starten)
- [Datenquellen-Konventionen](#datenquellen-konventionen)
- [Mitwirken](#mitwirken)
- [Lizenz](#lizenz)

## Features

- Vollständiges Periodensystem mit 118 Elementen (Ordnungszahl, Symbol, Name, Atomgewicht, physikalische Eigenschaften)
- Isotopendaten inkl. Halbwertszeiten, natürlicher Häufigkeiten und Zerfallsketten
- Klare Trennung von Domänenmodell (`Element`, `Isotope`, `Atom`) und Datenzugriff über JSON-Loader
- Physikalische Konstanten und Berechnungsfunktionen (`AtomicLibrary.Physics`)
- WinForms-Demo-Anwendung zur interaktiven Anzeige, Filterung und Detailansicht von Elementen und Isotopen
- Getestet mit xUnit

## Projektstruktur

- `src/AtomicLibrary.Core` — Domänenmodell (`Element`, `Isotope`, `Atom`, Elektronenkonfiguration)
- `src/AtomicLibrary.PeriodicTable` — Periodensystem-Repository und JSON-Loader
- `src/AtomicLibrary.Isotopes` — Isotopen-Repository, JSON-Loader, Zerfallskettenstruktur
- `src/AtomicLibrary.Physics` — Konstanten und physikalische Berechnungen
- `src/AtomicLibrary.WinFormsDemo` — WinForms-Demo (Anzeige + Filter + Details)
- `data/` — `elements.json`, `isotopes.json`, `decay-chains.json`
- `tests/AtomicLibrary.Tests` — xUnit-Tests

## Voraussetzungen

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows für die Ausführung der WinForms-Demo (WinForms ist plattformspezifisch für Windows)

## Installation

```bash
git clone https://github.com/mjohne/AtomicLibrary.git
cd AtomicLibrary
dotnet restore AtomicLibrary.sln
```

## Bauen und Testen

```bash
dotnet build AtomicLibrary.sln
dotnet test tests/AtomicLibrary.Tests/AtomicLibrary.Tests.csproj
```

## WinForms-Demo starten

```bash
dotnet run --project src/AtomicLibrary.WinFormsDemo
```

Die Demo lädt `data/elements.json` und `data/isotopes.json` beim Start, zeigt alle 118 Elemente in einer filterbaren Tabelle und stellt für das ausgewählte Element Detaildaten und bekannte Isotope inklusive Zerfallsketten dar.

## Datenquellen-Konventionen

- Elementdaten orientieren sich an etablierten Referenzwerten (u. a. IUPAC-Atomgewichte, typische Literaturwerte für physikalische Eigenschaften).
- Isotopendaten orientieren sich an Nukliddatenbanken-Konventionen (z. B. NUBASE/NNDC NuDat, inkl. Halbwertszeiten und natürlicher Häufigkeiten soweit verfügbar).
- Für experimentell unsichere/extrem kurzlebige superschwere Nuklide/Elemente können Felder als `0` oder `null` vorliegen.

## Mitwirken

Beiträge sind willkommen! Bitte erstelle für größere Änderungen zunächst ein Issue, um dein Vorhaben zu besprechen. Für kleinere Fixes oder Verbesserungen kannst du direkt einen Pull Request öffnen.

1. Repository forken
2. Feature-Branch erstellen (`git checkout -b feature/mein-feature`)
3. Änderungen committen (`git commit -m 'Beschreibung der Änderung'`)
4. Branch pushen (`git push origin feature/mein-feature`)
5. Pull Request öffnen

## Lizenz

Für dieses Projekt wurde bisher keine Lizenz hinterlegt. Bitte kontaktiere den Repository-Inhaber, bevor du den Code außerhalb dieses Repositorys verwendest.
