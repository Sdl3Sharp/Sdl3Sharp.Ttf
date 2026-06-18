using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents an enumerable collection of <see cref="GpuAtlasDrawSequence"/>s that in turn represent the GPU geometry data needed to render a <see cref="Text"/> using a <see cref="GpuTextEngine"/>
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly ref partial struct GpuAtlasDrawSequenceEnumerable
{
	private unsafe readonly GpuAtlasDrawSequence.TTF_GPUAtlasDrawSequence* mSequences;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal unsafe GpuAtlasDrawSequenceEnumerable(GpuAtlasDrawSequence.TTF_GPUAtlasDrawSequence* sequences) => mSequences = sequences;

	/// <summary>
	/// Enumerates the <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawSequenceEnumerable"/>
	/// </summary>
	/// <returns>An <see cref="Enumerator"/> that can be used to enumerate the <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawSequenceEnumerable"/></returns>
	public readonly Enumerator GetEnumerator() => new(this);
}
