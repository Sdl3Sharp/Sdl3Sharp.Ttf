using Sdl3Sharp.Internal;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Ttf.Internal.Interop;
using Sdl3Sharp.Ttf.TextEngineImplementation;
using Sdl3Sharp.Video;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CBool = Sdl3Sharp.Internal.Interop.CBool;

namespace Sdl3Sharp.Ttf;

partial class Text
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe readonly struct TTF_Text
	{
		public readonly byte* Text;
		public readonly int NumLines;

		public readonly int RefCount;

		public readonly TextData.TTF_TextData* Internal;
	}

	[FormattedConstant(SdlErrorHelper.ParameterInvalidErrorFormat, nameof(text))]
	private unsafe static partial string GetInvalidTextErrorMessageUtf16(TTF_Text* text = default);

	[FormattedConstant(SdlErrorHelper.ParameterInvalidErrorFormat, nameof(text))]
	private unsafe static partial ReadOnlySpan<byte> GetInvalidTextErrorMessageUtf8(TTF_Text* text = default);

	/// <summary>
	/// Appends UTF-8 text to a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="string">The UTF-8 text to insert</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function may cause the internal text representation to be rebuilt.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_AppendTextString">TTF_AppendTextString</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_AppendTextString(TTF_Text* text, byte* @string, nuint length);

	/// <summary>
	/// Creates a text object from UTF-8 text and a text engine
	/// </summary>
	/// <param name="engine">The text engine to use when creating the text object, may be NULL</param>
	/// <param name="font">The font to render with</param>
	/// <param name="text">The text to use, in UTF-8 encoding</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <returns>Returns a TTF_Text object or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the font and text engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateText">TTF_CreateText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_Text* TTF_CreateText(TextEngine.TTF_TextEngine* engine, Font.TTF_Font* font, byte *text, nuint length);

	/// <summary>
	/// Deletes UTF-8 text from a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="offset">The offset, in bytes, from the beginning of the string if &gt;= 0, the offset from the end of the string if &lt; 0. Note that this does not do UTF-8 validation, so you should only delete at UTF-8 sequence boundaries.</param>
	/// <param name="length">The length of text to delete, in bytes, or -1 for the remainder of the string.</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function may cause the internal text representation to be rebuilt.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_DeleteTextString">TTF_DeleteTextString</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_DeleteTextString(TTF_Text* text, int offset, int length);

	/// <summary>
	/// Destroys a text object created by a text engine
	/// </summary>
	/// <param name="text">The text to destroy</param>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_DestroyText">TTF_DestroyText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_DestroyText(TTF_Text* text);

	/// <summary>
	/// Draws text to an SDL renderer
	/// </summary>
	/// <param name="text">The text to draw</param>
	/// <param name="x">The x coordinate in pixels, positive from the left edge towards the right</param>
	/// <param name="y">The y coordinate in pixels, positive from the top edge towards the bottom</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// <c><paramref name="text"/></c> must have been created using a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> from <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateRendererTextEngine">TTF_CreateRendererTextEngine</see>(),
	/// and will draw using the renderer passed to that function.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_DrawRendererText">TTF_DrawRendererText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_DrawRendererText(TTF_Text* text, float x, float y);

	/// <summary>
	/// Draws text to an SDL surface
	/// </summary>
	/// <param name="text">The text to draw</param>
	/// <param name="x">The x coordinate in pixels, positive from the left edge towards the right</param>
	/// <param name="y">The y coordinate in pixels, positive from the top edge towards the bottom</param>
	/// <param name="surface">The surface to draw on</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// <c><paramref name="text"/></c> must have been created using a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> from <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateSurfaceTextEngine">TTF_CreateSurfaceTextEngine</see>().
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_DrawSurfaceText">TTF_DrawSurfaceText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_DrawSurfaceText(TTF_Text* text, int x, int y, Surface.SDL_Surface* surface);

#if SDL_TTF3_3_0_OR_GREATER

	/// <summary>
	/// Gets the geometry data needed for drawing the text
	/// </summary>
	/// <param name="text">The text to draw</param>
	/// <returns>Returns a NULL terminated linked list of <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_GLAtlasDrawSequence">TTF_GLAtlasDrawSequence</see> objects or NULL if the passed text is empty or in case of failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// <c><paramref name="text"/></c> must have been created using a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> from <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGLTextEngine">TTF_CreateGLTextEngine</see>().
	/// </para>
	/// <para>
	/// The positive X-axis is taken towards the right and the positive Y-axis is taken upwards for both the vertex and the texture coordinates, i.e, it follows the same convention used by the OpenGL API.
	/// If you want to use a different coordinate system you will need to transform the vertices yourself.
	/// </para>
	/// <para>
	/// If the text looks blocky use linear filtering.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGLTextDrawData">TTF_GetGLTextDrawData</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial GLAtlasDrawSequence.TTF_GLAtlasDrawSequence* TTF_GetGLTextDrawData(TTF_Text* text);

