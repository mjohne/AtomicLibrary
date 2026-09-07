namespace AtomicLibrary.Core.Elements;

/// <summary>Represents the block of the periodic table an element belongs to, based on its electron configuration.</summary>
public enum ElementBlock
{
	/// <summary>Represents the s-block of the periodic table, which includes elements in groups 1 and 2, as well as helium. These elements have their outermost electrons in s orbitals.</summary>
	S,
	/// <summary>Represents the p-block of the periodic table, which includes elements in groups 13 to 18. These elements have their outermost electrons in p orbitals.</summary>
	P,
	/// <summary>Represents the d-block of the periodic table, which includes transition metals. These elements have their outermost electrons in d orbitals.</summary>
	D,
	/// <summary>Represents the f-block of the periodic table, which includes the lanthanides and actinides. These elements have their outermost electrons in f orbitals.</summary>
	F
}
