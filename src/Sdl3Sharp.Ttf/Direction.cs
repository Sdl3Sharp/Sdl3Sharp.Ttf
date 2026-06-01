namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents the direction of a font or text
/// </summary>
/// <remarks>
/// <para>
/// The predefined values for this enumeration match the <see href="https://harfbuzz.github.io/harfbuzz-hb-common.html#hb-direction-t">hb_direction_t</see> values from HarfBuzz.
/// </para>
/// </remarks>
public enum Direction
{
	/// <summary>Represents an invalid font or text direction</summary>
	Invalid = 0,

	/// <summary>Left-to-right font or text direction</summary>
	LeftToRight = 4,
	
	/// <summary>Right-to-left font or text direction</summary>
	RightToLeft,

	/// <summary>Top-to-bottom font or text direction</summary>
	TopToBottom,

	/// <summary>Bottom-to-top font or text direction</summary>
	BottomToTop,
}
