using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Gpu;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

partial struct GpuAtlasDrawSequence
{
	[StructLayout(LayoutKind.Sequential)]
	internal readonly struct TTF_GPUAtlasDrawSequence
	{
		public unsafe readonly GpuTexture.SDL_GPUTexture* AtlasTexture;
		public unsafe readonly Point<float>* Xy;
		public unsafe readonly Point<float>* Uv;
		public readonly int NumVertices;
		public unsafe readonly int* Indices;
		public readonly int NumIndices;
		public readonly ImageType ImageType;

		public unsafe readonly TTF_GPUAtlasDrawSequence* Next;
	}
}
