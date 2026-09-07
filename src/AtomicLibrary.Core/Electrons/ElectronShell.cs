using System.Diagnostics;

namespace AtomicLibrary.Core.Electrons;

/// <summary>Represents an electron shell in an atom, characterized by its principal quantum number and the number of electrons it contains.</summary>
/// <param name="PrincipalQuantumNumber">The principal quantum number of the electron shell.</param>
/// <param name="ElectronCount">The number of electrons in the electron shell.</param>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly record struct ElectronShell(int PrincipalQuantumNumber, int ElectronCount)
{
	/// <summary>Gets the maximum number of electrons that can occupy this electron shell based on its principal quantum number.</summary>
	public int MaximumElectronCount => 2 * PrincipalQuantumNumber * PrincipalQuantumNumber;

	/// <summary>Gets a string representation of the current instance for debugging purposes.</summary>
	/// <returns>A string representation of the current instance.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString();
	}
}
