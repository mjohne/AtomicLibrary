namespace AtomicLibrary.Core.Elements;

/// <summary>Represents the category of an element in the periodic table.</summary>
public enum ElementCategory
{
	/// <summary>Represents an alkali metal, which is a group of chemical elements in Group 1 of the periodic table. Alkali metals are highly reactive and have a single electron in their outermost shell.</summary>
	AlkaliMetal,
	/// <summary>Represents an alkaline earth metal, which is a group of chemical elements in Group 2 of the periodic table. Alkaline earth metals are less reactive than alkali metals and have two electrons in their outermost shell.</summary>
	AlkalineEarthMetal,
	/// <summary>Represents a transition metal, which is a group of chemical elements found in the d-block of the periodic table. Transition metals are characterized by their ability to form variable oxidation states and often exhibit metallic properties.</summary>
	TransitionMetal,
	/// <summary>Represents a post-transition metal, which is a group of chemical elements found in the p-block of the periodic table. Post-transition metals are typically softer and have lower melting points than transition metals.</summary>
	PostTransitionMetal,
	/// <summary>Represents a metalloid, which is a group of chemical elements that have properties of both metals and nonmetals. Metalloids are typically semiconductors and can exhibit characteristics of both metallic and non-metallic behavior.</summary>
	Metalloid,
	/// <summary>Represents a nonmetal, which is a group of chemical elements that lack metallic properties. Nonmetals are typically poor conductors of heat and electricity and can exist in various states (solid, liquid, or gas) at room temperature.</summary>
	Nonmetal,
	/// <summary>Represents a halogen, which is a group of chemical elements found in Group 17 of the periodic table. Halogens are highly reactive nonmetals and have seven electrons in their outermost shell.</summary>
	Halogen,
	/// <summary>Represents a noble gas, which is a group of chemical elements found in Group 18 of the periodic table. Noble gases are characterized by their low reactivity and have a full outer electron shell, making them stable and inert under normal conditions.</summary>
	NobleGas,
	/// <summary>Represents a lanthanide, which is a group of chemical elements found in the f-block of the periodic table. Lanthanides are known for their high magnetic susceptibility and are often used in various technological applications, including magnets and phosphors.</summary>
	Lanthanide,
	/// <summary>Represents an actinide, which is a group of chemical elements found in the f-block of the periodic table. Actinides are typically radioactive and are known for their ability to form multiple oxidation states.</summary>
	Actinide
}
