namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents the hinting mode used for rendering fonts
/// </summary>
/// <remarks>
/// <para>
/// The hinting mode determines how much a font's outlines are adjusted for better alignment on the pixel grid.
/// </para>
/// </remarks>
public enum Hinting
{
	/// <summary>Represents an invalid hinting</summary>
	Invalid = -1,

	/// <summary>Normal hinting applies standard grid-fitting</summary>
	Normal,

	/// <summary>Light hinting applies a more subtle adjustments to improve rendering</summary>
	Light,

	/// <summary>Monochrome hinting ajusts the font for better rendering at lower resolutions</summary>
	Mono,

	/// <summary>No hinting, the font is rendered without any fitting or adjustments</summary>
	None,

	/// <summary>Light hinting with subpixel rendering for more precise font edges</summary>
	LightSubpixel,
}
