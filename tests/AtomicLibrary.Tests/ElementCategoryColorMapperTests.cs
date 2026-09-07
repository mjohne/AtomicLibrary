using AtomicLibrary.Core.Elements;

using System.Diagnostics;
using System.Drawing;

namespace AtomicLibrary.Tests;

/// <summary>Contains unit tests for <see cref="ElementCategoryColorMapper"/>.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class ElementCategoryColorMapperTests
{
	/// <summary>Tests that each element category maps to the expected background color.</summary>
	/// <param name="category">The element category.</param>
	/// <param name="expectedColorName">The expected color name.</param>
	[Theory]
	[InlineData(ElementCategory.AlkaliMetal, nameof(Color.LightCoral))]
	[InlineData(ElementCategory.AlkalineEarthMetal, nameof(Color.PeachPuff))]
	[InlineData(ElementCategory.TransitionMetal, nameof(Color.Khaki))]
	[InlineData(ElementCategory.PostTransitionMetal, nameof(Color.LightGreen))]
	[InlineData(ElementCategory.Metalloid, nameof(Color.Turquoise))]
	[InlineData(ElementCategory.Nonmetal, nameof(Color.LightSkyBlue))]
	[InlineData(ElementCategory.Halogen, nameof(Color.PaleGreen))]
	[InlineData(ElementCategory.NobleGas, nameof(Color.MediumPurple))]
	[InlineData(ElementCategory.Lanthanide, nameof(Color.LightPink))]
	[InlineData(ElementCategory.Actinide, nameof(Color.MediumVioletRed))]
	public void GetBackColorReturnsExpectedColor(ElementCategory category, string expectedColorName)
	{
		Color actual = ElementCategoryColorMapper.GetBackColor(category: category);
		Color expected = Color.FromName(name: expectedColorName);
		Assert.Equal(expected: expected.ToArgb(), actual: actual.ToArgb());
	}

	/// <summary>Tests that categories with dark background colors use white foreground text.</summary>
	[Theory]
	[InlineData(ElementCategory.NobleGas)]
	[InlineData(ElementCategory.Actinide)]
	public void GetForeColorReturnsWhiteForDarkCategories(ElementCategory category)
	{
		Assert.Equal(expected: Color.White.ToArgb(), actual: ElementCategoryColorMapper.GetForeColor(category: category).ToArgb());
	}

	/// <summary>Tests that other categories use black foreground text.</summary>
	[Theory]
	[InlineData(ElementCategory.AlkaliMetal)]
	[InlineData(ElementCategory.AlkalineEarthMetal)]
	[InlineData(ElementCategory.TransitionMetal)]
	[InlineData(ElementCategory.PostTransitionMetal)]
	[InlineData(ElementCategory.Metalloid)]
	[InlineData(ElementCategory.Nonmetal)]
	[InlineData(ElementCategory.Halogen)]
	[InlineData(ElementCategory.Lanthanide)]
	public void GetForeColorReturnsBlackForLightCategories(ElementCategory category)
	{
		Assert.Equal(expected: Color.Black.ToArgb(), actual: ElementCategoryColorMapper.GetForeColor(category: category).ToArgb());
	}

	/// <summary>Gets a string representation of the current instance for debugging purposes.</summary>
	/// <returns>A string representation of the current instance.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString() ?? string.Empty;
	}
}
