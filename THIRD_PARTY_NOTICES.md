# Third-Party Notices

AtomicLibrary incorporates third-party material from the projects and data
sources listed below. AtomicLibrary itself is licensed under the GNU General
Public License v3.0 (see [LICENSE](LICENSE)); the notices below apply only to
the respective third-party components.

## Runtime dependencies

The AtomicLibrary runtime libraries (`AtomicLibrary.Core`,
`AtomicLibrary.PeriodicTable`, `AtomicLibrary.Isotopes`,
`AtomicLibrary.Physics`) and the `AtomicLibrary.WinFormsDemo` application
target .NET 8 and depend only on the .NET base class library and Windows Forms
provided by the .NET 8 runtime. No additional third-party NuGet packages are
referenced at runtime.

### .NET 8 (including Windows Forms)

- Project: .NET Runtime / Windows Desktop Runtime
- Homepage: <https://github.com/dotnet/runtime>, <https://github.com/dotnet/winforms>
- Copyright: © .NET Foundation and Contributors
- License: MIT License — <https://github.com/dotnet/runtime/blob/main/LICENSE.TXT>

```text
The MIT License (MIT)

Copyright (c) .NET Foundation and Contributors

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## Test dependencies

The following packages are referenced only by the `AtomicLibrary.Tests` test
project (`tests/AtomicLibrary.Tests/AtomicLibrary.Tests.csproj`). They are not
distributed with the AtomicLibrary runtime libraries.

### xUnit.net

- Package: `xunit`
- Homepage: <https://github.com/xunit/xunit>
- Copyright: © .NET Foundation and Contributors
- License: Apache License 2.0 — <https://github.com/xunit/xunit/blob/main/LICENSE>

### xunit.runner.visualstudio

- Package: `xunit.runner.visualstudio`
- Homepage: <https://github.com/xunit/visualstudio.xunit>
- Copyright: © .NET Foundation and Contributors
- License: MIT License — <https://github.com/xunit/visualstudio.xunit/blob/main/License.txt>

### Microsoft.NET.Test.Sdk

- Package: `Microsoft.NET.Test.Sdk`
- Homepage: <https://github.com/microsoft/vstest>
- Copyright: © Microsoft Corporation
- License: MIT License — <https://github.com/microsoft/vstest/blob/main/LICENSE>

### coverlet.collector

- Package: `coverlet.collector`
- Homepage: <https://github.com/coverlet-coverage/coverlet>
- Copyright: © Toni Solarin-Sodara and Coverlet contributors
- License: MIT License — <https://github.com/coverlet-coverage/coverlet/blob/master/LICENSE>

## Reference data sources

The JSON data files under `data/` (`elements.json`, `isotopes.json`,
`decay-chains.json`) contain scientific reference values compiled from
publicly available scientific literature and databases. AtomicLibrary does
not redistribute the source databases themselves; the values are widely used
scientific constants and measurements. The following sources are referenced
for the conventions applied:

- **IUPAC** — International Union of Pure and Applied Chemistry, standard
  atomic weights and periodic table conventions.
  Homepage: <https://iupac.org/>
- **NUBASE** — Evaluation of nuclear and decay properties, published by the
  Atomic Mass Data Center (AMDC).
  Homepage: <https://www-nds.iaea.org/amdc/>
- **NNDC NuDat** — National Nuclear Data Center, Brookhaven National
  Laboratory, nuclear structure and decay data.
  Homepage: <https://www.nndc.bnl.gov/nudat3/>

Users who redistribute or build upon these data values are responsible for
consulting the original sources for the applicable terms of use and citation
requirements.
