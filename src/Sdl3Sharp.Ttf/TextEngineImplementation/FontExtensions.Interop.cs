using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Ttf.Internal.Interop;
using Sdl3Sharp.Video;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

partial class FontExtensions
{
	/// <summary>
	/// Gets the pixel image for a character index
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <param name="glyph_index">The index of the glyph to return</param>
	/// <param name="image_type">A pointer filled in with the glyph image type, may be NULL</param>
	/// <returns>Returns an SDL_Surface containing the glyph, or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is useful for text engine implementations, which can call this with the <c><paramref name="glyph_index"/></c> in a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CopyOperation">TTF_CopyOperation</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGlyphImageForIndex">TTF_GetGlyphImageForIndex</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_GetGlyphImageForIndex(Font.TTF_Font* font, uint glyph_index, ImageType* image_type);
}
