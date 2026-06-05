using Sdl3Sharp.IO;

namespace Sdl3Sharp.Ttf;

partial class Font
{
	/// <summary>
	/// Provides property names for <see cref="Font"/> <see cref="Properties">properties</see>
	/// </summary>
	public static class PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="Font(string, float, int?, int?, int?, Properties?)">property used when creating a <see cref="Font"/></see>
		/// that holds the path to the font file to load
		/// </summary>
		/// <remarks>
		/// <para>
		/// Specifying this property is required if neither <see cref="CreateIOStreamPointer"/> nor <see cref="CreateExistingFontPointer"/> is specified,
		/// and all three properties are mutually exclusive.
		/// </para>
		/// </remarks>
		public const string CreateFileNameString = "SDL_ttf.font.create.filename";

		/// <summary>
		/// The name of a <see cref="Font(string, float, int?, int?, int?, Properties?)">property used when creating a <see cref="Font"/></see>
		/// that holds a pointer to a native <c>SDL_IOStream</c> instance to read the font data from
		/// </summary>
		/// <remarks>
		/// <para>
		/// Specifying this property is required if neither <see cref="CreateFileNameString"/> nor <see cref="CreateExistingFontPointer"/> is specified,
		/// and all three properties are mutually exclusive.
		/// </para>
		/// <para>
		/// The given <c>SDL_IOStream</c> <em>must not</em> be closed until the resulting <see cref="Font"/> instance is <see cref="Dispose()">disposed</see>.
		/// </para>
		/// </remarks>
		public const string CreateIOStreamPointer = "SDL_ttf.font.create.iostream";

		/// <summary>
		/// The name of a <see cref="Font(string, float, int?, int?, int?, Properties?)">property used when creating a <see cref="Font"/></see>
		/// that holds the byte offset into the given <c>SDL_IOStream</c>/<see cref="Stream"/> instance to read the font data from
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property is only relevant if <see cref="CreateIOStreamPointer"/> is specified, and is ignored otherwise.
		/// </para>
		/// <para>
		/// If this property is not specified, the value of the associated property defaults to <c>0</c>.
		/// </para>
		/// </remarks>
		public const string CreateIOStreamOffsetNumber = "SDL_ttf.font.create.iostream.offset";

		/// <summary>
		/// The name of a <see cref="Font(string, float, int?, int?, int?, Properties?)">property used when creating a <see cref="Font"/></see>
		/// that holds a value indicating whether the given <c>SDL_IOStream</c>/<see cref="Stream"/> instance should be automatically closed when the resulting <see cref="Font"/> instance is <see cref="Dispose()">disposed</see>
		/// </summary>
		/// <remarks>
		/// <para>
		/// If the value of the associated property is <see langword="true"/>, the given <c>SDL_IOStream</c>/<see cref="Stream"/> instance will be automatically closed when the resulting <see cref="Font"/> instance is <see cref="Dispose()">disposed</see>.
		/// Otherwise, the caller is responsible for closing the <c>SDL_IOStream</c>/<see cref="Stream"/> instance themselves after that.
		/// Either way, the given <c>SDL_IOStream</c>/<see cref="Stream"/> instance <em>must</em> be kept open until the resulting <see cref="Font"/> instance is disposed.
		/// </para>
		/// </remarks>
		public const string CreateIOStreamAutoCloseBoolean = "SDL_ttf.font.create.iostream.autoclose";

		/// <summary>
		/// The name of a <see cref="Font(string, float, int?, int?, int?, Properties?)">property used when creating a <see cref="Font"/></see>
		/// that holds the point size to render the font at
		/// </summary>
		/// <remarks>
		/// <para>
		/// Some font files contain multiple sizes, so the value of the associated property will specify the index of which size to use.
		/// If the value is too high, the last possible indexed size will be used as the default.
		/// </para>
		/// </remarks>
		public const string CreateSizeFloat = "SDL_ttf.font.create.size";

		/// <summary>
		/// The name of a <see cref="Font(string, float, int?, int?, int?, Properties?)">property used when creating a <see cref="Font"/></see>
		/// that holds the index of the font face, if the font file contains multiple faces
		/// </summary>
		/// <remarks>
		/// <para>
		/// Some font files contain multiple font faces, so the value of the associated property can be optionally used to specify the index of which face to use.
		/// </para>
		/// </remarks>
		public const string CreateFaceNumber = "SDL_ttf.font.create.face";

