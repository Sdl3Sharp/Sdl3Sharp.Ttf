namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents how two joining lines are rendered
/// </summary>
public enum LineJoin
{
	/// <summary>
	/// Used to render rounded line joins.
	/// Circular arcs are used to join two lines smoothly.
	/// </summary>
	Round = 0,

	/// <summary>
	/// Used to render beveled line joins.
	/// The outer corner of the joined lines is filled by enclosing the triangular region of the corner with a straight line between the outer corners of each stroke.
	/// </summary>
	Bevel = 1,

	/// <summary>
	/// An alias for <see cref="Miter"/>.
	/// Please see <see cref="Miter"/> for more details.
	/// </summary>
	MiterVariable = 2,

	/// <summary>
	/// Used to render mitered line joins, with variable bevels if the miter limit is exceeded.
	/// The intersection of the strokes is clipped perpendicularly to the bisector, at a distance corresponding to the miter limit.
	/// This prevents long spikes being created.
	/// This generates a mitered line join as used in XPS.
	/// </summary>
	Miter = MiterVariable,

	/// <summary>
	/// Used to render mitered line joins, with fixed bevels if the miter limit is exceeded.
	/// The outer edges of the strokes for the two segments are extended until they meet at an angle.
	/// A <see cref="Bevel"/> join is used if the segments meet at too sharp an angle and the outer edges meet beyond a distance corresponding to the meter limit.
	/// This prevents long spikes being created.
	/// This generates a miter line join as used in PostScript and PDF.
	/// </summary>
	MiterFixed = 3,
}
