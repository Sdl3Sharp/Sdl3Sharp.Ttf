namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents the type of data contained in a glyph image
/// </summary>
public enum ImageType
{
	/// <summary>Represents an invalid image type</summary>
	Invalid,

	/// <summary>The color channels are white</summary>
	Alpha,

	/// <summary>The color channels contain actual image data</summary>
	Color,

	/// <summary>The alpha channel contains signed distance field (SDF) information</summary>
	SDF
}
