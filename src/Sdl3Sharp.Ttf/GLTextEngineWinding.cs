#if SDL_TTF3_3_0_OR_GREATER

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents the winding order of the vertices in a <see cref="GLAtlasDrawSequence"/> returned by <see cref="Text.TryGetGLDrawData(out GLAtlasDrawSequenceEnumerable)"/>
/// </summary>
public enum GLTextEngineWinding
{
	/// <summary>Represents an invalid winding order</summary>
	Invalid = -1,

	/// <summary>Clockwise winding order</summary>
	Clockwise,

	/// <summary>Counter-clockwise winding order</summary>
	CounterClockwise
}

#endif
