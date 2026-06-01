using System;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents flags associated with a <see cref="SubString"/>
/// </summary>
[Flags]
public enum SubStringFlags : uint
{
	/// <summary>A mask for the flow <see cref="Direction"/> of the substring</summary>
	DirectionMask = 0x000000FF,

	/// <summary>The substring contains the beginning of the text</summary>
	TextStart = 0x00000100,

	/// <summary>The substring contains the beginning of the line with line index <see cref="SubString.LineIndex"/></summary>
	LineStart = 0x00000200,

	/// <summary>The substring contains the end of the line with line index <see cref="SubString.LineIndex"/></summary>
	LineEnd = 0x00000400,

	/// <summary>The substring contains the end of the text</summary>
	TextEnd = 0x00000800,
}
