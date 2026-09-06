namespace AtomicLibrary.Core.Electrons;

public readonly record struct Orbital(int PrincipalQuantumNumber, OrbitalType Type, int ElectronCount)
{
    public int MaximumElectronCount => Type switch
    {
        OrbitalType.S => 2,
        OrbitalType.P => 6,
        OrbitalType.D => 10,
        OrbitalType.F => 14,
        _ => 0
    };
}
