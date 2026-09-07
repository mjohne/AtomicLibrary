namespace AtomicLibrary.Core.Isotopes;

/// <summary>Represents the different modes of radioactive decay that an isotope can undergo.</summary>
public enum DecayMode
{
	/// <summary>Indicates that the isotope is stable and does not undergo radioactive decay.</summary>
	None,
	/// <summary>Indicates that the isotope undergoes alpha decay, emitting an alpha particle (helium nucleus) from its nucleus.</summary>
	Alpha,
	/// <summary>Indicates that the isotope undergoes beta-minus decay, where a neutron is converted into a proton, emitting an electron and an antineutrino.</summary>
	BetaMinus,
	/// <summary>Indicates that the isotope undergoes beta-plus decay, where a proton is converted into a neutron, emitting a positron and a neutrino.</summary>
	BetaPlus,
	/// <summary>Indicates that the isotope undergoes electron capture, where an inner orbital electron is captured by the nucleus, converting a proton into a neutron and emitting a neutrino.</summary>
	ElectronCapture,
	/// <summary>Indicates that the isotope undergoes gamma decay, emitting a gamma ray (high-energy photon) from its nucleus without changing the number of protons or neutrons.</summary>
	Gamma,
	/// <summary>Indicates that the isotope undergoes neutron emission, where a neutron is emitted from the nucleus.</summary>
	NeutronEmission,
	/// <summary>Indicates that the isotope undergoes proton emission, where a proton is emitted from the nucleus.</summary>
	ProtonEmission,
	/// <summary>Indicates that the isotope undergoes spontaneous fission, where the nucleus splits into two or more smaller nuclei along with the release of neutrons and energy.</summary>
	SpontaneousFission,
	/// <summary>Indicates that the isotope undergoes double beta-minus decay, where two neutrons are converted into two protons, emitting two electrons and two antineutrinos.</summary>
	DoubleBetaMinus,
	/// <summary>Indicates that the isotope undergoes double beta-plus decay, where two protons are converted into two neutrons, emitting two positrons and two neutrinos.</summary>
	DoubleBetaPlus
}
