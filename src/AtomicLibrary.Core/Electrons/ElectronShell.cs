namespace AtomicLibrary.Core.Electrons;

public readonly record struct ElectronShell(int PrincipalQuantumNumber, int ElectronCount)
{
    public int MaximumElectronCount => 2 * PrincipalQuantumNumber * PrincipalQuantumNumber;
}
