using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

partial class TextEngine
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct TTF_TextEngine;
}
