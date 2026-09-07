using AtomicLibrary.Core.Elements;

namespace AtomicLibrary.WinFormsDemo;

/// <summary>Provides UI colors for element categories.</summary>
internal static class ElementCategoryColorMapper
{
	/// <summary>Gets the background color for the specified element category.</summary>
	/// <param name="category">The element category.</param>
	/// <returns>The background color to use for the category.</returns>
	public static Color GetBackColor(ElementCategory category)
	{
		return category switch
		{
			ElementCategory.AlkaliMetal => Color.LightCoral,
			ElementCategory.AlkalineEarthMetal => Color.PeachPuff,
			ElementCategory.TransitionMetal => Color.Khaki,
			ElementCategory.PostTransitionMetal => Color.LightGreen,
			ElementCategory.Metalloid => Color.Turquoise,
			ElementCategory.Nonmetal => Color.LightSkyBlue,
			ElementCategory.Halogen => Color.PaleGreen,
			ElementCategory.NobleGas => Color.MediumPurple,
			ElementCategory.Lanthanide => Color.LightPink,
			ElementCategory.Actinide => Color.MediumVioletRed,
			_ => Color.White
		};
	}

	/// <summary>Gets the foreground color for the specified element category.</summary>
	/// <param name="category">The element category.</param>
	/// <returns>The foreground color to use for the category.</returns>
	public static Color GetForeColor(ElementCategory category)
	{
		return category switch
		{
			ElementCategory.AlkaliMetal => Color.Black,
			ElementCategory.AlkalineEarthMetal => Color.Black,
			ElementCategory.TransitionMetal => Color.Black,
			ElementCategory.PostTransitionMetal => Color.Black,
			ElementCategory.Metalloid => Color.Black,
			ElementCategory.Nonmetal => Color.Black,
			ElementCategory.Halogen => Color.Black,
			ElementCategory.NobleGas => Color.White,
			ElementCategory.Lanthanide => Color.Black,
			ElementCategory.Actinide => Color.White,
			_ => Color.Black
		};
	}
}
