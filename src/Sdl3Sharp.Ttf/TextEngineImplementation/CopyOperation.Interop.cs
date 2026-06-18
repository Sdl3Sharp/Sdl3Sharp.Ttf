using Sdl3Sharp.Video.Drawing;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

partial struct DrawOperation
{
	partial struct TTF_DrawOperation
	{
		[FieldOffset(0)] public readonly CopyOperation.TTF_CopyOperation Copy;
	}
}

partial struct CopyOperation
{
	[StructLayout(LayoutKind.Sequential)]
	internal readonly struct TTF_CopyOperation
	{
		public readonly DrawCommand Cmd;
		public readonly int TextOffset;
		public unsafe readonly Font.TTF_Font* GlyphFont;
		public readonly uint GlyphIndex;
		public readonly Rect<int> Src;
		public readonly Rect<int> Dst;
		private unsafe readonly void* mReserved;
	}
}
