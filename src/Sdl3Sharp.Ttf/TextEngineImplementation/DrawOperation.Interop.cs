using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

partial struct DrawOperation
{
	[StructLayout(LayoutKind.Explicit)]
	internal readonly partial struct TTF_DrawOperation
	{
		[FieldOffset(0)] public readonly DrawCommand Cmd;
	}
}
