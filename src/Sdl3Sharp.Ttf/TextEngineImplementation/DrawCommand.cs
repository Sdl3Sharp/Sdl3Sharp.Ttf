namespace Sdl3Sharp.Ttf.TextEngineImplementation;

/// <summary>
/// Represents the type of a <see cref="DrawOperation"/>, serving as a discriminator to distinguish the different kinds of <see cref="DrawOperation"/>s
/// </summary>
public enum DrawCommand
{
	/// <summary>No draw operation</summary>
	/// <remarks>
	/// <para>
	/// Associated draw operation type: none (will be represented as the "base" <see cref="DrawOperation"/> type)
	/// </para>
	/// </remarks>
	NoOp,

	/// <summary>Fill draw operation</summary>
	/// <remarks>
	/// <para>
	/// Associated draw operation type: <see cref="FillOperation"/>
	/// </para>
	/// </remarks>
	Fill,

	/// <summary>Copy draw operation</summary>
	/// <remarks>
	/// <para>
	/// Associated draw operation type: <see cref="CopyOperation"/>
	/// </para>
	/// </remarks>
	Copy
}