#endif

	/// <summary>
	/// Gets the geometry data needed for drawing the text
	/// </summary>
	/// <param name="text">The text to draw</param>
	/// <returns>Returns a NULL terminated linked list of <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_GPUAtlasDrawSequence">TTF_GPUAtlasDrawSequence</see> objects or NULL if the passed text is empty or in case of failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// <c><paramref name="text"/></c> must have been created using a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> from <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGPUTextEngine">TTF_CreateGPUTextEngine</see>().
	/// </para>
	/// <para>
	/// The positive X-axis is taken towards the right and the positive Y-axis is taken upwards for both the vertex and the texture coordinates, i.e, it follows the same convention used by the SDL_GPU API.
	/// If you want to use a different coordinate system you will need to transform the vertices yourself.
	/// </para>
	/// <para>
	/// If the text looks blocky use linear filtering.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGPUTextDrawData">TTF_GetGPUTextDrawData</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial GpuAtlasDrawSequence.TTF_GPUAtlasDrawSequence* TTF_GetGPUTextDrawData(TTF_Text* text);

	/// <summary>
	/// Gets the next substring in a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="substring">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_SubString">TTF_SubString</see> to query</param>
	/// <param name="next">A pointer filled in with the next substring</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// If called at the end of the text, this will return a zero length substring with the <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_SUBSTRING_TEXT_END">TTF_SUBSTRING_TEXT_END</see> flag set.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetNextTextSubString">TTF_GetNextTextSubString</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetNextTextSubString(TTF_Text* text, SubString* substring, SubString* next);

	/// <summary>
	/// Gets the previous substring in a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="substring">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_SubString">TTF_SubString</see> to query</param>
	/// <param name="previous">A pointer filled in with the previous substring</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// If called at the start of the text, this will return a zero length substring with the <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_SUBSTRING_TEXT_START">TTF_SUBSTRING_TEXT_START</see> flag set.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetPreviousTextSubString">TTF_GetPreviousTextSubString</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetPreviousTextSubString(TTF_Text* text, SubString* substring, SubString* previous);

	/// <summary>
	/// Gets the color of a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="r">A pointer filled in with the red color value in the range of 0-255, may be NULL</param>
	/// <param name="g">A pointer filled in with the green color value in the range of 0-255, may be NULL</param>
	/// <param name="b">A pointer filled in with the blue color value in the range of 0-255, may be NULL</param>
	/// <param name="a">A pointer filled in with the alpha value in the range of 0-255, may be NULL</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextColor">TTF_GetTextColor</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetTextColor(TTF_Text* text, byte* r, byte* g, byte* b, byte* a);

	/// <summary>
	/// Gets the color of a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="r">A pointer filled in with the red color value, normally in the range of 0-1, may be NULL</param>
	/// <param name="g">A pointer filled in with the green color value, normally in the range of 0-1, may be NULL</param>
	/// <param name="b">A pointer filled in with the blue color value, normally in the range of 0-1, may be NULL</param>
	/// <param name="a">A pointer filled in with the alpha value, normally in the range of 0-1, may be NULL</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextColorFloat">TTF_GetTextColorFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetTextColorFloat(TTF_Text* text, float* r, float* g, float* b, float* a);

	/// <summary>
	/// Gets the direction to be used for text shaping a text object
	/// </summary>
	/// <param name="text">The text to query</param>
	/// <returns>Returns the direction to be used for text shaping</returns>
	/// <remarks>
	/// <para>
	/// This defaults to the direction of the font used by the text object.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextDirection">TTF_GetTextDirection</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Direction TTF_GetTextDirection(TTF_Text* text);

	/// <summary>
	/// Gets the text engine used by a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <returns>Returns the <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> used by the text on success or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextEngine">TTF_GetTextEngine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TextEngine.TTF_TextEngine* TTF_GetTextEngine(TTF_Text* text);

	/// <summary>
	/// Gets the font used by a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <returns>Returns the <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Font">TTF_Font</see> used by the text on success or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextFont">TTF_GetTextFont</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Font.TTF_Font* TTF_GetTextFont(TTF_Text* text);

	/// <summary>
	/// Gets the position of a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="x">A pointer filled in with the x offset of the upper left corner of this text in pixels, may be NULL</param>
	/// <param name="y">A pointer filled in with the y offset of the upper left corner of this text in pixels, may be NULL</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextPosition">TTF_GetTextPosition</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetTextPosition(TTF_Text* text, int* x, int* y);

	/// <summary>
	/// Gets the properties associated with a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <returns>Returns a valid property ID on success or 0 on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextProperties">TTF_GetTextProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint TTF_GetTextProperties(TTF_Text* text);

	/// <summary>
	/// Gets the script used for text shaping a text object
	/// </summary>
	/// <param name="text">The text to query</param>
	/// <returns>Returns an <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924 code</see> or 0 if a script hasn't been set on either the text object or the font</returns>
	/// <remarks>
	/// <para>
	/// This defaults to the script of the font used by the text object.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextScript">TTF_GetTextScript</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Script TTF_GetTextScript(TTF_Text* text);

	/// <summary>
	/// Gets the size of a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="w">A pointer filled in with the width of the text, in pixels, may be NULL</param>
	/// <param name="h">A pointer filled in with the height of the text, in pixels, may be NULL</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// The size of the text may change when the font or font style and size change.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextSize">TTF_GetTextSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetTextSize(TTF_Text* text, int* w, int* h);

	/// <summary>
	/// Gets the substring of a text object that surrounds a text offset
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="offset">A byte offset into the text string</param>
	/// <param name="substring">A pointer filled in with the substring containing the offset</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// If <c><paramref name="offset"/></c> is less than 0, this will return a zero length substring at the beginning of the text with the <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_SUBSTRING_TEXT_START">TTF_SUBSTRING_TEXT_START</see> flag set.
	/// If <c><paramref name="offset"/></c> is greater than or equal to the length of the text string, this will return a zero length substring at the end of the text with the <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_SUBSTRING_TEXT_END">TTF_SUBSTRING_TEXT_END</see> flag set.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextSubString">TTF_GetTextSubString</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetTextSubString(TTF_Text* text, int offset, SubString* substring);

	/// <summary>
	/// Gets the substring of a text object that contains the given line
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="line">A zero-based line index, in the range [0 .. text->num_lines-1]</param>
	/// <param name="substring">A pointer filled in with the substring containing the offset</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// If <c><paramref name="line"/></c> is less than 0, this will return a zero length substring at the beginning of the text with the <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_SUBSTRING_TEXT_START">TTF_SUBSTRING_TEXT_START</see> flag set.
	/// If <c><paramref name="line"/></c> is greater than or equal to text->num_lines this will return a zero length substring at the end of the text with the <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_SUBSTRING_TEXT_END">TTF_SUBSTRING_TEXT_END</see> flag set.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextSubStringForLine">TTF_GetTextSubStringForLine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetTextSubStringForLine(TTF_Text* text, int line, SubString* substring);

	/// <summary>
	/// Gets the portion of a text string that is closest to a point
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="x">The x coordinate relative to the left side of the text, may be outside the bounds of the text area</param>
	/// <param name="y">The y coordinate relative to the top of the text, may be outside the bounds of the text area</param>
	/// <param name="substring">A pointer filled in with the closest substring of text to the given point</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This will return the closest substring of text to the given point.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextSubStringForPoint">TTF_GetTextSubStringForPoint</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetTextSubStringForPoint(TTF_Text* text, int x, int y, SubString* substring);

	/// <summary>
	/// Gets the substrings of a text object that contain a range of text
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="offset">A byte offset into the text string</param>
	/// <param name="length">The length of the range being queried, in bytes, or -1 for the remainder of the string</param>
	/// <param name="count">A pointer filled in with the number of substrings returned, may be NULL</param>
	/// <returns>
	/// Returns a NULL terminated array of substring pointers or NULL on failure; call SDL_GetError() for more information.
	/// This is a single allocation that should be freed with SDL_free() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextSubStringsForRange">TTF_GetTextSubStringsForRange</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SubString** TTF_GetTextSubStringsForRange(TTF_Text* text, int offset, int length, int* count);

	/// <summary>
	/// Gets whether wrapping is enabled on a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <param name="wrap_width">A pointer filled in with the maximum width in pixels or 0 if the text is being wrapped on newline characters</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetTextWrapWidth">TTF_GetTextWrapWidth</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_GetTextWrapWidth(TTF_Text* text, int* wrap_width);

	/// <summary>
	/// Insert UTF-8 text into a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="offset">The offset, in bytes, from the beginning of the string if &gt;= 0, the offset from the end of the string if &lt; 0. Note that this does not do UTF-8 validation, so you should only insert at UTF-8 sequence boundaries.</param>
	/// <param name="string">The UTF-8 text to insert</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function may cause the internal text representation to be rebuilt.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_InsertTextString">TTF_InsertTextString</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_InsertTextString(TTF_Text* text, int offset, byte* @string, nuint length);

	/// <summary>
	/// Sets the color of a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="r">The red color value in the range of 0-255</param>
	/// <param name="g">The green color value in the range of 0-255</param>
	/// <param name="b">The blue color value in the range of 0-255</param>
	/// <param name="a">The alpha value in the range of 0-255</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// The default text color is white (255, 255, 255, 255).
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetTextColor">TTF_SetTextColor</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetTextColor(TTF_Text* text, byte r, byte g, byte b, byte a);

	/// <summary>
	/// Sets the color of a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="r">The red color value, normally in the range of 0-1</param>
	/// <param name="g">The green color value, normally in the range of 0-1</param>
	/// <param name="b">The blue color value, normally in the range of 0-1</param>
	/// <param name="a">The alpha value, normally in the range of 0-1</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// The default text color is white (1.0f, 1.0f, 1.0f, 1.0f).
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetTextColorFloat">TTF_SetTextColorFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetTextColorFloat(TTF_Text* text, float r, float g, float b, float a);

	/// <summary>
	/// Sets the direction to be used for text shaping a text object
	/// </summary>
	/// <param name="text">The text to modify</param>
	/// <param name="direction">The new direction for text to flow</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function only supports left-to-right text shaping if SDL_ttf was not built with HarfBuzz support.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetTextDirection">TTF_SetTextDirection</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetTextDirection(TTF_Text* text, Direction direction);

	/// <summary>
	/// Sets the text engine used by a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="engine">The text engine to use for drawing</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function may cause the internal text representation to be rebuilt.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetTextEngine">TTF_SetTextEngine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetTextEngine(TTF_Text* text, TextEngine.TTF_TextEngine* engine);

	/// <summary>
	/// Sets the font used by a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="font">Tthe font to use, may be NULL</param>
	/// <returns>Returns false if the text pointer is null; otherwise, true. call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// When a text object has a font, any changes to the font will automatically regenerate the text.
	/// If you set the font to NULL, the text will continue to render but changes to the font will no longer affect the text.
	/// </para>
	/// <para>
	/// This function may cause the internal text representation to be rebuilt.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetTextFont">TTF_SetTextFont</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetTextFont(TTF_Text* text, Font.TTF_Font* font);

	/// <summary>
	/// Sets the position of a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="x">The x offset of the upper left corner of this text in pixels</param>
	/// <param name="y">The y offset of the upper left corner of this text in pixels</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This can be used to position multiple text objects within a single wrapping text area.
	/// </para>
	/// <para>
	/// This function may cause the internal text representation to be rebuilt.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetTextPosition">TTF_SetTextPosition</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetTextPosition(TTF_Text* text, int x, int y);

	/// <summary>
	/// Sets the script to be used for text shaping a text object
	/// </summary>
	/// <param name="text">The text to modify</param>
	/// <param name="script">An <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924 code</see></param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This returns false if SDL_ttf isn't built with HarfBuzz support.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetTextScript">TTF_SetTextScript</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetTextScript(TTF_Text* text, Script script);

	/// <summary>
	/// Sets the UTF-8 text used by a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="string">The UTF-8 text to use, may be NULL</param>
	/// <param name="length">The length of the text, in bytes, or 0 for null terminated text</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function may cause the internal text representation to be rebuilt.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetTextString">TTF_SetTextString</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetTextString(TTF_Text* text, byte* @string, nuint length);

	/// <summary>
	/// Sets whether whitespace should be visible when wrapping a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="visible">True to show whitespace when wrapping text, false to hide it</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// If the whitespace is visible, it will take up space for purposes of alignment and wrapping.
	/// This is good for editing, but looks better when centered or aligned if whitespace around line wrapping is hidden.
	/// This defaults false.
	/// </para>
	/// <para>
	/// This function may cause the internal text representation to be rebuilt.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetTextWrapWhitespaceVisible">TTF_SetTextWrapWhitespaceVisible</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetTextWrapWhitespaceVisible(TTF_Text* text, CBool visible);

	/// <summary>
	/// Sets whether wrapping is enabled on a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to modify</param>
	/// <param name="wrap_width">The maximum width in pixels, 0 to wrap on newline characters</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function may cause the internal text representation to be rebuilt.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetTextWrapWidth">TTF_SetTextWrapWidth</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_SetTextWrapWidth(TTF_Text* text, int wrap_width);

	/// <summary>
	/// Return whether whitespace is shown when wrapping a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to query</param>
	/// <returns>Returns true if whitespace is shown when wrapping text, or false otherwise</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextWrapWhitespaceVisible">TTF_TextWrapWhitespaceVisible</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_TextWrapWhitespaceVisible(TTF_Text* text);

	/// <summary>
	/// Updates the layout of a text object
	/// </summary>
	/// <param name="text">The <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text">TTF_Text</see> to update</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is automatically done when the layout is requested or the text is rendered, but you can call this if you need more control over the timing of when the layout and text engine representation are updated.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the text.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_UpdateText">TTF_UpdateText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool TTF_UpdateText(TTF_Text* text);
}
