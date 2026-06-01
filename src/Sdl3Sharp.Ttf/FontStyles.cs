using System;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents the font style flags used for rendering text
/// </summary>
[Flags]
public enum FontStyles : uint
{
	/// <summary>No particular font style</summary>
	Normal = 0x00,

	/// <summary>Bold text font style</summary>
	Bold = 0x01,

	/// <summary>Italic text font style</summary>
	Italic = 0x02,

	/// <summary>Underlined text font style</summary>
	Underline = 0x04,

	/// <summary>Strikethrough text font style</summary>
	Strikethrough = 0x08,
}