		/// <summary>
		/// The name of a <see cref="Font(string, float, int?, int?, int?, Properties?)">property used when creating a <see cref="Font"/></see>
		/// that holds the horizontal DPI to render the font at
		/// </summary>
		/// <remarks>
		/// <para>
		/// If not specified, the value of the associated property defaults to the value of the <see cref="CreateVerticalDpiNumber"/> property, or <c>72</c> if that property is also not specified.
		/// </para>
		/// </remarks>
		public const string CreateHorizontalDpiNumber = "SDL_ttf.font.create.hdpi";

		/// <summary>
		/// The name of a <see cref="Font(string, float, int?, int?, int?, Properties?)">property used when creating a <see cref="Font"/></see>
		/// that holds the vertical DPI to render the font at
		/// </summary>
		/// <remarks>
		/// <para>
		/// If not specified, the value of the associated property defaults to the value of the <see cref="CreateHorizontalDpiNumber"/> property, or <c>72</c> if that property is also not specified.
		/// </para>
		/// </remarks>
		public const string CreateVerticalDpiNumber = "SDL_ttf.font.create.vdpi";

		/// <summary>
		/// The name of a <see cref="Font(string, float, int?, int?, int?, Properties?)">property used when creating a <see cref="Font"/></see>
		/// that holds a pointer to an existing native <c>TTF_Font</c> instance to use as the source of font data
		/// </summary>
		/// <remarks>
		/// <para>
		/// Specifying this property is required if neither <see cref="CreateFileNameString"/> nor <see cref="CreateIOStreamPointer"/> is specified,
		/// and all three properties are mutually exclusive.
		/// </para>
		/// <para>
		/// The newly created <see cref="Font"/> instance will copy the given <c>TTF_Font</c> instance's size and styles unless specified otherwise.
		/// </para>
		/// </remarks>
		public const string CreateExistingFontPointer = "SDL_ttf.font.create.existing_font";

		/// <summary>
		/// The name of a <see cref="Properties">property</see>
		/// that holds a value determining how the end of outline lines are rendered for a certain <see cref="Font"/>
		/// </summary>
		/// <remarks>
		/// <para>
		/// If not specified, the value of the associated property defaults to <see cref="LineCap.Round"/>.
		/// </para>
		/// <para>
		/// The value of the associated property can be directly cast to a <c><see href="https://freetype.org/freetype2/docs/reference/ft2-glyph_stroker.html#ft_stroker_linecap">FT_Stroker_LineCap</see></c>.
		/// </para>
		/// </remarks>
		public const string OutlineLineCapNumber = "SDL_ttf.font.outline.line_cap";

		/// <summary>
		/// The name of a <see cref="Properties">property</see>
		/// that holds a value determining how two joining outline lines are rendered for a certain <see cref="Font"/>
		/// </summary>
		/// <remarks>
		/// <para>
		/// If not specified, the value of the associated property defaults to <see cref="LineJoin.Round"/>.
		/// </para>
		/// <para>
		/// The value of the associated property can be directly cast to a <c><see href="https://freetype.org/freetype2/docs/reference/ft2-glyph_stroker.html#ft_stroker_linejoin">FT_Stroker_LineJoin</see></c>.
		/// </para>
		/// </remarks>
		public const string OutlineLineJoinNumber = "SDL_ttf.font.outline.line_join";

		/// <summary>
		/// The name of a <see cref="Properties">property</see>
		/// that holds the miter limit used for rendering outline lines for a certain <see cref="Font"/>
		/// </summary>
		/// <remarks>
		/// <para>
		/// If not specified, the value of the associated property defaults to <c>0</c>.
		/// </para>
		/// <para>
		/// The value of the associated property is only relevant if the value of the <see cref="OutlineLineJoinNumber"/> property is set to <see cref="LineJoin.Miter"/>/<see cref="LineJoin.MiterVariable"/> or <see cref="LineJoin.MiterFixed"/>.
		/// </para>
		/// <para>
		/// The value of the associated property can be directly cast to a <c><see href="https://freetype.org/freetype2/docs/reference/ft2-basic_types.html#ft_fixed">FT_Fixed</see></c>.
		/// </para>
		/// </remarks>
		public const string OutlineMiterLimitNumber = "SDL_ttf.font.outline.miter_limit";
	}
}
