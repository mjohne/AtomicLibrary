using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace AtomicLibrary.Core.Electrons;

public sealed class ElectronConfiguration
{
    private static readonly (int N, OrbitalType Type, int MaxElectrons)[] AufbauOrder =
    [
        (1, OrbitalType.S, 2),
        (2, OrbitalType.S, 2),
        (2, OrbitalType.P, 6),
        (3, OrbitalType.S, 2),
        (3, OrbitalType.P, 6),
        (4, OrbitalType.S, 2),
        (3, OrbitalType.D, 10),
        (4, OrbitalType.P, 6),
        (5, OrbitalType.S, 2),
        (4, OrbitalType.D, 10),
        (5, OrbitalType.P, 6),
        (6, OrbitalType.S, 2),
        (4, OrbitalType.F, 14),
        (5, OrbitalType.D, 10),
        (6, OrbitalType.P, 6),
        (7, OrbitalType.S, 2),
        (5, OrbitalType.F, 14),
        (6, OrbitalType.D, 10),
        (7, OrbitalType.P, 6)
    ];

    public ElectronConfiguration(IEnumerable<Orbital> orbitals)
    {
        Orbitals = new ReadOnlyCollection<Orbital>(orbitals.Where(o => o.ElectronCount > 0).ToList());
    }

    public IReadOnlyList<Orbital> Orbitals { get; }

    public int ElectronCount => Orbitals.Sum(o => o.ElectronCount);

    public static ElectronConfiguration FromElectronCount(int electronCount)
    {
        if (electronCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(electronCount));
        }

        var remaining = electronCount;
        var orbitals = new List<Orbital>();

        foreach (var (n, type, maxElectrons) in AufbauOrder)
        {
            if (remaining <= 0)
            {
                break;
            }

            var fill = Math.Min(remaining, maxElectrons);
            orbitals.Add(new Orbital(n, type, fill));
            remaining -= fill;
        }

        return new ElectronConfiguration(orbitals);
    }

    public static ElectronConfiguration FromAtomicNumber(int atomicNumber)
    {
        if (atomicNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(atomicNumber));
        }

        return FromElectronCount(atomicNumber);
    }

    public static ElectronConfiguration Parse(string configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration))
        {
            return new ElectronConfiguration(Array.Empty<Orbital>());
        }

        var orbitals = new List<Orbital>();
        var tokens = configuration.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var token in tokens)
        {
            if (token.StartsWith("[", StringComparison.Ordinal))
            {
                throw new FormatException("Bracketed noble-gas notation is not supported.");
            }

            var nPart = new StringBuilder();
            var typePart = '\0';
            var ePart = new StringBuilder();
            foreach (var ch in token)
            {
                if (char.IsDigit(ch) && typePart == '\0')
                {
                    nPart.Append(ch);
                }
                else if (char.IsLetter(ch) && typePart == '\0')
                {
                    typePart = char.ToLowerInvariant(ch);
                }
                else if (char.IsDigit(ch))
                {
                    ePart.Append(ch);
                }
                else
                {
                    throw new FormatException($"Invalid orbital token '{token}'.");
                }
            }

            if (nPart.Length == 0 || ePart.Length == 0)
            {
                throw new FormatException($"Invalid orbital token '{token}'.");
            }

            var n = int.Parse(nPart.ToString(), CultureInfo.InvariantCulture);
            var electrons = int.Parse(ePart.ToString(), CultureInfo.InvariantCulture);
            if (n < 1)
            {
                throw new FormatException($"Orbital '{token}' has invalid principal quantum number.");
            }

            var type = typePart switch
            {
                's' => OrbitalType.S,
                'p' => OrbitalType.P,
                'd' => OrbitalType.D,
                'f' => OrbitalType.F,
                _ => throw new FormatException($"Unsupported orbital type '{typePart}'.")
            };
            var orbital = new Orbital(n, type, electrons);
            if (electrons < 1)
            {
                throw new FormatException($"Orbital '{token}' has invalid occupancy.");
            }

            if (electrons > orbital.MaximumElectronCount)
            {
                throw new FormatException($"Orbital '{token}' exceeds maximum occupancy.");
            }

            orbitals.Add(orbital);
        }

        return new ElectronConfiguration(orbitals);
    }

    public override string ToString() => string.Join(' ', Orbitals.Select(o => $"{o.PrincipalQuantumNumber}{o.Type.ToString().ToLowerInvariant()}{o.ElectronCount}"));
}
