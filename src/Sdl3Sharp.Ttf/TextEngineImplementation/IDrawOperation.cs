namespace Sdl3Sharp.Ttf.TextEngineImplementation;

/// <summary>
/// A common interface shared by all draw operation types
/// </summary>
/// <typeparam name="TSelf">The actual type of the draw operation implementing this interface</typeparam>
public interface IDrawOperation<TSelf>
	where TSelf : struct, IDrawOperation<TSelf>, allows ref struct
{
	internal static abstract bool Accepts(DrawCommand command);

	internal static abstract TSelf FromBase(DrawOperation operation);

	/// <summary>
	/// Gets the actual <see cref="DrawCommand"/> serving as a discriminator to distinguishing the different kinds of <see cref="DrawOperation"/>s
	/// </summary>
	/// <value>
	/// The actual <see cref="DrawCommand"/> serving as a discriminator to distinguishing the different kinds of <see cref="DrawOperation"/>s
	/// </value>
	DrawCommand Command { get; }

	/// <summary>
	/// Converts a specific draw operation of type <typeparamref name="TSelf"/> to the base <see cref="DrawOperation"/> type
	/// </summary>
	/// <param name="operation">The specific draw operation of type <typeparamref name="TSelf"/> to convert to the base <see cref="DrawOperation"/> type</param>
	static abstract implicit operator DrawOperation(TSelf operation);
}
