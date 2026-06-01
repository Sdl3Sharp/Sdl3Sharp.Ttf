namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents the weight of a font, in terms of the lightness or heaviness of the strokes
/// </summary>
/// <remarks>
/// <para>
/// If you want to specify a custom font weight that is not one of the predefined values, you can use the <see cref="FontWeightExtensions.Custom(int)"/> method.
/// </para>
/// </remarks>
public enum FontWeight : int
{
	/// <summary>Thin font weight (100)</summary>
	Thin = 100,

	/// <summary>Extra-light font weight (200)</summary>
	ExtraLight = 200,

	/// <summary>Light font weight (300)</summary>
	Light = 300,

	/// <summary>Normal font weight (400)</summary>
	Normal = 400,

	/// <summary>Medium font weight (500)</summary>
	Medium = 500,

	/// <summary>Semi-bold font weight (600)</summary>
	SemiBold = 600,

	/// <summary>Bold font weight (700)</summary>
	Bold = 700,

	/// <summary>Extra-bold font weight (800)</summary>
	ExtraBold = 800,

	/// <summary>Black font weight (900)</summary>
	Black = 900,

	/// <summary>Extra-black font weight (950)</summary>
	ExtraBlack = 950,
}
