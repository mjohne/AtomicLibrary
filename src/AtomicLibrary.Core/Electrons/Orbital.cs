using System.Diagnostics;

namespace AtomicLibrary.Core.Electrons;

/// <summary>Represents an electron orbital with a principal quantum number, type, and electron count.</summary>
/// <param name="PrincipalQuantumNumber">The principal quantum number of the orbital.</param>
/// <param name="Type">The type of the orbital (s, p, d, f).</param>
/// <param name="ElectronCount">The number of electrons in the orbital.</param>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly record struct Orbital(int PrincipalQuantumNumber, OrbitalType Type, int ElectronCount)
{
	/// <summary>Gets the maximum number of electrons that can occupy this orbital based on its type.</summary>
	public int MaximumElectronCount => Type switch
	{
		OrbitalType.S => 2,
		OrbitalType.P => 6,
		OrbitalType.D => 10,
		OrbitalType.F => 14,
		_ => 0
	};

	/// <summary>Gets a string representation of the current instance for debugging purposes.</summary>
	/// <returns>A string representation of the current instance.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString();
	}
}
