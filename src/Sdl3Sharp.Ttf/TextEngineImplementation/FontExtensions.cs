using Sdl3Sharp.Video;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

/// <summary>
/// Provides extension methods for <see cref="Font"/> when used in custom text engine implementations
/// </summary>
public static partial class FontExtensions
{
	extension(Font font)
	{
		/// <summary>
		/// Tries to get the pixel image for a character index
		/// </summary>
		/// <param name="glyphIndex">The index of the glyph in the font. This might not be the same as the Unicode code point of the character.</param>
		/// <param name="image">The pixel image of the glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
		/// <param name="imageType">The type of data contained in the glyph <paramref name="image"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see cref="ImageType.Invalid"/></c></param>
		/// <returns><c><see langword="true"/></c>, if the glyph image was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// This method is only really useful if used in custom text engine implementations.
		/// </para>
		/// <para>
		/// The <paramref name="glyphIndex"/> is not necessarily the same as the Unicode code point of the character.
		/// You should call this method with the value of <see cref="CopyOperation.GlyphIndex"/> from a <see cref="CopyOperation"/> passed as the <paramref name="glyphIndex"/> parameter.
		/// </para>
		/// <para>
		/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned image when you're done using it.
		/// </para>
		/// <para>
		/// This method should be only called on the thread that created the font.
		/// </para>
		/// </remarks>
		public bool TryGetGlyphImageForIndex(uint glyphIndex, [NotNullWhen(true)] out Surface? image, out ImageType imageType)
		{
			unsafe
			{
				Unsafe.SkipInit(out ImageType imageTypeTmp);

				var imagePtr = TTF_GetGlyphImageForIndex(font is not null ? font.Pointer : null, glyphIndex, &imageTypeTmp);

				if (!Surface.TryGetOrCreate(imagePtr, out image))
				{
					// Surface.TryGetOrCreate only fails if the pointer is null

					image = null;
					imageType = ImageType.Invalid;

					return false;
				}

				imageType = imageTypeTmp;

				return true;
			}
		}
	}
}
