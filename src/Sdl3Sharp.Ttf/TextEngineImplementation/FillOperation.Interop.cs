using Sdl3Sharp.Video.Drawing;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

partial struct DrawOperation
{
	partial struct TTF_DrawOperation
	{
		[FieldOffset(0)] public readonly FillOperation.TTF_FillOperation Fill;
	}
}

partial struct FillOperation
{
	[StructLayout(LayoutKind.Sequential)]
	internal readonly struct TTF_FillOperation
	{
		public readonly DrawCommand Cmd;
		public readonly Rect<int> Rect;
	}
}
