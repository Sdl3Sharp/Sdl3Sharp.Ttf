using Sdl3Sharp.Video.Drawing;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Provides extension methods for <see cref="Text"/>
/// </summary>
public static class TextExtensions
{
	extension(Text text)
	{
		/// <summary>
		/// Tries to get the substring of this <see cref="Text"/> that is closest to the specified point
		/// </summary>
		/// <param name="point">The point to find the closest substring for</param>
		/// <param name="subString">The substring that is closest to the specified point, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="SubString"/>)</c></param>
		/// <returns><c><see langword="true"/></c>, if the substring was successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// This method tries to find the substring of this <see cref="Text"/> that is closest to the specified point, containing the point if possible.
		/// </para>
		/// <para>
		/// The horizontal <see cref="Point{T}.X">X</see> coordinate is relative to the left edge of the text and may be outside of the bounds of the text area,
		/// and the vertical <see cref="Point{T}.Y">Y</see> coordinate is relative to the top edge of the text and may also be outside of the bounds of the text area.
		/// </para>
		/// <para>
		/// This method should only be called from the thread that created the text.
		/// </para>
		/// </remarks>
		public bool TryGetSubStringForPoint(in Point<int> point, out SubString subString)
		{
			if (text is null)
			{
				subString = default;
				return false;
			}

			return text.TryGetSubStringForPoint(point.X, point.Y, out subString);
		}
	}
}
