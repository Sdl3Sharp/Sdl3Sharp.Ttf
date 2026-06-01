using Sdl3Sharp.IO;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Ttf.Internal.Interop;
using Sdl3Sharp.Video;
using Sdl3Sharp.Video.Coloring;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CBool = Sdl3Sharp.Internal.Interop.CBool;

namespace Sdl3Sharp.Ttf;

partial class Font
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct TTF_Font;

	/// <summary>
	/// Adds a fallback font
	/// </summary>
	/// <param name="font">The font to modify</param>
	/// <param name="fallback">The font to add as a fallback.</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// Add a font that will be used for glyphs that are not in the current font. The fallback font should have the same size and style as the current font.
	/// </para>
	/// <para>
	/// If there are multiple fallback fonts, they are used in the order added.
	/// </para>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created both fonts.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_AddFallbackFont">TTF_AddFallbackFont</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_AddFallbackFont(TTF_Font* font, TTF_Font* fallback);

	/// <summary>
	/// Removes all fallback fonts
	/// </summary>
	/// <param name="font">The font to modify</param>
	/// <remarks>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created both fonts.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_ClearFallbackFonts">TTF_ClearFallbackFonts</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_ClearFallbackFonts(TTF_Font* font);

	/// <summary>
	/// Disposes of a previously-created font
	/// </summary>
	/// <param name="font">The font to dispose of</param>
	/// <remarks>
	/// <para>
	/// Call this when done with a font. This function will free any resources associated with it.
	/// It is safe to call this function on NULL, for example on the result of a failed call to <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_OpenFont">TTF_OpenFont()</see>.
	/// </para>
	/// <para>
	/// The font is not valid after being passed to this function.
	/// String pointers from functions that return information on this font,
	/// such as <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontFamilyName">TTF_GetFontFamilyName</see>() and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontStyleName">TTF_GetFontStyleName</see>(),
	/// are no longer valid after this call, as well.
	/// </para>
	/// <para>
	/// This function should not be called while any other thread is using the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_CloseFont">TTF_CloseFont</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_CloseFont(TTF_Font* font);

	/// <summary>
	/// Creates a copy of an existing font
	/// </summary>
	/// <param name="existing_font">The font to copy</param>
	/// <returns>Returns a valid <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CopyFont">TTF_Font</see>, or NULL on failure; call SDL_GetError() for more information.</returns>
	/// <remarks>
	/// <para>
	/// The copy will be distinct from the original, but will share the font file and have the same size and style as the original.
	/// </para>
	/// <para>
	/// When done with the returned <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Font">TTF_Font</see>, use <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CloseFont">TTF_CloseFont</see>() to dispose of it.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the original font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_CopyFont">TTF_CopyFont</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_Font* TTF_CopyFont(TTF_Font* existing_font);

	/// <summary>
	/// Checks whether a glyph is provided by the font for a UNICODE codepoint
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <param name="glyph">The codepoint to check</param>
	/// <returns>Returns true if font provides a glyph for this character, false if not</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_FontHasGlyph">TTF_FontHasGlyph</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_FontHasGlyph(TTF_Font* font, uint glyph);

	/// <summary>
	/// Queries whether a font is fixed-width
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns true if the font is fixed-width, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// A "fixed-width" font means all glyphs are the same width across; a lowercase 'i' will be the same size across as a capital 'W', for example.
	/// This is common for terminals and text editors, and other apps that treat text as a grid.
	/// Most other things (WYSIWYG word processors, web pages, etc) are more likely to not be fixed-width in most cases.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_FontIsFixedWidth">TTF_FontIsFixedWidth</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_FontIsFixedWidth(TTF_Font* font);

	/// <summary>
	/// Queries whether a font is scalable or not
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns true if the font is scalable, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// Scalability lets us distinguish between outline and bitmap fonts.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_FontIsScalable">TTF_FontIsScalable</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_FontIsScalable(TTF_Font* font);

	/// <summary>
	/// Queries the offset from the baseline to the top of a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the font's ascent</returns>
	/// <remarks>
	/// <para>
	/// This is a positive value, relative to the baseline.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontAscent">TTF_GetFontAscent</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int TTF_GetFontAscent(TTF_Font* font);

	/// <summary>
	/// Gets the additional character spacing in pixels to be applied between any two rendered characters
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the character spacing in pixels</returns>
	/// <remarks>
	/// <para>
	/// This defaults to 0 if it hasn't been set.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontCharSpacing">TTF_GetFontCharSpacing</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int TTF_GetFontCharSpacing(TTF_Font* font);

	/// <summary>
	/// Queries the offset from the baseline to the bottom of a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the font's descent</returns>
	/// <remarks>
	/// <para>
	/// This is a negative value, relative to the baseline.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontDescent">TTF_GetFontDescent</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int TTF_GetFontDescent(TTF_Font* font);

	/// <summary>
	/// Gets the direction to be used for text shaping by a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the direction to be used for text shaping</returns>
	/// <remarks>
	/// <para>
	/// This defaults to <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_DIRECTION_INVALID">TTF_DIRECTION_INVALID</see> if it hasn't been set.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontDirection">TTF_GetFontDirection</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Direction TTF_GetFontDirection(TTF_Font* font);

	/// <summary>
	/// Gets font target resolutions, in dots per inch
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <param name="hdpi">A pointer filled in with the target horizontal DPI</param>
	/// <param name="vdpi">A pointer filled in with the target vertical DPI</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontDPI">TTF_GetFontDPI</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetFontDPI(TTF_Font* font, int* hdpi, int* vdpi);

	/// <summary>
	/// Queries a font's family name
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the font's family name</returns>
	/// <remarks>
	/// <para>
	/// This string is dictated by the contents of the font file.
	/// </para>
	/// <para>
	/// Note that the returned string is to internal storage, and should not be modified or free'd by the caller.
	/// The string becomes invalid, with the rest of the font, when font is handed to <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CloseFont">TTF_CloseFont</see>().
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontFamilyName">TTF_GetFontFamilyName</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* TTF_GetFontFamilyName(TTF_Font* font);

	/// <summary>
	/// Gets the font generation
	/// </summary>
	/// <param name="font"></param>
	/// <returns>Returns the font generation or 0 on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// The generation is incremented each time font properties change that require rebuilding glyphs, such as style, size, etc.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontGeneration">TTF_GetFontGeneration</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint TTF_GetFontGeneration(TTF_Font* font);

	/// <summary>
	/// Queries the total height of a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the font's height</returns>
	/// <remarks>
	/// <para>
	/// This is usually equal to point size.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontHeight">TTF_GetFontHeight</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int TTF_GetFontHeight(TTF_Font* font);

	/// <summary>
	/// Queries a font's current FreeType hinter setting
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the font's current hinter value, or <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_INVALID">TTF_HINTING_INVALID</see> if the font is invalid</returns>
	/// <remarks>
	/// <para>
	/// The hinter setting is a single value:
	/// <list type="bullet">
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_NORMAL"><c>TTF_HINTING_NORMAL</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_LIGHT"><c>TTF_HINTING_LIGHT</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_MONO"><c>TTF_HINTING_MONO</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_NONE"><c>TTF_HINTING_NONE</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_LIGHT_SUBPIXEL"><c>TTF_HINTING_LIGHT_SUBPIXEL</c></see> (available in SDL_ttf 3.0.0 and later)</description></item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontHinting">TTF_GetFontHinting</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Hinting TTF_GetFontHinting(TTF_Font* font);

	/// <summary>
	/// Queries whether or not kerning is enabled for a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns true if kerning is enabled, false otherwise</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontKerning">TTF_GetFontKerning</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetFontKerning(TTF_Font* font);

	/// <summary>
	/// Queries the spacing between lines of text for a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the font's recommended spacing</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontLineSkip">TTF_GetFontLineSkip</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int TTF_GetFontLineSkip(TTF_Font* font);

	/// <summary>
	/// Queries a font's current outline
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the font's current outline value</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontOutline">TTF_GetFontOutline</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int TTF_GetFontOutline(TTF_Font* font);

	/// <summary>
	/// Gets the properties associated with a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns a valid property ID on success or 0 on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// The following read-write properties are provided by SDL:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_OUTLINE_LINE_CAP_NUMBER"><c>TTF_PROP_FONT_OUTLINE_LINE_CAP_NUMBER</c></see></term>
	///			<description>The FT_Stroker_LineCap value used when setting the font outline, defaults to <c>FT_STROKER_LINECAP_ROUND</c></description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_OUTLINE_LINE_JOIN_NUMBER"><c>TTF_PROP_FONT_OUTLINE_LINE_JOIN_NUMBER</c></see></term>
	///			<description>The FT_Stroker_LineJoin value used when setting the font outline, defaults to <c>FT_STROKER_LINEJOIN_ROUND</c></description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_OUTLINE_MITER_LIMIT_NUMBER"><c>TTF_PROP_FONT_OUTLINE_MITER_LIMIT_NUMBER</c></see></term>
	///			<description>The FT_Fixed miter limit used when setting the font outline, defaults to 0</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontProperties">TTF_GetFontProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint TTF_GetFontProperties(TTF_Font* font);

	/// <summary>
	/// Gets the script used for text shaping a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns an <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924 code</see> or 0 if a script hasn't been set</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontScript">TTF_GetFontScript</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Script TTF_GetFontScript(TTF_Font* font);

	/// <summary>
	/// Queries whether Signed Distance Field rendering is enabled for a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns true if enabled, false otherwise</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontSDF">TTF_GetFontSDF</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetFontSDF(TTF_Font* font);

	/// <summary>
	/// Gets the size of a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the size of the font, or 0.0f on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontSize">TTF_GetFontSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial float TTF_GetFontSize(TTF_Font* font);

	/// <summary>
	/// Queries a font's current style
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the current font style, as a set of bit flags</returns>
	/// <remarks>
	/// <para>
	/// The font styles are a set of bit flags, OR'd together:
	/// <list type="bullet">
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_STYLE_NORMAL"><c>TTF_STYLE_NORMAL</c></see> (is zero)</description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_STYLE_BOLD"><c>TTF_STYLE_BOLD</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_STYLE_ITALIC"><c>TTF_STYLE_ITALIC</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_STYLE_UNDERLINE"><c>TTF_STYLE_UNDERLINE</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_STYLE_STRIKETHROUGH"><c>TTF_STYLE_STRIKETHROUGH</c></see></description></item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontStyle">TTF_GetFontStyle</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial FontStyles TTF_GetFontStyle(TTF_Font* font);

	/// <summary>
	/// Queries a font's style name
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the font's style name</returns>
	/// <remarks>
	/// <para>
	/// This string is dictated by the contents of the font file.
	/// </para>
	/// <para>
	/// Note that the returned string is to internal storage, and should not be modified or free'd by the caller.
	/// The string becomes invalid, with the rest of the font, when font is handed to <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CloseFont">TTF_CloseFont</see>().
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontStyleName">TTF_GetFontStyleName</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* TTF_GetFontStyleName(TTF_Font* font);

	/// <summary>
	/// Queries a font's current wrap alignment option
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the font's current wrap alignment option</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFontWrapAlignment">TTF_GetFontWrapAlignment</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial HorizontalAlignment TTF_GetFontWrapAlignment(TTF_Font* font);

	/// <summary>
	/// Gets the pixel image for a UNICODE codepoint
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <param name="ch">The codepoint to check</param>
	/// <param name="image_type">A pointer filled in with the glyph image type, may be NULL</param>
	/// <returns>Returns an SDL_Surface containing the glyph, or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGlyphImage">TTF_GetGlyphImage</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_GetGlyphImage(TTF_Font* font, uint ch, ImageType* image_type);

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
	internal unsafe static partial Surface.SDL_Surface* TTF_GetGlyphImageForIndex(TTF_Font* font, uint glyph_index, ImageType* image_type);

	/// <summary>
	/// Queries the kerning size between the glyphs of two UNICODE codepoints
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <param name="previous_ch">The previous codepoint</param>
	/// <param name="ch">The current codepoint</param>
	/// <param name="kerning">A pointer filled in with the kerning size between the two glyphs, in pixels, may be NULL.</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGlyphKerning">TTF_GetGlyphKerning</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetGlyphKerning(TTF_Font* font, uint previous_ch, uint ch, int* kerning);

	/// <summary>
	/// Queries the metrics (dimensions) of a font's glyph for a UNICODE codepoint
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <param name="ch">The codepoint to check</param>
	/// <param name="minx">A pointer filled in with the minimum x coordinate of the glyph from the left edge of its bounding box. This value may be negative.</param>
	/// <param name="maxx">A pointer filled in with the maximum x coordinate of the glyph from the left edge of its bounding box.</param>
	/// <param name="miny">A pointer filled in with the minimum y coordinate of the glyph from the bottom edge of its bounding box. This value may be negative.</param>
	/// <param name="maxy">A pointer filled in with the maximum y coordinate of the glyph from the bottom edge of its bounding box.</param>
	/// <param name="advance">A pointer filled in with the distance to the next glyph from the left edge of this glyph's bounding box.</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// To understand what these metrics mean, here is a useful link: <see href="https://freetype.sourceforge.net/freetype2/docs/tutorial/step2.html"/>
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGlyphMetrics">TTF_GetGlyphMetrics</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetGlyphMetrics(TTF_Font* font, uint ch, int* minx, int* maxx, int* miny, int* maxy, int* advance);

	/// <summary>
	/// Gets the script used by a 32-bit codepoint
	/// </summary>
	/// <param name="ch">The character code to check.</param>
	/// <returns>Returns an <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924 code</see> on success, or 0 on failure; call SDL_GetError() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGlyphScript">TTF_GetGlyphScript</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial Script TTF_GetGlyphScript(uint ch);

	/// <summary>
	/// Queries the number of faces of a font
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <returns>Returns the number of FreeType font faces</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetNumFontFaces">TTF_GetNumFontFaces</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int TTF_GetNumFontFaces(TTF_Font* font);

	/// <summary>
	/// Calculates the dimensions of a rendered string of UTF-8 text
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <param name="text">Text to calculate, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="w">Will be filled with width, in pixels, on return</param>
	/// <param name="h">Will be filled with height, in pixels, on return</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This will report the width and height, in pixels, of the space that the specified string will take to fully render.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetStringSize">TTF_GetStringSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetStringSize(TTF_Font* font, byte* text, nuint length, int* w, int* h);

	/// <summary>
	/// Calculates the dimensions of a rendered string of UTF-8 text
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <param name="text">Text to calculate, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="wrap_width">The maximum width or 0 to wrap on newline characters</param>
	/// <param name="w">Will be filled with width, in pixels, on return</param>
	/// <param name="h">Will be filled with height, in pixels, on return</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This will report the width and height, in pixels, of the space that the specified string will take to fully render.
	/// </para>
	/// <para>
	/// Text is wrapped to multiple lines on line endings and on word boundaries if it extends beyond <c><paramref name="wrap_width"/></c> in pixels.
	/// </para>
	/// <para>
	/// If wrap_width is 0, this function will only wrap on newline characters.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetStringSizeWrapped">TTF_GetStringSizeWrapped</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetStringSizeWrapped(TTF_Font* font, byte* text, nuint length, int wrap_width, int* w, int* h);

	/// <summary>
	/// Calculate how much of a UTF-8 string will fit in a given width
	/// </summary>
	/// <param name="font">The font to query</param>
	/// <param name="text">Text to calculate, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="max_width">Maximum width, in pixels, available for the string, or 0 for unbounded width</param>
	/// <param name="measured_witdh">A pointer filled in with the width, in pixels, of the string that will fit, may be NULL</param>
	/// <param name="measured_length">A pointer filled in with the length, in bytes, of the string that will fit, may be NULL</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This reports the number of characters that can be rendered before reaching <c><paramref name="max_width"/></c>.
	/// </para>
	/// <para>
	/// This does not need to render the string to do this calculation.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_MeasureString">TTF_MeasureString</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_MeasureString(TTF_Font* font, byte* text, nuint length, int max_width, int* measured_witdh, nuint* measured_length);

	/// <summary>
	/// Creates a font from a file, using a specified point size
	/// </summary>
	/// <param name="file">Path to font file</param>
	/// <param name="ptsize">Point size to use for the newly-opened font</param>
	/// <returns>Returns a valid <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Font">TTF_Font</see>, or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// Some .fon fonts will have several sizes embedded in the file, so the point size becomes the index of choosing which size.
	/// If the value is too high, the last indexed size will be the default.
	/// </para>
	/// <para>
	/// When done with the returned <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Font">TTF_Font</see>, use <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CloseFont">TTF_CloseFont</see>() to dispose of it.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_OpenFont">TTF_OpenFont</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_Font* TTF_OpenFont(byte* file, float ptsize);

	/// <summary>
	/// Creates a font from an SDL_IOStream, using a specified point size
	/// </summary>
	/// <param name="src">An SDL_IOStream to provide a font file's data</param>
	/// <param name="closeio">True to close <c><paramref name="src"/></c> when the font is closed, false to leave it open</param>
	/// <param name="ptsize"></param>
	/// <returns>Returns a valid <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Font">TTF_Font</see>, or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// Some .fon fonts will have several sizes embedded in the file, so the point size becomes the index of choosing which size.
	/// If the value is too high, the last indexed size will be the default.
	/// </para>
	/// <para>
	/// If <c><paramref name="closeio"/></c> is true, <c><paramref name="src"/></c> will be automatically closed once the font is closed. Otherwise you should keep <c><paramref name="src"/></c> open until the font is closed.
	/// </para>
	/// <para>
	/// When done with the returned <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Font">TTF_Font</see>, use <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CloseFont">TTF_CloseFont</see>() to dispose of it.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_OpenFontIO">TTF_OpenFontIO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_Font* TTF_OpenFontIO(Stream.SDL_IOStream* src, CBool closeio, float ptsize);

	/// <summary>
	/// Creates a font with the specified properties
	/// </summary>
	/// <param name="props">The properties to use</param>
	/// <returns>Returns a valid <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Font">TTF_Font</see>, or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// These are the supported properties:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_FILENAME_STRING"><c>TTF_PROP_FONT_CREATE_FILENAME_STRING</c></see></term>
	///			<description>
	///				The font file to open, if an SDL_IOStream isn't being used.
	///				This is required if <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_IOSTREAM_POINTER"><c>TTF_PROP_FONT_CREATE_IOSTREAM_POINTER</c></see>
	///				and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_EXISTING_FONT_POINTER"><c>TTF_PROP_FONT_CREATE_EXISTING_FONT_POINTER</c></see> aren't set.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_IOSTREAM_POINTER"><c>TTF_PROP_FONT_CREATE_IOSTREAM_POINTER</c></see></term>
	///			<description>
	///				An SDL_IOStream containing the font to be opened. This should not be closed until the font is closed.
	///				This is required if <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_FILENAME_STRING"><c>TTF_PROP_FONT_CREATE_FILENAME_STRING</c></see>
	///				and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_EXISTING_FONT_POINTER"><c>TTF_PROP_FONT_CREATE_EXISTING_FONT_POINTER</c></see> aren't set.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_IOSTREAM_OFFSET_NUMBER"><c>TTF_PROP_FONT_CREATE_IOSTREAM_OFFSET_NUMBER</c></see></term>
	///			<description>The offset in the iostream for the beginning of the font, defaults to 0</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_IOSTREAM_AUTOCLOSE_BOOLEAN"><c>TTF_PROP_FONT_CREATE_IOSTREAM_AUTOCLOSE_BOOLEAN</c></see></term>
	///			<description>True if closing the font should also close the associated SDL_IOStream</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_SIZE_FLOAT"><c>TTF_PROP_FONT_CREATE_SIZE_FLOAT</c></see></term>
	///			<description>
	///				The point size of the font.
	///				Some .fon fonts will have several sizes embedded in the file, so the point size becomes the index of choosing which size.
	///				If the value is too high, the last indexed size will be the default.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_FACE_NUMBER"><c>TTF_PROP_FONT_CREATE_FACE_NUMBER</c></see></term>
	///			<description>The face index of the font, if the font contains multiple font faces</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_HORIZONTAL_DPI_NUMBER"><c>TTF_PROP_FONT_CREATE_HORIZONTAL_DPI_NUMBER</c></see></term>
	///			<description>The horizontal DPI to use for font rendering, defaults to <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_VERTICAL_DPI_NUMBER"><c>TTF_PROP_FONT_CREATE_VERTICAL_DPI_NUMBER</c></see> if set, or 72 otherwise</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_VERTICAL_DPI_NUMBER"><c>TTF_PROP_FONT_CREATE_VERTICAL_DPI_NUMBER</c></see></term>
	///			<description>The vertical DPI to use for font rendering, defaults to <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_HORIZONTAL_DPI_NUMBER"><c>TTF_PROP_FONT_CREATE_HORIZONTAL_DPI_NUMBER</c></see> if set, or 72 otherwise</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_CREATE_EXISTING_FONT_POINTER"><c>TTF_PROP_FONT_CREATE_EXISTING_FONT_POINTER</c></see></term>
	///			<description>An optional <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Font">TTF_Font</see> that, if set, will be used as the font data source and the initial size and style of the new font</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_OpenFontWithProperties">TTF_OpenFontWithProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_Font* TTF_OpenFontWithProperties(uint props);

	/// <summary>
	/// Removes a fallback font
	/// </summary>
	/// <param name="font">The font to modify</param>
	/// <param name="fallback">The font to remove as a fallback</param>
	/// <remarks>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created both fonts.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RemoveFallbackFont">TTF_RemoveFallbackFont</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_RemoveFallbackFont(TTF_Font* font, TTF_Font* fallback);

	/// <summary>
	/// Renders a single UNICODE codepoint at high quality to a new ARGB surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="ch">The codepoint to render</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <returns>Returns a new 32-bit, ARGB surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 32-bit, ARGB surface, using alpha blending to dither the font with the given color.
	/// This function returns the new surface, or NULL if there was an error.
	/// </para>
	/// <para>
	/// The glyph is rendered without any padding or centering in the X direction, and aligned normally in the Y direction.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Solid">TTF_RenderGlyph_Solid</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Shaded">TTF_RenderGlyph_Shaded</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_LCD">TTF_RenderGlyph_LCD</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Blended">TTF_RenderGlyph_Blended</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderGlyph_Blended(TTF_Font* font, uint ch, Color<byte> fg);

	/// <summary>
	/// Renders a single UNICODE codepoint at LCD subpixel quality to a new ARGB surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="ch">The codepoint to render</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <param name="bg">The background color for the text</param>
	/// <returns>Returns a new 32-bit, ARGB surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 32-bit, ARGB surface, and render alpha-blended text using FreeType's LCD subpixel rendering.
	/// This function returns the new surface, or NULL if there was an error.
	/// </para>
	/// <para>
	/// The glyph is rendered without any padding or centering in the X direction, and aligned normally in the Y direction.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Solid">TTF_RenderGlyph_Solid</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Shaded">TTF_RenderGlyph_Shaded</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Blended">TTF_RenderGlyph_Blended</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_LCD">TTF_RenderGlyph_LCD</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderGlyph_LCD(TTF_Font* font, uint ch, Color<byte> fg, Color<byte> bg);

	/// <summary>
	/// Renders a single UNICODE codepoint at high quality to a new 8-bit surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="ch">The codepoint to render</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <param name="bg">The background color for the text</param>
	/// <returns>Returns a new 8-bit, palettized surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 8-bit, palettized surface.
	/// The surface's 0 pixel will be the specified background color, while other pixels have varying degrees of the foreground color.
	/// This function returns the new surface, or NULL if there was an error.
	/// </para>
	/// <para>
	/// The glyph is rendered without any padding or centering in the X direction, and aligned normally in the Y direction.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Solid">TTF_RenderGlyph_Solid</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Blended">TTF_RenderGlyph_Blended</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_LCD">TTF_RenderGlyph_LCD</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Shaded">TTF_RenderGlyph_Shaded</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderGlyph_Shaded(TTF_Font* font, uint ch, Color<byte> fg, Color<byte> bg);

	/// <summary>
	/// Renders a single 32-bit glyph at fast quality to a new 8-bit surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="ch">The codepoint to render</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <returns>Returns a new 8-bit, palettized surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 8-bit, palettized surface.
	/// The surface's 0 pixel will be the colorkey, giving a transparent background.
	/// The 1 pixel will be set to the text color.
	/// </para>
	/// <para>
	/// The glyph is rendered without any padding or centering in the X direction, and aligned normally in the Y direction.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Shaded">TTF_RenderGlyph_Shaded</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Blended">TTF_RenderGlyph_Blended</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_LCD">TTF_RenderGlyph_LCD</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderGlyph_Solid">TTF_RenderGlyph_Solid</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderGlyph_Solid(TTF_Font* font, uint ch, Color<byte> fg);

	/// <summary>
	/// Renders UTF-8 text at high quality to a new ARGB surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="text">Text to render, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <returns>Returns a new 32-bit, ARGB surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 32-bit, ARGB surface, using alpha blending to dither the font with the given color.
	/// This function returns the new surface, or NULL if there was an error.
	/// </para>
	/// <para>
	/// This will not word-wrap the string; you'll get a surface with a single line of text, as long as the string requires.
	/// You can use <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Blended_Wrapped">TTF_RenderText_Blended_Wrapped</see>() instead if you need to wrap the output to multiple lines.
	/// </para>
	/// <para>
	/// This will not wrap on newline characters.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Solid">TTF_RenderText_Solid</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Shaded">TTF_RenderText_Shaded</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_LCD">TTF_RenderText_LCD</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Blended">TTF_RenderText_Blended</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderText_Blended(TTF_Font* font, byte* text, nuint length, Color<byte> fg);

	/// <summary>
	/// Render word-wrapped UTF-8 text at high quality to a new ARGB surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="text">Text to render, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <param name="wrap_width">The maximum width of the text surface or 0 to wrap on newline characters</param>
	/// <returns>Returns a new 32-bit, ARGB surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 32-bit, ARGB surface, using alpha blending to dither the font with the given color.
	/// This function returns the new surface, or NULL if there was an error.
	/// </para>
	/// <para>
	/// Text is wrapped to multiple lines on line endings and on word boundaries if it extends beyond <c><paramref name="wrap_width"/></c> in pixels.
	/// </para>
	/// <para>
	/// If <c><paramref name="wrap_width"/></c> is 0, this function will only wrap on newline characters.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Solid_Wrapped">TTF_RenderText_Solid_Wrapped</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Shaded_Wrapped">TTF_RenderText_Shaded_Wrapped</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_LCD_Wrapped">TTF_RenderText_LCD_Wrapped</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Blended_Wrapped">TTF_RenderText_Blended_Wrapped</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderText_Blended_Wrapped(TTF_Font* font, byte* text, nuint length, Color<byte> fg, int wrap_width);

	/// <summary>
	/// Renders UTF-8 text at LCD subpixel quality to a new ARGB surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="text">Text to render, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <param name="bg">The background color for the text</param>
	/// <returns>Returns a new 32-bit, ARGB surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 32-bit, ARGB surface, and render alpha-blended text using FreeType's LCD subpixel rendering.
	/// This function returns the new surface, or NULL if there was an error.
	/// </para>
	/// <para>
	/// This will not word-wrap the string; you'll get a surface with a single line of text, as long as the string requires.
	/// You can use <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_LCD_Wrapped">TTF_RenderText_LCD_Wrapped</see>() instead if you need to wrap the output to multiple lines.
	/// </para>
	/// <para>
	/// This will not wrap on newline characters.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Solid">TTF_RenderText_Solid</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Shaded">TTF_RenderText_Shaded</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Blended">TTF_RenderText_Blended</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_LCD">TTF_RenderText_LCD</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderText_LCD(TTF_Font* font, byte* text, nuint length, Color<byte> fg, Color<byte> bg);

	/// <summary>
	/// Renders word-wrapped UTF-8 text at LCD subpixel quality to a new ARGB surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="text">Text to render, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <param name="bg">The background color for the text</param>
	/// <param name="wrap_width">The maximum width of the text surface or 0 to wrap on newline characters</param>
	/// <returns>Returns a new 32-bit, ARGB surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 32-bit, ARGB surface, and render alpha-blended text using FreeType's LCD subpixel rendering.
	/// This function returns the new surface, or NULL if there was an error.
	/// </para>
	/// <para>
	/// Text is wrapped to multiple lines on line endings and on word boundaries if it extends beyond <c><paramref name="wrap_width"/></c> in pixels.
	/// </para>
	/// <para>
	/// If <c><paramref name="wrap_width"/></c> is 0, this function will only wrap on newline characters.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Solid_Wrapped">TTF_RenderText_Solid_Wrapped</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Shaded_Wrapped">TTF_RenderText_Shaded_Wrapped</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Blended_Wrapped">TTF_RenderText_Blended_Wrapped</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_LCD_Wrapped">TTF_RenderText_LCD_Wrapped</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderText_LCD_Wrapped(TTF_Font* font, byte* text, nuint length, Color<byte> fg, Color<byte> bg, int wrap_width);

	/// <summary>
	/// Render UTF-8 text at high quality to a new 8-bit surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="text">Text to render, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <param name="bg">The background color for the text</param>
	/// <returns>Returns a new 8-bit, palettized surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 8-bit, palettized surface.
	/// The surface's 0 pixel will be the specified background color, while other pixels have varying degrees of the foreground color.
	/// This function returns the new surface, or NULL if there was an error.
	/// </para>
	/// <para>
	/// This will not word-wrap the string; you'll get a surface with a single line of text, as long as the string requires.
	/// You can use <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Shaded_Wrapped">TTF_RenderText_Shaded_Wrapped</see>() instead if you need to wrap the output to multiple lines.
	/// </para>
	/// <para>
	/// This will not wrap on newline characters.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Solid">TTF_RenderText_Solid</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Blended">TTF_RenderText_Blended</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_LCD">TTF_RenderText_LCD</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Shaded">TTF_RenderText_Shaded</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderText_Shaded(TTF_Font* font, byte* text, nuint length, Color<byte> fg, Color<byte> bg);

	/// <summary>
	/// Renders word-wrapped UTF-8 text at high quality to a new 8-bit surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="text">Text to render, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <param name="bg">The background color for the text</param>
	/// <param name="wrap_width">The maximum width of the text surface or 0 to wrap on newline characters</param>
	/// <returns>Returns a new 8-bit, palettized surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 8-bit, palettized surface.
	/// The surface's 0 pixel will be the specified background color, while other pixels have varying degrees of the foreground color.
	/// This function returns the new surface, or NULL if there was an error.
	/// </para>
	/// <para>
	/// Text is wrapped to multiple lines on line endings and on word boundaries if it extends beyond <c><paramref name="wrap_width"/></c> in pixels.
	/// </para>
	/// <para>
	/// If <c><paramref name="wrap_width"/></c> is 0, this function will only wrap on newline characters.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Solid_Wrapped">TTF_RenderText_Solid_Wrapped</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Blended_Wrapped">TTF_RenderText_Blended_Wrapped</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_LCD_Wrapped">TTF_RenderText_LCD_Wrapped</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Shaded_Wrapped">TTF_RenderText_Shaded_Wrapped</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderText_Shaded_Wrapped(TTF_Font* font, byte* text, nuint length, Color<byte> fg, Color<byte> bg, int wrap_width);

	/// <summary>
	/// Renders UTF-8 text at fast quality to a new 8-bit surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="text">Text to render, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <returns>Returns a new 8-bit, palettized surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 8-bit, palettized surface.
	/// The surface's 0 pixel will be the colorkey, giving a transparent background.
	/// The 1 pixel will be set to the text color.
	/// </para>
	/// <para>
	/// This will not word-wrap the string; you'll get a surface with a single line of text, as long as the string requires.
	/// You can use <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Solid_Wrapped">TTF_RenderText_Solid_Wrapped</see>() instead if you need to wrap the output to multiple lines.
	/// </para>
	/// <para>
	/// This will not wrap on newline characters.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Shaded">TTF_RenderText_Shaded</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Blended">TTF_RenderText_Blended</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_LCD">TTF_RenderText_LCD</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Solid">TTF_RenderText_Solid</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderText_Solid(TTF_Font* font, byte* text, nuint length, Color<byte> fg);

	/// <summary>
	/// Renders word-wrapped UTF-8 text at fast quality to a new 8-bit surface
	/// </summary>
	/// <param name="font">The font to render with</param>
	/// <param name="text">Text to render, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <param name="fg">The foreground color for the text</param>
	/// <param name="wrap_width">The maximum width of the text surface or 0 to wrap on newline characters</param>
	/// <returns>Returns a new 8-bit, palettized surface, or NULL if there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function will allocate a new 8-bit, palettized surface.
	/// The surface's 0 pixel will be the colorkey, giving a transparent background.
	/// The 1 pixel will be set to the text color.
	/// </para>
	/// <para>
	/// Text is wrapped to multiple lines on line endings and on word boundaries if it extends beyond <c><paramref name="wrap_width"/></c> in pixels.
	/// </para>
	/// <para>
	/// If <c><paramref name="wrap_width"/></c> is 0, this function will only wrap on newline characters.
	/// </para>
	/// <para>
	/// You can render at other quality levels with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Shaded_Wrapped">TTF_RenderText_Shaded_Wrapped</see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Blended_Wrapped">TTF_RenderText_Blended_Wrapped</see>, and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_LCD_Wrapped">TTF_RenderText_LCD_Wrapped</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_RenderText_Solid_Wrapped">TTF_RenderText_Solid_Wrapped</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* TTF_RenderText_Solid_Wrapped(TTF_Font* font, byte* text, nuint length, Color<byte> fg, int wrap_width);

	/// <summary>
	/// Sets additional space in pixels to be applied between any two rendered characters
	/// </summary>
	/// <param name="font">The font to modify</param>
	/// <param name="spacing">The new additional glyph spacing for the font</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// The spacing value is applied uniformly after each character, in addition to the normal glyph's advance.
	/// </para>
	/// <para>
	/// Spacing may be a negative value, in which case it will reduce the distance instead.
	/// </para>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontCharSpacing">TTF_SetFontCharSpacing</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetFontCharSpacing(TTF_Font* font, int spacing);

	/// <summary>
	/// Sets the direction to be used for text shaping by a font
	/// </summary>
	/// <param name="font">The font to modify</param>
	/// <param name="direction">The new direction for text to flow.</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function only supports left-to-right text shaping if SDL_ttf was not built with HarfBuzz support.
	/// </para>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontDirection">TTF_SetFontDirection</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetFontDirection(TTF_Font* font, Direction direction);

	/// <summary>
	/// Sets a font's current hinter setting
	/// </summary>
	/// <param name="font">The font to set a new hinter setting on</param>
	/// <param name="hinting">The new hinter setting</param>
	/// <remarks>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font, and clears already-generated glyphs, if any, from the cache.
	/// </para>
	/// <para>
	/// The hinter setting is a single value:
	/// <list type="bullet">
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_NORMAL"><c>TTF_HINTING_NORMAL</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_LIGHT"><c>TTF_HINTING_LIGHT</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_MONO"><c>TTF_HINTING_MONO</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_NONE"><c>TTF_HINTING_NONE</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_HINTING_LIGHT_SUBPIXEL"><c>TTF_HINTING_LIGHT_SUBPIXEL</c></see> (available in SDL_ttf 3.0.0 and later)</description></item>
	/// </list>
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontHinting">TTF_SetFontHinting</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_SetFontHinting(TTF_Font* font, Hinting hinting);

	/// <summary>
	/// Sets if kerning is enabled for a font
	/// </summary>
	/// <param name="font">The font to set kerning on</param>
	/// <param name="enabled">True to enable kerning, false to disable</param>
	/// <remarks>
	/// <para>
	/// Newly-opened fonts default to allowing kerning.
	/// This is generally a good policy unless you have a strong reason to disable it, as it tends to produce better rendering (with kerning disabled, some fonts might render the word kerning as something that looks like keming for example).
	/// </para>
	/// <para>
	/// This function only supports left-to-right text shaping if SDL_ttf was not built with HarfBuzz support.
	/// </para>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontKerning">TTF_SetFontKerning</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_SetFontKerning(TTF_Font* font, CBool enabled);

	/// <summary>
	/// Sets language to be used for text shaping by a font
	/// </summary>
	/// <param name="font">The font to specify a language for</param>
	/// <param name="language_bcp47">A null-terminated string containing the desired language's BCP47 code. Or null to reset the value.</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// If SDL_ttf was not built with HarfBuzz support, this function returns false.
	/// </para>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontLanguage">TTF_SetFontLanguage</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetFontLanguage(TTF_Font* font, byte* language_bcp47);

	/// <summary>
	/// Sets the spacing between lines of text for a font
	/// </summary>
	/// <param name="font">The font to modify.</param>
	/// <param name="lineskip">The new line spacing for the font</param>
	/// <remarks>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontLineSkip">TTF_SetFontLineSkip</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_SetFontLineSkip(TTF_Font* font, int lineskip);

	/// <summary>
	/// Sets a font's current outline
	/// </summary>
	/// <param name="font">The font to set a new outline on</param>
	/// <param name="outline">Positive outline value, 0 to default</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This uses the font properties <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_OUTLINE_LINE_CAP_NUMBER"><c>TTF_PROP_FONT_OUTLINE_LINE_CAP_NUMBER</c></see>, <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_OUTLINE_LINE_JOIN_NUMBER"><c>TTF_PROP_FONT_OUTLINE_LINE_JOIN_NUMBER</c></see>,
	/// and <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_FONT_OUTLINE_MITER_LIMIT_NUMBER"><c>TTF_PROP_FONT_OUTLINE_MITER_LIMIT_NUMBER</c></see> when setting the font outline.
	/// </para>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font, and clears already-generated glyphs, if any, from the cache.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontOutline">TTF_SetFontOutline</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetFontOutline(TTF_Font* font, int outline);

	/// <summary>
	/// Sets the script to be used for text shaping by a font
	/// </summary>
	/// <param name="font">The font to modify</param>
	/// <param name="script">An <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924 code</see></param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This returns false if SDL_ttf isn't built with HarfBuzz support.
	/// </para>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontScript">TTF_SetFontScript</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetFontScript(TTF_Font* font, Script script);

	/// <summary>
	/// Enables Signed Distance Field rendering for a font
	/// </summary>
	/// <param name="font">The font to set SDF support on</param>
	/// <param name="enabled">Ttrue to enable SDF, false to disable</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDF is a technique that helps fonts look sharp even when scaling and rotating, and requires special shader support for display.
	/// </para>
	/// <para>
	/// This works with Blended APIs, and generates the raw signed distance values in the alpha channel of the resulting texture.
	/// </para>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font, and clears already-generated glyphs, if any, from the cache.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontSDF">TTF_SetFontSDF</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetFontSDF(TTF_Font* font, CBool enabled);

	/// <summary>
	/// Sets a font's size dynamically
	/// </summary>
	/// <param name="font">The font to resize</param>
	/// <param name="ptsize">The new point size.</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font, and clears already-generated glyphs, if any, from the cache.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontSize">TTF_SetFontSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetFontSize(TTF_Font* font, float ptsize);

	/// <summary>
	/// Sets font size dynamically with target resolutions, in dots per inch
	/// </summary>
	/// <param name="font">The font to resize</param>
	/// <param name="ptsize">The new point size</param>
	/// <param name="hdpi">The target horizontal DPI</param>
	/// <param name="vdpi">The target vertical DPI</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font, and clears already-generated glyphs, if any, from the cache.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontSizeDPI">TTF_SetFontSizeDPI</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetFontSizeDPI(TTF_Font* font, float ptsize, int hdpi, int vdpi);

	/// <summary>
	/// Sets a font's current style
	/// </summary>
	/// <param name="font">The font to set a new style on</param>
	/// <param name="style">The new style values to set, OR'd together</param>
	/// <remarks>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font, and clears already-generated glyphs, if any, from the cache.
	/// </para>
	/// <para>
	/// The font styles are a set of bit flags, OR'd together:
	/// <list type="bullet">
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_STYLE_NORMAL"><c>TTF_STYLE_NORMAL</c></see> (is zero)</description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_STYLE_BOLD"><c>TTF_STYLE_BOLD</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_STYLE_ITALIC"><c>TTF_STYLE_ITALIC</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_STYLE_UNDERLINE"><c>TTF_STYLE_UNDERLINE</c></see></description></item>
	/// <item><description><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_STYLE_STRIKETHROUGH"><c>TTF_STYLE_STRIKETHROUGH</c></see></description></item>
	/// </list>
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontStyle">TTF_SetFontStyle</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_SetFontStyle(TTF_Font* font, FontStyles style);

	/// <summary>
	/// Sets a font's current wrap alignment option
	/// </summary>
	/// <param name="font">The font to set a new wrap alignment option on.</param>
	/// <param name="align">The new wrap alignment option</param>
	/// <remarks>
	/// <para>
	/// This updates any <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> objects using this font.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetFontWrapAlignment">TTF_SetFontWrapAlignment</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_SetFontWrapAlignment(TTF_Font* font, HorizontalAlignment align);
}
