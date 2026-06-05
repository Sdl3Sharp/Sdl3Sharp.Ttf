namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents how the end of lines are rendered
/// </summary>
public enum LineCap
{
	/// <summary>The end of lines is rendered as a full stop on the last point itself</summary>
	Butt = 0,

	/// <summary>The end of lines is rendered as a half-circle around the last point</summary>
	Round,

	/// <summary>The end of lines is rendered as a square around the last point</summary>
	Square
}
