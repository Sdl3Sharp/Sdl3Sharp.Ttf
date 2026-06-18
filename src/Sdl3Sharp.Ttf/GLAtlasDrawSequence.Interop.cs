#if SDL_TTF3_3_0_OR_GREATER

using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

partial struct GLAtlasDrawSequence
{
	[StructLayout(LayoutKind.Sequential)]
	internal readonly struct TTF_GLAtlasDrawSequence
	{
		public readonly uint AtlasTexture;
		public unsafe readonly GLAtlasDrawVertex* Vertices;
		public readonly int NumVertices;
		public unsafe readonly ushort* Indices;
		public readonly int NumIndices;
		public readonly ImageType ImageType;

		public unsafe readonly TTF_GLAtlasDrawSequence* Next;
	}
}

#endif
