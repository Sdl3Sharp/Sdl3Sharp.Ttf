using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

partial struct GpuAtlasDrawData
{
	[StructLayout(LayoutKind.Sequential)]
	internal readonly ref struct Node
	{
		public readonly GpuAtlasDrawSequence Sequence;
		public unsafe readonly Node* Next;
	}
}
