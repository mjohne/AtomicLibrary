namespace AtomicLibrary.Core.Electrons;

/// <summary>Represents an electron shell in an atom, characterized by its principal quantum number and the number of electrons it contains.</summary>
/// <param name="PrincipalQuantumNumber">The principal quantum number of the electron shell.</param>
/// <param name="ElectronCount">The number of electrons in the electron shell.</param>
public readonly record struct ElectronShell(int PrincipalQuantumNumber, int ElectronCount)
{
	/// <summary>Gets the maximum number of electrons that can occupy this electron shell based on its principal quantum number.</summary>
	public int MaximumElectronCount => 2 * PrincipalQuantumNumber * PrincipalQuantumNumber;
}
