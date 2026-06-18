#if SDL_TTF3_3_0_OR_GREATER

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents an enumerable collection of <see cref="GLAtlasDrawSequence"/>s that in turn represent the GPU geometry data needed to render a <see cref="Text"/> using a <see cref="GLTextEngine"/>
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly ref partial struct GLAtlasDrawSequenceEnumerable
{
	private unsafe readonly GLAtlasDrawSequence.TTF_GLAtlasDrawSequence* mSequences;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal unsafe GLAtlasDrawSequenceEnumerable(GLAtlasDrawSequence.TTF_GLAtlasDrawSequence* sequences) => mSequences = sequences;

	/// <summary>
	/// Enumerates the <see cref="GLAtlasDrawSequence"/>s in this <see cref="GLAtlasDrawSequenceEnumerable"/>
	/// </summary>
	/// <returns>An <see cref="Enumerator"/> that can be used to enumerate the <see cref="GLAtlasDrawSequence"/>s in this <see cref="GLAtlasDrawSequenceEnumerable"/></returns>
	public readonly Enumerator GetEnumerator() => new(this);
}

#endif
