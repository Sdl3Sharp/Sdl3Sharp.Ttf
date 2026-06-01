using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Gpu;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

[StructLayout(LayoutKind.Sequential)]
public readonly struct GpuAtlasDrawSequence
{
	private unsafe readonly GpuTexture.SDL_GPUTexture* mAtlasTexture;
	private unsafe readonly Point<float>* mXy;
	private unsafe readonly Point<float>* mUv;
	private readonly int mNumVertices;
	private unsafe readonly int* mIndices;
	private readonly int mNumIndices;
	private readonly ImageType mImageType;
}
