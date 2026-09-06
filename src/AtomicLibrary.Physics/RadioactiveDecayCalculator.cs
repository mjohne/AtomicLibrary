namespace AtomicLibrary.Physics;

public static class RadioactiveDecayCalculator
{
    public static double DecayConstant(double halfLifeSeconds)
    {
        if (halfLifeSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(halfLifeSeconds));
        }

        return Math.Log(2.0) / halfLifeSeconds;
    }

    public static double RemainingFraction(double halfLifeSeconds, double timeSeconds)
    {
        if (timeSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timeSeconds));
        }

        var lambda = DecayConstant(halfLifeSeconds);
        return Math.Exp(-lambda * timeSeconds);
    }

    public static double RemainingAtoms(double initialAtoms, double halfLifeSeconds, double timeSeconds)
    {
        if (initialAtoms < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialAtoms));
        }

        return initialAtoms * RemainingFraction(halfLifeSeconds, timeSeconds);
    }

    public static double Activity(double atomCount, double halfLifeSeconds)
    {
        if (atomCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(atomCount));
        }

        return DecayConstant(halfLifeSeconds) * atomCount;
    }
}
