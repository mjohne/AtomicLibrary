using AtomicLibrary.Core.Isotopes;

namespace AtomicLibrary.Isotopes;

public sealed class DecayChain
{
    public required int ParentAtomicNumber { get; init; }
    public required int ParentMassNumber { get; init; }
    public required IReadOnlyList<DecayChainStep> Steps { get; init; }
}

public sealed class DecayChainStep
{
    public required int AtomicNumber { get; init; }
    public required int MassNumber { get; init; }
    public required DecayMode DecayMode { get; init; }
}
