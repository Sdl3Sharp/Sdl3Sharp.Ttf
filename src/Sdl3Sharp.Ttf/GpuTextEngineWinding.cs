namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents the winding order of the vertices in a <see cref="GpuAtlasDrawSequence"/> returned by <see cref="Text.TryGetGpuDrawData(out GpuAtlasDrawSequenceEnumerable)"/>
/// </summary>
public enum GpuTextEngineWinding
{
	/// <summary>Represents an invalid winding order</summary>
	Invalid = -1,

	/// <summary>Clockwise winding order</summary>
	Clockwise,

	/// <summary>Counter-clockwise winding order</summary>
	CounterClockwise,
}
