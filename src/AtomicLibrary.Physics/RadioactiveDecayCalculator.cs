namespace AtomicLibrary.Physics;

/// <summary>Provides methods for calculating radioactive decay properties, including decay constant, remaining fraction of atoms, remaining atom count, and activity based on half-life and time.</summary>
public static class RadioactiveDecayCalculator
{
	/// <summary>Calculates the decay constant (lambda) from the half-life in seconds.</summary>
	/// <param name="halfLifeSeconds">The half-life of the radioactive substance in seconds.</param>
	/// <returns>The decay constant (lambda) in inverse seconds.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the half-life is less than or equal to zero.</exception>
	public static double DecayConstant(double halfLifeSeconds)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value: halfLifeSeconds);
		return Math.Log(d: 2.0) / halfLifeSeconds;
	}

	/// <summary>Calculates the remaining fraction of radioactive atoms after a given time based on the half-life.</summary>
	/// <param name="halfLifeSeconds">The half-life of the radioactive substance in seconds.</param>
	/// <param name="timeSeconds">The elapsed time in seconds.</param>
	/// <returns>The remaining fraction of radioactive atoms.</returns>
	public static double RemainingFraction(double halfLifeSeconds, double timeSeconds)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(value: timeSeconds);
		double lambda = DecayConstant(halfLifeSeconds: halfLifeSeconds);
		return Math.Exp(d: -lambda * timeSeconds);
	}

	/// <summary>Calculates the remaining number of radioactive atoms after a given time based on the half-life.</summary>
	/// <param name="initialAtoms">The initial number of radioactive atoms.</param>
	/// <param name="halfLifeSeconds">The half-life of the radioactive substance in seconds.</param>
	/// <param name="timeSeconds">The elapsed time in seconds.</param>
	/// <returns>The remaining number of radioactive atoms.</returns>
	public static double RemainingAtoms(double initialAtoms, double halfLifeSeconds, double timeSeconds)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(value: initialAtoms);
		return initialAtoms * RemainingFraction(halfLifeSeconds: halfLifeSeconds, timeSeconds: timeSeconds);
	}

	/// <summary>Calculates the activity of a radioactive substance based on the number of atoms and half-life.</summary>
	/// <param name="atomCount">The number of radioactive atoms.</param>
	/// <param name="halfLifeSeconds">The half-life of the radioactive substance in seconds.</param>
	/// <returns>The activity of the radioactive substance.</returns>
	public static double Activity(double atomCount, double halfLifeSeconds)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(value: atomCount);
		return DecayConstant(halfLifeSeconds: halfLifeSeconds) * atomCount;
	}
}
