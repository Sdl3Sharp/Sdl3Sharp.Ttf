using Sdl3Sharp.Internal;
using Sdl3Sharp.IO;
using Sdl3Sharp.Ttf.Internal;
using Sdl3Sharp.Video;
using Sdl3Sharp.Video.Coloring;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a font that can be used to render <see cref="Text"/>s, strings, and individual glyphs with various styles
/// </summary>
/// <remarks>
/// <para>
/// Make sure that you don't <see cref="Dispose()">dispose</see> <see cref="Font"/>s that are currently in use by, for example, <see cref="Text"/> instances.
/// Always <see cref="Text.Dispose()">dispose</see> the <see cref="Text"/> first before disposing the <see cref="Font"/> that it uses.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed partial class Font : IDisposable
{
	private interface IUnsafeConstructorDispatch;

	private static readonly ConcurrentDictionary<IntPtr, WeakReference<Font>> mKnownInstances = [];

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString();

	private unsafe TTF_Font* mFont;
	private readonly FallbackCollection mFallbacks;

	private unsafe Font(TTF_Font* font, bool register)
	{
		mFont = font;
		mFallbacks = new FallbackCollection(this);

		if (register)
		{
			if (mFont is not null)
			{
				mKnownInstances.AddOrUpdate(unchecked((IntPtr)mFont), addRef, updateRef, this);
			}

			static WeakReference<Font> addRef(IntPtr font, Font newFont) => new(newFont);

			static WeakReference<Font> updateRef(IntPtr font, WeakReference<Font> existingFontRef, Font newFont)
			{
				if (existingFontRef.TryGetTarget(out var existingFont))
				{
#pragma warning disable IDE0079
#pragma warning disable CA1816
					GC.SuppressFinalize(existingFont);
#pragma warning restore CA1816
#pragma warning restore IDE0079

					existingFont.Dispose(forget: false);
				}

				existingFontRef.SetTarget(newFont);

				return existingFontRef;
			}
		}
	}

	/// <exception cref="SdlException">The <see cref="Font"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveInlining)]
	[return: NotNull]
	private unsafe static TTF_Font* ValidateFont([NotNull] TTF_Font* font)
	{
		if (font is null)
		{
			[DoesNotReturn]
			static void failCouldNotCreateFont() => throw new SdlException($"Could not create the {nameof(Font)}");

			failCouldNotCreateFont();
		}

		return font;
	}

	private unsafe static TTF_Font* CreateWithFileName(string fileName, float size)
	{
		using var fileNameUtf8 = NativeStrings.FromUtf16ToUtf8(fileName);

		return TTF_OpenFont(fileNameUtf8.Buffer, size);
	}

	/// <inheritdoc cref="ValidateFont(TTF_Font*)"/>
	private unsafe Font(string fileName, float size, IUnsafeConstructorDispatch? _ = default)
		: this(ValidateFont(CreateWithFileName(fileName, size)), register: true)
	{ }

	/// <summary>
	/// Creates a new <see cref="Font"/> from the specified font file and point size
	/// </summary>
	/// <param name="fileName">The path to the font file</param>
	/// <param name="size">The point size of the font</param>
	/// <remarks>
	/// <para>
	/// Some font files contain multiple sizes, so the point <paramref name="size"/> will specify the index of which size to use.
	/// If the value is too high, the last possible indexed size will be used as the default.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Font(string, float, IUnsafeConstructorDispatch?)"/>
	public Font(string fileName, float size) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(fileName, size, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	private unsafe static TTF_Font* CreateWithStream(Stream stream, bool closeAfterwards, float size)
	{
		return TTF_OpenFontIO(stream is not null ? stream.Pointer : null, closeAfterwards, size);
	}

	/// <inheritdoc cref="ValidateFont(TTF_Font*)"/>
	private unsafe Font(Stream stream, bool closeAfterwards, float size, IUnsafeConstructorDispatch? _ = default) :
		this(ValidateFont(CreateWithStream(stream, closeAfterwards, size)), register: true)
	{ }

	/// <summary>
	/// Creates a new <see cref="Font"/> from the specified <see cref="Stream"/> and point size
	/// </summary>
	/// <param name="stream">The <see cref="Stream"/> containing the font data</param>
	/// <param name="closeAfterwards">A value indicating whether the given <paramref name="stream"/> should be automatically closed <em>when the resulting font is <see cref="Dispose()">disposed</see></em></param>
	/// <param name="size">The point size of the font</param>
	/// <remarks>
	/// <para>
	/// Some font files contain multiple sizes, so the point <paramref name="size"/> will specify the index of which size to use.
	/// If the value is too high, the last possible indexed size will be used as the default.
	/// </para>
	/// <para>
	/// If <paramref name="closeAfterwards"/> is <c><see langword="true"/></c>, the given <paramref name="stream"/> will be automatically closed when the resulting font is <see cref="Dispose()">disposed</see>.
	/// Otherwise, you will be responsible for disposing the given <paramref name="stream"/> yourself after that.
	/// Either way, you <em>must</em> keep the given <paramref name="stream"/> open and undisposed until the resulting font is disposed. 
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Font(Stream, bool, float, IUnsafeConstructorDispatch?)"/>
	public Font(Stream stream, bool closeAfterwards, float size) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(stream, closeAfterwards, size, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <summary>
	/// Creates a new <see cref="Font"/> from the specified <see cref="Stream"/> and point size
	/// </summary>
	/// <param name="stream">The <see cref="Stream"/> containing the font data</param>
	/// <param name="size">The point size of the font</param>
	/// <remarks>
	/// <para>
	/// Some font files contain multiple sizes, so the point <paramref name="size"/> will specify the index of which size to use.
	/// If the value is too high, the last possible indexed size will be used as the default.
	/// </para>
	/// <para>
	/// This constructor does <em>not</em> automatically close the given <paramref name="stream"/> when the resulting font is <see cref="Dispose()">disposed</see>.
	/// You <em>must</em> keep the given <paramref name="stream"/> open and undisposed until the resulting font is disposed.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Font(Stream, bool, float, IUnsafeConstructorDispatch?)"/>
	public Font(Stream stream, float size) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(stream, closeAfterwards: false, size, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	private static unsafe TTF_Font* CreateWithProperties(
		string? fileName = default, Stream? stream = default, long? streamOffset = default, bool? closeAfterwards = default,
		float? size = default, int? faceIndex = default, int? horizontalDpi = default, int? verticalDpi = default,
		Font? exisitingFont = default, Properties? properties = default
	)
	{
		Properties propertiesUsed;
		Unsafe.SkipInit(out string? fileNameBackup);
		Unsafe.SkipInit(out IntPtr? streamBackup);
		Unsafe.SkipInit(out long? streamOffsetBackup);
		Unsafe.SkipInit(out bool? closeAfterwardsBackup);
		Unsafe.SkipInit(out float? sizeBackup);
		Unsafe.SkipInit(out int? faceIndexBackup);
		Unsafe.SkipInit(out int? horizontalDpiBackup);
		Unsafe.SkipInit(out int? verticalDpiBackup);
		Unsafe.SkipInit(out IntPtr? existingFontBackup);

		if (properties is null)
		{
			propertiesUsed = [];

			if (fileName is not null)
			{
				propertiesUsed.TrySetStringValue(PropertyNames.CreateFileNameString, fileName);
			}

			if (stream is not null)
			{
				propertiesUsed.TrySetPointerValue(PropertyNames.CreateIOStreamPointer, unchecked((IntPtr)stream.Pointer));
			}

			if (streamOffset is long streamOffsetValue)
			{
				propertiesUsed.TrySetNumberValue(PropertyNames.CreateIOStreamOffsetNumber, streamOffsetValue);
			}

			if (closeAfterwards is bool closeAfterwardsValue)
			{
				propertiesUsed.TrySetBooleanValue(PropertyNames.CreateIOStreamAutoCloseBoolean, closeAfterwardsValue);
			}

			if (size is float sizeValue)
			{
				propertiesUsed.TrySetFloatValue(PropertyNames.CreateSizeFloat, sizeValue);
			}

			if (faceIndex is int faceIndexValue)
			{
				propertiesUsed.TrySetNumberValue(PropertyNames.CreateFaceNumber, faceIndexValue);
			}

			if (horizontalDpi is int horizontalDpiValue)
			{
				propertiesUsed.TrySetNumberValue(PropertyNames.CreateHorizontalDpiNumber, horizontalDpiValue);
			}

			if (verticalDpi is int verticalDpiValue)
			{
				propertiesUsed.TrySetNumberValue(PropertyNames.CreateVerticalDpiNumber, verticalDpiValue);
			}

			if (exisitingFont is not null)
			{
				propertiesUsed.TrySetPointerValue(PropertyNames.CreateExistingFontPointer, unchecked((IntPtr)exisitingFont.mFont));
			}
		}
		else
		{
			propertiesUsed = properties;

			if (fileName is not null)
			{
				fileNameBackup = propertiesUsed.TryGetStringValue(PropertyNames.CreateFileNameString, out var existingFileName)
					? existingFileName
					: null;

				propertiesUsed.TrySetStringValue(PropertyNames.CreateFileNameString, fileName);
			}

			if (stream is not null)
			{
				streamBackup = propertiesUsed.TryGetPointerValue(PropertyNames.CreateIOStreamPointer, out var existingStreamPtr)
					? existingStreamPtr
					: null;

				propertiesUsed.TrySetPointerValue(PropertyNames.CreateIOStreamPointer, unchecked((IntPtr)stream.Pointer));
			}

			if (streamOffset is long streamOffsetValue)
			{
				streamOffsetBackup = propertiesUsed.TryGetNumberValue(PropertyNames.CreateIOStreamOffsetNumber, out var existingStreamOffset)
					? existingStreamOffset
					: null;

				propertiesUsed.TrySetNumberValue(PropertyNames.CreateIOStreamOffsetNumber, streamOffsetValue);
			}

			if (closeAfterwards is bool closeAfterwardsValue)
			{
				closeAfterwardsBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateIOStreamAutoCloseBoolean, out var existingCloseAfterwards)
					? existingCloseAfterwards
					: null;

				propertiesUsed.TrySetBooleanValue(PropertyNames.CreateIOStreamAutoCloseBoolean, closeAfterwardsValue);
			}

			if (size is float sizeValue)
			{
				sizeBackup = propertiesUsed.TryGetFloatValue(PropertyNames.CreateSizeFloat, out var existingSize)
					? existingSize
					: null;

				propertiesUsed.TrySetFloatValue(PropertyNames.CreateSizeFloat, sizeValue);
			}

			if (faceIndex is int faceIndexValue)
			{
				faceIndexBackup = propertiesUsed.TryGetNumberValue(PropertyNames.CreateFaceNumber, out var existingFaceIndex)
					? unchecked((int)existingFaceIndex)
					: null;

				propertiesUsed.TrySetNumberValue(PropertyNames.CreateFaceNumber, faceIndexValue);
			}

			if (horizontalDpi is int horizontalDpiValue)
			{
				horizontalDpiBackup = propertiesUsed.TryGetNumberValue(PropertyNames.CreateHorizontalDpiNumber, out var existingHorizontalDpi)
					? unchecked((int)existingHorizontalDpi)
					: null;

				propertiesUsed.TrySetNumberValue(PropertyNames.CreateHorizontalDpiNumber, horizontalDpiValue);
			}

			if (verticalDpi is int verticalDpiValue)
			{
				verticalDpiBackup = propertiesUsed.TryGetNumberValue(PropertyNames.CreateVerticalDpiNumber, out var existingVerticalDpi)
					? unchecked((int)existingVerticalDpi)
					: null;

				propertiesUsed.TrySetNumberValue(PropertyNames.CreateVerticalDpiNumber, verticalDpiValue);
			}

			if (exisitingFont is not null)
			{
				existingFontBackup = propertiesUsed.TryGetPointerValue(PropertyNames.CreateExistingFontPointer, out var existingFontPtr)
					? existingFontPtr
					: null;

				propertiesUsed.TrySetPointerValue(PropertyNames.CreateExistingFontPointer, unchecked((IntPtr)exisitingFont.mFont));
			}
		}

		try
		{
			return TTF_OpenFontWithProperties(propertiesUsed.Id);
		}
		finally
		{
			if (properties is null)
			{
				// propertiesUsed was just a temporary instance we created for this call, so we need to dispose it now

				propertiesUsed.Dispose();
			}
			else
			{
				// we restore the original properties values from the given properties instance

				if (fileName is not null)
				{
					if (fileNameBackup is not null)
					{
						propertiesUsed.TrySetStringValue(PropertyNames.CreateFileNameString, fileNameBackup);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateFileNameString);
					}
				}

				if (stream is not null)
				{
					if (streamBackup is IntPtr streamPtr)
					{
						propertiesUsed.TrySetPointerValue(PropertyNames.CreateIOStreamPointer, streamPtr);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateIOStreamPointer);
					}
				}

				if (streamOffset.HasValue)
				{
					if (streamOffsetBackup is long streamOffsetValue)
					{
						propertiesUsed.TrySetNumberValue(PropertyNames.CreateIOStreamOffsetNumber, streamOffsetValue);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateIOStreamOffsetNumber);
					}
				}

				if (closeAfterwards.HasValue)
				{
					if (closeAfterwardsBackup is bool closeAfterwardsValue)
					{
						propertiesUsed.TrySetBooleanValue(PropertyNames.CreateIOStreamAutoCloseBoolean, closeAfterwardsValue);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateIOStreamAutoCloseBoolean);
					}
				}

				if (size.HasValue)
				{
					if (sizeBackup is float sizeValue)
					{
						propertiesUsed.TrySetFloatValue(PropertyNames.CreateSizeFloat, sizeValue);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateSizeFloat);
					}
				}

				if (faceIndex.HasValue)
				{
					if (faceIndexBackup is int faceIndexValue)
					{
						propertiesUsed.TrySetNumberValue(PropertyNames.CreateFaceNumber, faceIndexValue);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateFaceNumber);
					}
				}

				if (horizontalDpi.HasValue)
				{
					if (horizontalDpiBackup is int horizontalDpiValue)
					{
						propertiesUsed.TrySetNumberValue(PropertyNames.CreateHorizontalDpiNumber, horizontalDpiValue);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateHorizontalDpiNumber);
					}
				}

				if (verticalDpi.HasValue)
				{
					if (verticalDpiBackup is int verticalDpiValue)
					{
						propertiesUsed.TrySetNumberValue(PropertyNames.CreateVerticalDpiNumber, verticalDpiValue);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateVerticalDpiNumber);
					}
				}

				if (exisitingFont is not null)
				{
					if (existingFontBackup is IntPtr existingFontPtr)
					{
						propertiesUsed.TrySetPointerValue(PropertyNames.CreateExistingFontPointer, existingFontPtr);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateExistingFontPointer);
					}
				}
			}
		}
	}

	/// <inheritdoc cref="ValidateFont(TTF_Font*)"/>
	private unsafe Font(string fileName, float size, int? faceIndex, int? horizontalDpi, int? verticalDpi, Properties? properties, IUnsafeConstructorDispatch? _ = default) :
		this(ValidateFont(CreateWithProperties(fileName: fileName, size: size, faceIndex: faceIndex, horizontalDpi: horizontalDpi, verticalDpi: verticalDpi, properties: properties)), register: true)
	{ }

	/// <summary>
	/// Creates a new <see cref="Font"/> from the specified font file, point size, and additional properties
	/// </summary>
	/// <param name="fileName">The path to the font file</param>
	/// <param name="size">The point size of the font</param>
	/// <param name="faceIndex">The face index of the font, if it contains multiple font faces</param>
	/// <param name="horizontalDpi">The horizontal DPI of the font. Defaults to <paramref name="verticalDpi"/>, if not specified while <paramref name="verticalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="verticalDpi">The vertical DPI of the font. Defaults to <paramref name="horizontalDpi"/>, if not specified while <paramref name="horizontalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="properties">Additional properties</param>
	/// <remarks>
	/// <para>
	/// Some font files contain multiple sizes, so the point <paramref name="size"/> will specify the index of which size to use.
	/// If the value is too high, the last possible indexed size will be used as the default.
	/// </para>
	/// <para>
	/// Some font files contain multiple font faces, so the optional <paramref name="faceIndex"/> can be used to specify the index of which face to use.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Font(string, float, int?, int?, int?, Properties?, IUnsafeConstructorDispatch?)"/>
	public Font(string fileName, float size, int? faceIndex = default, int? horizontalDpi = default, int? verticalDpi = default, Properties? properties = default) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(fileName, size, faceIndex, horizontalDpi, verticalDpi, properties, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034 
	{ }

	/// <inheritdoc cref="ValidateFont(TTF_Font*)"/>
	private unsafe Font(Stream stream, long streamOffset, bool closeAfterwards, float size, int? faceIndex = default, int? horizontalDpi = default, int? verticalDpi = default, Properties? properties = default, IUnsafeConstructorDispatch? _ = default)
		: this(ValidateFont(CreateWithProperties(stream: stream, streamOffset: streamOffset, closeAfterwards: closeAfterwards, size: size, faceIndex: faceIndex, horizontalDpi: horizontalDpi, verticalDpi: verticalDpi, properties: properties)), register: true)
	{ }

	/// <summary>
	/// Creates a new <see cref="Font"/> from the specified <see cref="Stream"/>, point size, and additional properties
	/// </summary>
	/// <param name="stream">The <see cref="Stream"/> containing the font data</param>
	/// <param name="streamOffset">The byte offset into the given <paramref name="stream"/> at which the font data begins</param>
	/// <param name="closeAfterwards">A value indicating whether the given <paramref name="stream"/> should be automatically closed <em>when the resulting font is <see cref="Dispose()">disposed</see></em></param>
	/// <param name="size">The point size of the font</param>
	/// <param name="faceIndex">The face index of the font, if it contains multiple font faces</param>
	/// <param name="horizontalDpi">The horizontal DPI of the font. Defaults to <paramref name="verticalDpi"/>, if not specified while <paramref name="verticalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="verticalDpi">The vertical DPI of the font. Defaults to <paramref name="horizontalDpi"/>, if not specified while <paramref name="horizontalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="properties">Additional properties</param>
	/// <remarks>
	/// <para>
	/// Some font files contain multiple sizes, so the point <paramref name="size"/> will specify the index of which size to use.
	/// If the value is too high, the last possible indexed size will be used as the default.
	/// </para>
	/// <para>
	/// Some font files contain multiple font faces, so the optional <paramref name="faceIndex"/> can be used to specify the index of which face to use.
	/// </para>
	/// <para>
	/// If <paramref name="closeAfterwards"/> is <c><see langword="true"/></c>, the given <paramref name="stream"/> will be automatically closed when the resulting font is <see cref="Dispose()">disposed</see>.
	/// Otherwise, you will be responsible for disposing the given <paramref name="stream"/> yourself after that.
	/// Either way, you <em>must</em> keep the given <paramref name="stream"/> open and undisposed until the resulting font is disposed. 
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Font(Stream, long, bool, float, int?, int?, int?, Properties?, IUnsafeConstructorDispatch?)"/>
	public Font(Stream stream, long streamOffset, bool closeAfterwards, float size, int? faceIndex = default, int? horizontalDpi = default, int? verticalDpi = default, Properties? properties = default) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(stream, streamOffset, closeAfterwards, size, faceIndex, horizontalDpi, verticalDpi, properties, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <summary>
	/// Creates a new <see cref="Font"/> from the specified <see cref="Stream"/>, point size, and additional properties
	/// </summary>
	/// <param name="stream">The <see cref="Stream"/> containing the font data</param>
	/// <param name="streamOffset">The byte offset into the given <paramref name="stream"/> at which the font data begins</param>
	/// <param name="size">The point size of the font</param>
	/// <param name="faceIndex">The face index of the font, if it contains multiple font faces</param>
	/// <param name="horizontalDpi">The horizontal DPI of the font. Defaults to <paramref name="verticalDpi"/>, if not specified while <paramref name="verticalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="verticalDpi">The vertical DPI of the font. Defaults to <paramref name="horizontalDpi"/>, if not specified while <paramref name="horizontalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="properties">Additional properties</param>
	/// <remarks>
	/// <para>
	/// Some font files contain multiple sizes, so the point <paramref name="size"/> will specify the index of which size to use.
	/// If the value is too high, the last possible indexed size will be used as the default.
	/// </para>
	/// <para>
	/// Some font files contain multiple font faces, so the optional <paramref name="faceIndex"/> can be used to specify the index of which face to use.
	/// </para>
	/// <para>
	/// This constructor does <em>not</em> automatically close the given <paramref name="stream"/> when the resulting font is <see cref="Dispose()">disposed</see>.
	/// You <em>must</em> keep the given <paramref name="stream"/> open and undisposed until the resulting font is disposed.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Font(Stream, long, bool, float, int?, int?, int?, Properties?, IUnsafeConstructorDispatch?)"/>
	public Font(Stream stream, long streamOffset, float size, int? faceIndex = default, int? horizontalDpi = default, int? verticalDpi = default, Properties? properties = default) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(stream, streamOffset, closeAfterwards: false, size, faceIndex, horizontalDpi, verticalDpi, properties, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <summary>
	/// Creates a new <see cref="Font"/> from the specified <see cref="Stream"/>, point size, and additional properties
	/// </summary>
	/// <param name="stream">The <see cref="Stream"/> containing the font data</param>
	/// <param name="closeAfterwards">A value indicating whether the given <paramref name="stream"/> should be automatically closed <em>when the resulting font is <see cref="Dispose()">disposed</see></em></param>
	/// <param name="size">The point size of the font</param>
	/// <param name="faceIndex">The face index of the font, if it contains multiple font faces</param>
	/// <param name="horizontalDpi">The horizontal DPI of the font. Defaults to <paramref name="verticalDpi"/>, if not specified while <paramref name="verticalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="verticalDpi">The vertical DPI of the font. Defaults to <paramref name="horizontalDpi"/>, if not specified while <paramref name="horizontalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="properties">Additional properties</param>
	/// <remarks>
	/// <para>
	/// Some font files contain multiple sizes, so the point <paramref name="size"/> will specify the index of which size to use.
	/// If the value is too high, the last possible indexed size will be used as the default.
	/// </para>
	/// <para>
	/// Some font files contain multiple font faces, so the optional <paramref name="faceIndex"/> can be used to specify the index of which face to use.
	/// </para>
	/// <para>
	/// If <paramref name="closeAfterwards"/> is <c><see langword="true"/></c>, the given <paramref name="stream"/> will be automatically closed when the resulting font is <see cref="Dispose()">disposed</see>.
	/// Otherwise, you will be responsible for disposing the given <paramref name="stream"/> yourself after that.
	/// Either way, you <em>must</em> keep the given <paramref name="stream"/> open and undisposed until the resulting font is disposed. 
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Font(Stream, long, bool, float, int?, int?, int?, Properties?, IUnsafeConstructorDispatch?)"/>
	public Font(Stream stream, bool closeAfterwards, float size, int? faceIndex = default, int? horizontalDpi = default, int? verticalDpi = default, Properties? properties = default) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(stream, streamOffset: 0, closeAfterwards, size, faceIndex, horizontalDpi, verticalDpi, properties, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <summary>
	/// Creates a new <see cref="Font"/> from the specified <see cref="Stream"/>, point size, and additional properties
	/// </summary>
	/// <param name="stream">The <see cref="Stream"/> containing the font data</param>
	/// <param name="size">The point size of the font</param>
	/// <param name="faceIndex">The face index of the font, if it contains multiple font faces</param>
	/// <param name="horizontalDpi">The horizontal DPI of the font. Defaults to <paramref name="verticalDpi"/>, if not specified while <paramref name="verticalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="verticalDpi">The vertical DPI of the font. Defaults to <paramref name="horizontalDpi"/>, if not specified while <paramref name="horizontalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="properties">Additional properties</param>
	/// <remarks>
	/// <para>
	/// Some font files contain multiple sizes, so the point <paramref name="size"/> will specify the index of which size to use.
	/// If the value is too high, the last possible indexed size will be used as the default.
	/// </para>
	/// <para>
	/// Some font files contain multiple font faces, so the optional <paramref name="faceIndex"/> can be used to specify the index of which face to use.
	/// </para>
	/// <para>
	/// This constructor does <em>not</em> automatically close the given <paramref name="stream"/> when the resulting font is <see cref="Dispose()">disposed</see>.
	/// You <em>must</em> keep the given <paramref name="stream"/> open and undisposed until the resulting font is disposed.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Font(Stream, long, bool, float, int?, int?, int?, Properties?, IUnsafeConstructorDispatch?)"/>
	public Font(Stream stream, float size, int? faceIndex = default, int? horizontalDpi = default, int? verticalDpi = default, Properties? properties = default) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(stream, streamOffset: 0, closeAfterwards: false, size, faceIndex, horizontalDpi, verticalDpi, properties, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	private unsafe Font(Font exisitingFont, float? size, int? faceIndex, int? horizontalDpi, int? verticalDpi, Properties? properties, IUnsafeConstructorDispatch? _ = default) :
		this(ValidateFont(CreateWithProperties(exisitingFont: exisitingFont, size: size, faceIndex: faceIndex, horizontalDpi: horizontalDpi, verticalDpi: verticalDpi, properties: properties)), register: true)
	{ }

	/// <summary>
	/// Creates a new <see cref="Font"/> as a copy of an existing <see cref="Font"/>, using its size and styles unless specified otherwise
	/// </summary>
	/// <param name="exisitingFont">The existing <see cref="Font"/> to copy</param>
	/// <param name="size">The point size of the font</param>
	/// <param name="faceIndex">The face index of the font, if it contains multiple font faces</param>
	/// <param name="horizontalDpi">The horizontal DPI of the font. Defaults to <paramref name="verticalDpi"/>, if not specified while <paramref name="verticalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="verticalDpi">The vertical DPI of the font. Defaults to <paramref name="horizontalDpi"/>, if not specified while <paramref name="horizontalDpi"/> is; otherwise <c>72</c></param>
	/// <param name="properties">Additional properties</param>
	/// <remarks>
	/// <para>
	/// Some font files contain multiple sizes, so the point <paramref name="size"/> will specify the index of which size to use.
	/// If the value is too high, the last possible indexed size will be used as the default.
	/// </para>
	/// <para>
	/// Some font files contain multiple font faces, so the optional <paramref name="faceIndex"/> can be used to specify the index of which face to use.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Font(Font, float?, int?, int?, int?, Properties?, IUnsafeConstructorDispatch?)"/>
	public Font(Font exisitingFont, float? size = default, int? faceIndex = default, int? horizontalDpi = default, int? verticalDpi = default, Properties? properties = default) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(exisitingFont, size, faceIndex, horizontalDpi, verticalDpi, properties, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <inheritdoc/>
	~Font() => Dispose(forget: true);

	/// <summary>
	/// Gets the offset from the baseline to the top of this font,
	/// </summary>
	/// <value>
	/// The offset from the baseline to the top of this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will be positive, relative to the baseline, if the top of the font is above the baseline, 
	/// and will be more positive the further above the baseline the top of the font is.
	/// </para>
	/// </remarks>
	public int Ascent
	{
		get
		{
			unsafe
			{
				return TTF_GetFontAscent(mFont);
			}
		}
	}

#if SDL_TTF3_4_0_OR_GREATER

	/// <summary>
	/// Gets or sets the additional spacing, in pixels, that is applied between any two rendered characters of this font
	/// </summary>
	/// <value>
	/// The additional spacing, in pixels, that is applied between any two rendered characters of this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will be added to the regular glyph's advance and is applied uniformly after each character when rendering text with this font.
	/// </para>
	/// <para>
	/// The value of this property my be negative, in which case it will reduce the spacing between characters.
	/// </para>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the character spacing couldn't be set for the font (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>"
	public int CharacterSpacing
	{
		get
		{
			unsafe
			{
				return TTF_GetFontCharSpacing(mFont);
			}
		}

		set
		{
			unsafe
			{
				// To be on-par with the rest of the API, we don't throw if the font is invalid (i.e, has been disposed); all other SDL errors will be thrown, though.
				SdlErrorHelper.ThrowIfFailed(TTF_SetFontCharSpacing(mFont, value), filterError: GetInvalidFontErrorMessage());
			}
		}
	}

#endif

	/// <summary>
	/// Gets the offset from the baseline to the bottom of this font
	/// </summary>
	/// <value>
	/// The offset from the baseline to the bottom of this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will be negative, relative to the baseline, if the bottom of the font is below the baseline,
	/// and will be more negative the further below the baseline the bottom of the font is.
	/// </para>
	/// </remarks>
	public int Descent
	{
		get
		{
			unsafe
			{
				return TTF_GetFontDescent(mFont);
			}
		}
	}

	/// <summary>
	/// Gets or sets the direction used in text shaping for this font
	/// </summary>
	/// <value>
	/// The direction used in text shaping for this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defaults to <see cref="Direction.Invalid"/> when not set.
	/// </para>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the text shaping direction couldn't be set for the font (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public Direction Direction
	{
		get
		{
			unsafe
			{
				return TTF_GetFontDirection(mFont);
			}
		}

		set
		{
			unsafe
			{
				// To be on-par with the rest of the API, we don't throw if the font is invalid (i.e, has been disposed); all other SDL errors will be thrown, though.
				SdlErrorHelper.ThrowIfFailed(TTF_SetFontDirection(mFont, value), filterError: GetInvalidFontErrorMessage());
			}
		}
	}

	/// <summary>
	/// Gets or sets the target resolutions, in dots per inch (DPI), used for rendering this font
	/// </summary>
	/// <value>
	/// The target resolutions, in dots per inch (DPI), used for rendering this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is distinguished in horizontal and vertical components, representing the target horizontal and vertical DPI, respectively.
	/// </para>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When getting or setting this property, the font's DPI couldn't be retrieved or set, respectively (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public (int Horizontal, int Vertical) Dpi
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (int Horizontal, int Vertical) dpi);

				// To be on-par with the rest of the API, we don't throw if the font is invalid (i.e, has been disposed); all other SDL errors will be thrown, though.
				SdlErrorHelper.ThrowIfFailed(TTF_GetFontDPI(mFont, &dpi.Horizontal, &dpi.Vertical), filterError: GetInvalidFontErrorMessage());

				return dpi;
			}
		}

		set
		{
			unsafe
			{
				// SDL_ttf only offers a TTF_SetFontSizeDPI function to simultaneously set the font size and DPI.
				// I suppose that's done that way because of optimizations and trying to prevent to rebuilt the glyph cache multiple times.
				// But I really linke to separate those two things in the managed API.
				// So what I'm going to do is call TTF_SetFontSizeDPI with the current font size and the new DPI values
				// and we just live with the fact that if someone wants to set the size as well as the DPI from the managed API, they will have to live with some performance drawbacks.

				// To be on-par with the rest of the API, we don't throw if the font is invalid (i.e, has been disposed); all other SDL errors will be thrown, though.
				SdlErrorHelper.ThrowIfFailed(TTF_SetFontSizeDPI(mFont, TTF_GetFontSize(mFont), value.Horizontal, value.Vertical), filterError: GetInvalidFontErrorMessage());
			}
		}
	}

	/// <summary>
	/// Gets the number of FreeType font faces for this font
	/// </summary>
	/// <value>
	/// The number of FreeType font faces for this font
	/// </value>
	public int FaceCount
	{
		get
		{
			unsafe
			{
				return TTF_GetNumFontFaces(mFont);
			}
		}
	}

	/// <summary>
	/// Gets the collection of fallback fonts associated with this font
	/// </summary>
	/// <value>
	/// The collection of fallback fonts associated with this font
	/// </value>
	public FallbackCollection Fallbacks { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mFallbacks; }

	/// <summary>
	/// Gets the family name of this font
	/// </summary>
	/// <value>
	/// The family name of this font, or <c><see langword="null"/></c> if the family name could not be retrieved successfully
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is determined by the underlying font file and can be occasionally be <c><see langword="null"/></c> depending on that file.
	/// </para>
	/// </remarks>
	public string? FamilyName
	{
		get
		{
			unsafe
			{
				using var familyNameUtf16 = NativeStrings.FromUtf8ToUtf16(TTF_GetFontFamilyName(mFont));

				return familyNameUtf16.ToManaged();  // This can return null if mFont is invalid (i.e., has been disposed), or if there was a problem with the font file
			}
		}
	}

	/// <summary>
	/// Gets the generation of this font
	/// </summary>
	/// <value>
	/// The generation of this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The generation of a font is a value that is incremented each time the font is modified in a way that would require the glyph cache to be rebuilt, e.g., when changing the font's <see cref="Style"/> or <see cref="Size"/>.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	public uint Generation
	{
		get
		{
			unsafe
			{
				return TTF_GetFontGeneration(mFont);
			}
		}
	}

	/// <summary>
	/// Gets the total height of this font
	/// </summary>
	/// <value>
	/// The total height of this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is usually equal to the <see cref="Size"/> of the font.
	/// </para>
	/// </remarks>
	public int Height
	{
		get
		{
			unsafe
			{
				return TTF_GetFontHeight(mFont);
			}
		}
	}

	/// <summary>
	/// Gets or sets the hinting mode used for this font
	/// </summary>
	/// <value>
	/// The hinting mode used for this font
	/// </value>
	/// <remarks>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	public Hinting Hinting
	{
		get
		{
			unsafe
			{
				return TTF_GetFontHinting(mFont);
			}
		}

		set
		{
			unsafe
			{
				TTF_SetFontHinting(mFont, value);
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether this font is a fixed-width font
	/// </summary>
	/// <value>
	/// A value indicating whether this font is a fixed-width font
	/// </value>
	/// <remarks>
	/// <para>
	/// A "fixed-width" font means all glyphs of that font are the same width across; a lowercase 'i' will be the same size across as a capital 'W', for example.
	/// This is common for terminals and text editors, and other apps that treat text as a grid.
	/// Most other things (WYSIWYG word processors, web pages, etc) are more likely to use fonts that are not fixed-width in most cases.
	/// </para>
	/// </remarks>
	public bool IsFixedWitdh
	{
		get
		{
			unsafe
			{
				return TTF_FontIsFixedWidth(mFont);
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether kerning is enabled for this font
	/// </summary>
	/// <value>
	/// A value indicating whether kerning is enabled for this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defaults to <c><see langword="true"/></c> for newly created fonts.
	/// This is generally a good policy unless you have a strong reason to disable it, as it tends to produce better rendering
	/// (with kerning disabled, some fonts might render the word <c>kerning</c> as something that looks like <c>keming</c>, for example).
	/// </para>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	public bool IsKerningEnabled
	{
		get
		{
			unsafe
			{
				return TTF_GetFontKerning(mFont);
			}
		}

		set
		{
			unsafe
			{
				TTF_SetFontKerning(mFont, value);
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether this font is a scalable font
	/// </summary>
	/// <value>
	/// A value indicating whether this font is a scalable font
	/// </value>
	/// <remarks>
	/// <para>
	/// The scalability of a font lets you distinguish between bitmap fonts and outline fonts.
	/// Bitmap fonts look best at their native size (the <see cref="IsScalable"/> property will have a value of <c><see langword="false"/></c>).
	/// Outline fonts, on the other hand, can be scaled to any size without compromising on quality (the <see cref="IsScalable"/> property will have a value of <c><see langword="true"/></c>).
	/// </para>
	/// </remarks>
	public bool IsScalable
	{
		get
		{
			unsafe
			{
				return TTF_FontIsScalable(mFont);
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether signed distance field (SDF) rendering is enabled for this font
	/// </summary>
	/// <value>
	/// A value indicating whether signed distance field (SDF) rendering is enabled for this font
	/// </value>
	/// <remarks>
	/// <para>
	/// Signed distance field (SDF) is a technique that helps fonts look sharp even when scaling and rotating, and requires special shader support for display.
	/// </para>
	/// <para>
	/// This works well with <see cref="TryRenderBlendedGlyph(Rune, Color{byte}, out Surface?)"/>, <see cref="TryRenderBlendedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>, <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{char}, Color{byte}, int, out Surface?)"/>, etc.,
	/// and generates the raw signed distance values in the alpha channel of the resulting texture.
	/// </para>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	public bool IsSdfEnabled
	{
		get
		{
			unsafe
			{
				return TTF_GetFontSDF(mFont);
			}
		}

		set
		{
			unsafe
			{
				// To be on-par with the rest of the API, we don't throw if the font is invalid (i.e, has been disposed); all other SDL errors will be thrown, though.
				SdlErrorHelper.ThrowIfFailed(TTF_SetFontSDF(mFont, value), filterError: GetInvalidFontErrorMessage());
			}
		}
	}

	/// <summary>
	/// Gets or sets the spacing between lines of text for this font
	/// </summary>
	/// <value>
	/// The spacing between lines of text for this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defaults to the font's recommended line spacing when not set.
	/// </para>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	public int LineSkip
	{
		get
		{
			unsafe
			{
				return TTF_GetFontLineSkip(mFont);
			}
		}

		set
		{
			unsafe
			{
				TTF_SetFontLineSkip(mFont, value);
			}
		}
	}

	/// <summary>
	/// Gets or sets the outline thickness used for rendering this font
	/// </summary>
	/// <value>
	/// The outline thickness used for rendering this font
	/// </value>
	/// <remarks>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the outline thickness couldn't be set for the font (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public int Outline
	{
		get
		{
			unsafe
			{
				return TTF_GetFontOutline(mFont);
			}
		}

		set
		{
			unsafe
			{
				// To be on-par with the rest of the API, we don't throw if the font is invalid (i.e, has been disposed); all other SDL errors will be thrown, though.
				SdlErrorHelper.ThrowIfFailed(TTF_SetFontOutline(mFont, value), filterError: GetInvalidFontErrorMessage());
			}
		}
	}

	/// <summary>
	/// Gets or sets a value determining how the end of outline lines are rendered for this font
	/// </summary>
	/// <value>
	/// A value determining how the end of outline lines are rendered for this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defaults to <see cref="LineCap.Round"/>.
	/// </para>
	/// </remarks>
	public LineCap OutlineLineCap
	{
		get => Properties?.TryGetNumberValue(PropertyNames.OutlineLineCapNumber, out var lineCap) is true
			? unchecked((LineCap)(int)lineCap)
			: LineCap.Round;

		set => Properties?.TrySetNumberValue(PropertyNames.OutlineLineCapNumber, unchecked((int)value));
	}

	/// <summary>
	/// Gets or sets a value determining how two joining outline lines are rendered for this font
	/// </summary>
	/// <value>
	/// A value determining how two joining outline lines are rendered for this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defaults to <see cref="LineJoin.Round"/>.
	/// </para>
	/// </remarks>
	public LineJoin OutlineLineJoin
	{
		get => Properties?.TryGetNumberValue(PropertyNames.OutlineLineJoinNumber, out var lineJoin) is true
			? unchecked((LineJoin)(int)lineJoin)
			: LineJoin.Round;

		set => Properties?.TrySetNumberValue(PropertyNames.OutlineLineJoinNumber, unchecked((int)value));
	}

	/// <summary>
	/// Gets or sets the miter limit used for rendering outline lines for this font
	/// </summary>
	/// <value>
	/// The miter limit used for rendering outline lines for this font
	/// </value>
	/// <remarks>
	/// <para>
	/// This property is relevant when the <see cref="OutlineLineJoin"/> of this font is set to <see cref="LineJoin.Miter"/>/<see cref="LineJoin.MiterVariable"/> or <see cref="LineJoin.MiterFixed"/>.
	/// </para>
	/// <para>
	/// The value of this property defaults to <c>0.0</c>.
	/// </para>
	/// </remarks>
	public double OutlineMiterLimit
	{
		get => Properties?.TryGetNumberValue(PropertyNames.OutlineMiterLimitNumber, out var miterLimit) is true
			? FixedPoint.FromSigned16Dot16(unchecked((uint)miterLimit))
			: 0.0;

		set => Properties?.TrySetNumberValue(PropertyNames.OutlineMiterLimitNumber, FixedPoint.ToSigned16Dot16(value));
	}

	internal unsafe TTF_Font* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mFont; }

	/// <summary>
	/// Gets the properties associated with this font
	/// </summary>
	/// <value>
	/// The properties associated with this font, or <c><see langword="null"/></c> if the properties could not be retrieved successfully (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	public Properties? Properties
	{
		get
		{
			unsafe
			{
				return TTF_GetFontProperties(mFont) switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		}
	}

	/// <summary>
	/// Gets or sets the script used in text shaping for this font
	/// </summary>
	/// <value>
	/// The script used in text shaping for this font
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defaults to <c><see langword="default"/>(<see cref="Sdl3Sharp.Ttf.Script"/>)</c> when not set.
	/// </para>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the text shaping script couldn't be set for the font (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public Script Script
	{
		get
		{
			unsafe
			{
				return TTF_GetFontScript(mFont);
			}
		}

		set
		{
			unsafe
			{
				// To be on-par with the rest of the API, we don't throw if the font is invalid (i.e, has been disposed); all other SDL errors will be thrown, though.
				SdlErrorHelper.ThrowIfFailed(TTF_SetFontScript(mFont, value), filterError: GetInvalidFontErrorMessage());
			}
		}
	}

	/// <summary>
	/// Gets or sets the point size used for rendering this font
	/// </summary>
	/// <value>
	/// The point size used for rendering this font
	/// </value>
	/// <remarks>
	/// <para>
	/// When getting the value of this property, if the returned value is <c>0.0</c>, there might have been an error in retrieving the font size (check <see cref="Error.TryGet(out string?)"/> for more information).
	/// </para>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the point size couldn't be set for the font (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public float Size
	{
		get
		{
			unsafe
			{
				return TTF_GetFontSize(mFont);
			}
		}

		set
		{
			unsafe
			{
				// To be on-par with the rest of the API, we don't throw if the font is invalid (i.e, has been disposed); all other SDL errors will be thrown, though.
				SdlErrorHelper.ThrowIfFailed(TTF_SetFontSize(mFont, value), filterError: GetInvalidFontErrorMessage());
			}
		}
	}

	/// <summary>
	/// Gets or sets the style used for rendering this font
	/// </summary>
	/// <value>
	/// The style used for rendering this font
	/// </value>
	/// <remarks>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	public FontStyles Style
	{
		get
		{
			unsafe
			{
				return TTF_GetFontStyle(mFont);
			}
		}

		set
		{
			unsafe
			{
				TTF_SetFontStyle(mFont, value);
			}
		}
	}

	/// <summary>
	/// Gets the style name of this font
	/// </summary>
	/// <value>
	/// The style name of this font, or <c><see langword="null"/></c> if the style name could not be retrieved successfully
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is determined by the underlying font file and can be occasionally be <c><see langword="null"/></c> depending on that file.
	/// </para>
	/// </remarks>
	public string? StyleName
	{
		get
		{
			unsafe
			{
				using var styleNameUtf16 = NativeStrings.FromUtf8ToUtf16(TTF_GetFontStyleName(mFont));

				return styleNameUtf16.ToManaged();  // This can return null if mFont is invalid (i.e., has been disposed), or if there was a problem with the font file
			}
		}
	}

#if SDL_TTF3_4_0_OR_GREATER

	/// <summary>
	/// Gets the weight in terms of the lightness or heaviness of the strokes of this font
	/// </summary>
	/// <value>
	/// The weight in terms of the lightness or heaviness of the strokes of this font
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	public FontWeight Weight
	{
		get
		{
			unsafe
			{
				return TTF_GetFontWeight(mFont);
			}
		}
	}

#endif

	/// <summary>
	/// Gets or sets the horizontal alignment used when rendering wrapped text with this font
	/// </summary>
	/// <value>
	/// The horizontal alignment used when rendering wrapped text with this font
	/// </value>
	/// <remarks>
	/// <para>
	/// Setting the value of this property will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the font.
	/// </para>
	/// </remarks>
	public HorizontalAlignment WrapAlignment
	{
		get
		{
			unsafe
			{
				return TTF_GetFontWrapAlignment(mFont);
			}
		}

		set
		{
			unsafe
			{
				TTF_SetFontWrapAlignment(mFont, value);
			}
		}
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(forget: true);
	}

	private void Dispose(bool forget)
	{
		unsafe
		{
			if (mFont is not null)
			{
				if (forget)
				{
					mKnownInstances.TryRemove(unchecked((IntPtr)mFont), out _);
				}

				TTF_CloseFont(mFont);
				mFont = null;
			}
		}
	}

	/// <summary>
	/// Determines whether this font contains a glyph for a given Unicode code point
	/// </summary>
	/// <param name="glyph">The Unicode code point to check</param>
	/// <returns><c><see langword="true"/></c>, if the font contains the glyph; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method should only be called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool HasGlyph(Rune glyph)
	{
		unsafe
		{
			return TTF_FontHasGlyph(mFont, Unsafe.BitCast<Rune, uint>(glyph));
		}
	}

	/// <summary>
	/// Determines whether this font contains a glyph for a given UTF-16 unit
	/// </summary>
	/// <param name="glyph">The UTF-16 unit to check</param>
	/// <returns><c><see langword="true"/></c>, if the font contains the glyph; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method is a convenience overload for <see cref="HasGlyph(Rune)"/>.
	/// </para>
	/// <para>
	/// This method should only be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="glyph"/> represents a UTF-16 surrogate code point (U+D800..U+DFFF, inclusive)</exception>
	public bool HasGlyph(char glyph) => HasGlyph(new Rune(glyph));

	/// <summary>
	/// Determines whether this font contains a glyph for a given UTF-16 surrogate pair
	/// </summary>
	/// <param name="glyphHighSurrogate">The high surrogate of the UTF-16 pair to check</param>
	/// <param name="glyphLowSurrogate">The low surrogate of the UTF-16 pair to check</param>
	/// <returns><c><see langword="true"/></c>, if the font contains the glyph; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method is a convenience overload for <see cref="HasGlyph(Rune)"/>.
	/// </para>
	/// <para>
	/// This method should only be called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="glyphHighSurrogate"/> does not represent a UTF-16 high surrogate code point
	/// - OR -
	/// <paramref name="glyphLowSurrogate"/> does not represent a UTF-16 low surrogate code point
	/// </exception>
	public bool HasGlyph(char glyphHighSurrogate, char glyphLowSurrogate) => HasGlyph(new Rune(glyphHighSurrogate, glyphLowSurrogate));

	/// <inheritdoc/>
	public override string ToString()
		=> FamilyName switch
		{
			null => nameof(Font),
			var familyName => $"{familyName}{StyleName switch
			{
				null => string.Empty,
				var styleName => $" - {styleName}"
			}}"
		};

	// This could also be "TryCopy" to be more faithful to the underlying SDL API,
	// but I believe "TryDuplicate" is more intuitive for C# devs,
	// and even Sdl3Sharp.Video.Surface uses "TryDuplicate", although in that case the underlying SDL API also uses "duplicate" as a term.
	/// <summary>
	/// Tries to duplicate this font
	/// </summary>
	/// <param name="duplicate">The duplicated font, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the font was duplicated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The duplicated font will be distinct from the original, but will share the font file and have the same size and style.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Dispose()">dispose</see> the duplicated font when you're done using it.
	/// The duplicated font will have a separate lifetime from the original, i.e., you can safely <see cref="Dispose()">dispose</see> the original font before the duplicate.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the original font.
	/// </para>
	/// </remarks>
	public bool TryDuplicate([NotNullWhen(true)] out Font? duplicate)
	{
		unsafe
		{
			var duplicatePtr = TTF_CopyFont(mFont);

			if (duplicatePtr is null)
			{
				duplicate = null;

				return false;
			}

			duplicate = new(duplicatePtr, register: true);

			return true;
		}
	}

	/// <summary>
	/// Tries to get pixel image for a glyph for a given Unicode code point from this font
	/// </summary>
	/// <param name="glyph">The Unicode code point of the glyph to retrieve</param>
	/// <param name="image">The pixel image of the glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <param name="imageType">The type of data contained in the glyph <paramref name="image"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see cref="ImageType.Invalid"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph image was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned image when you're done using it.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryGetGlyphImage(Rune glyph, [NotNullWhen(true)] out Surface? image, out ImageType imageType)
	{
		unsafe
		{
			Unsafe.SkipInit(out ImageType imageTypeTmp);

			var imagePtr = TTF_GetGlyphImage(mFont, Unsafe.BitCast<Rune, uint>(glyph), &imageTypeTmp);

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

	/// <summary>
	/// Tries to get pixel image for a glyph for a given UTF-16 unit from this font
	/// </summary>
	/// <param name="glyph">The UTF-16 unit of the glyph to retrieve</param>
	/// <param name="image">The pixel image of the glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <param name="imageType">The type of data contained in the glyph <paramref name="image"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see cref="ImageType.Invalid"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph image was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetGlyphImage(Rune, out Surface?, out ImageType)"/>.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned image when you're done using it.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="glyph"/> represents a UTF-16 surrogate code point (U+D800..U+DFFF, inclusive)</exception>
	public bool TryGetGlyphImage(char glyph, [NotNullWhen(true)] out Surface? image, out ImageType imageType)
		=> TryGetGlyphImage(new Rune(glyph), out image, out imageType);

	/// <summary>
	/// Tries to get pixel image for a glyph for a given UTF-16 surrogate pair from this font
	/// </summary>
	/// <param name="glyphHighSurrogate">The high surrogate of the UTF-16 pair of the glyph to retrieve</param>
	/// <param name="glyphLowSurrogate">The low surrogate of the UTF-16 pair of the glyph to retrieve</param>
	/// <param name="image">The pixel image of the glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <param name="imageType">The type of data contained in the glyph <paramref name="image"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see cref="ImageType.Invalid"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph image was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetGlyphImage(Rune, out Surface?, out ImageType)"/>.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned image when you're done using it.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="glyphHighSurrogate"/> does not represent a UTF-16 high surrogate code point
	/// - OR -
	/// <paramref name="glyphLowSurrogate"/> does not represent a UTF-16 low surrogate code point
	/// </exception>
	public bool TryGetGlyphImage(char glyphHighSurrogate, char glyphLowSurrogate, [NotNullWhen(true)] out Surface? image, out ImageType imageType)
		=> TryGetGlyphImage(new Rune(glyphHighSurrogate, glyphLowSurrogate), out image, out imageType);

	/// <summary>
	/// Tries to get the kerning distance between two Unicode code points for this font
	/// </summary>
	/// <param name="previousGlyph">The previous Unicode code point</param>
	/// <param name="glyph">The current Unicode code point</param>
	/// <param name="kerning">The kerning distance between the two code points, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the kerning distance was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryGetGlyphKerning(Rune previousGlyph, Rune glyph, out int kerning)
	{
		unsafe
		{
			Unsafe.SkipInit(out int kerningTmp);

			if (!(bool)TTF_GetGlyphKerning(mFont, Unsafe.BitCast<Rune, uint>(previousGlyph), Unsafe.BitCast<Rune, uint>(glyph), &kerningTmp))
			{
				kerning = default;

				return false;
			}

			kerning = kerningTmp;

			return true;
		}
	}

	/// <summary>
	/// Tries to get the kerning distance between two UTF-16 units for this font
	/// </summary>
	/// <param name="previousGlyph">The previous UTF-16 unit</param>
	/// <param name="glyph">The current UTF-16 unit</param>
	/// <param name="kerning">The kerning distance between the two UTF-16 units, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the kerning distance was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetGlyphKerning(Rune, Rune, out int)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="glyph"/> represents a UTF-16 surrogate code point (U+D800..U+DFFF, inclusive)</exception>
	public bool TryGetGlyphKerning(char previousGlyph, char glyph, out int kerning)
		=> TryGetGlyphKerning(new Rune(previousGlyph), new Rune(glyph), out kerning);

	/// <summary>
	/// Tries to get the kerning distance between two UTF-16 surrogate pairs for this font
	/// </summary>
	/// <param name="previousGlyphHighSurrogate">The high surrogate of the previous UTF-16 unit</param>
	/// <param name="previousGlyphLowSurrogate">The low surrogate of the previous UTF-16 unit</param>
	/// <param name="glyphHighSurrogate">The high surrogate of the current UTF-16 unit</param>
	/// <param name="glyphLowSurrogate">The low surrogate of the current UTF-16 unit</param>
	/// <param name="kerning">The kerning distance between the two UTF-16 surrogate pairs, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the kerning distance was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetGlyphKerning(Rune, Rune, out int)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="glyphHighSurrogate"/> does not represent a UTF-16 high surrogate code point
	/// - OR -
	/// <paramref name="glyphLowSurrogate"/> does not represent a UTF-16 low surrogate code point
	/// </exception>
	public bool TryGetGlyphKerning(char previousGlyphHighSurrogate, char previousGlyphLowSurrogate, char glyphHighSurrogate, char glyphLowSurrogate, out int kerning)
		=> TryGetGlyphKerning(new Rune(previousGlyphHighSurrogate, previousGlyphLowSurrogate), new Rune(glyphHighSurrogate, glyphLowSurrogate), out kerning);

	/// <summary>
	/// Tries to get the metrics (dimensions) of a glyph for a given Unicode code point from this font
	/// </summary>
	/// <param name="glyph">The Unicode code point of the glyph to get the metrics for</param>
	/// <param name="minX">
	/// The minimum horizontal coordinate of the glyph from the left egde of its bounding box.
	/// This value can be negative, if the coordinate lies to the left of the glyph's bounding box's origin.
	/// </param>
	/// <param name="maxX">
	/// The maximum horizontal coordinate of the glyph from the left edge of its bounding box
	/// </param>
	/// <param name="minY">
	/// The minimum vertical coordinate of the glyph from the bottom edge of its bounding box.
	/// This value can be negative, if the coordinate lies below the glyph's bounding box's origin.
	/// </param>
	/// <param name="maxY">
	/// The maximum vertical coordinate of the glyph from the bottom edge of its bounding box
	/// </param>
	/// <param name="advance">
	/// The horizontal advance of the glyph, i.e., the distance from the left edge of the next glyph's bounding box to the left edge of the given glyph's bounding box
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the glyph metrics were retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// To understand what these metrics mean, you can see FreeType's documentation on glyph metrics: <see href="https://freetype.sourceforge.net/freetype2/docs/tutorial/step2.html"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryGetGlyphMetrics(Rune glyph, out int minX, out int maxX, out int minY, out int maxY, out int advance)
	{
		unsafe
		{
			// One of the rare instances where pinning, even if it's on the heap,
			// could be actually more efficient than copying from temporary variables on the stack,
			// just because of the sheer number of variables we would have to copy (5).

			fixed (int* minXPtr = &minX, maxXPtr = &maxX, minYPtr = &minY, maxYPtr = &maxY, advancePtr = &advance)
			{
				if (!(bool)TTF_GetGlyphMetrics(mFont, Unsafe.BitCast<Rune, uint>(glyph), minXPtr, maxXPtr, minYPtr, maxYPtr, advancePtr))
				{
					// TTF_GetGlyphMetrics doesn't set defaults for the out parameters when it fails early,
					// so we have to do that ourselves

					minX = default;
					maxX = default;
					minY = default;
					maxY = default;
					advance = default;

					return false;
				}
			}

			return true;
		}
	}

	/// <summary>
	/// Tries to get the metrics (dimensions) of a glyph for a given UTF-16 unit from this font
	/// </summary>
	/// <param name="glyph">The UTF-16 unit of the glyph to get the metrics for</param>
	/// <param name="minX">
	/// The minimum horizontal coordinate of the glyph from the left egde of its bounding box.
	/// This value can be negative, if the coordinate lies to the left of the glyph's bounding box's origin.
	/// </param>
	/// <param name="maxX">
	/// The maximum horizontal coordinate of the glyph from the left edge of its bounding box
	/// </param>
	/// <param name="minY">
	/// The minimum vertical coordinate of the glyph from the bottom edge of its bounding box.
	/// This value can be negative, if the coordinate lies below the glyph's bounding box's origin.
	/// </param>
	/// <param name="maxY">
	/// The maximum vertical coordinate of the glyph from the bottom edge of its bounding box
	/// </param>
	/// <param name="advance">
	/// The horizontal advance of the glyph, i.e., the distance from the left edge of the next glyph's bounding box to the left edge of the given glyph's bounding box
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the glyph metrics were retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetGlyphMetrics(Rune, out int, out int, out int, out int, out int)"/>.
	/// </para>
	/// <para>
	/// To understand what these metrics mean, you can see FreeType's documentation on glyph metrics: <see href="https://freetype.sourceforge.net/freetype2/docs/tutorial/step2.html"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="glyph"/> represents a UTF-16 surrogate code point (U+D800..U+DFFF, inclusive)</exception>
	public bool TryGetGlyphMetrics(char glyph, out int minX, out int maxX, out int minY, out int maxY, out int advance)
		=> TryGetGlyphMetrics(new Rune(glyph), out minX, out maxX, out minY, out maxY, out advance);

	/// <summary>
	/// Tries to get the metrics (dimensions) of a glyph for a given UTF-16 surrogate pair from this font
	/// </summary>
	/// <param name="glyphHighSurrogate">The high surrogate of the UTF-16 pair</param>
	/// <param name="glyphLowSurrogate">The low surrogate of the UTF-16 pair</param>
	/// <param name="minX">
	/// The minimum horizontal coordinate of the glyph from the left egde of its bounding box.
	/// This value can be negative, if the coordinate lies to the left of the glyph's bounding box's origin.
	/// </param>
	/// <param name="maxX">
	/// The maximum horizontal coordinate of the glyph from the left edge of its bounding box
	/// </param>
	/// <param name="minY">
	/// The minimum vertical coordinate of the glyph from the bottom edge of its bounding box.
	/// This value can be negative, if the coordinate lies below the glyph's bounding box's origin.
	/// </param>
	/// <param name="maxY">
	/// The maximum vertical coordinate of the glyph from the bottom edge of its bounding box
	/// </param>
	/// <param name="advance">
	/// The horizontal advance of the glyph, i.e., the distance from the left edge of the next glyph's bounding box to the left edge of the given glyph's bounding box
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the glyph metrics were retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetGlyphMetrics(Rune, out int, out int, out int, out int, out int)"/>.
	/// </para>
	/// <para>
	/// To understand what these metrics mean, you can see FreeType's documentation on glyph metrics: <see href="https://freetype.sourceforge.net/freetype2/docs/tutorial/step2.html"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="glyphHighSurrogate"/> does not represent a UTF-16 high surrogate code point
	/// - OR -
	/// <paramref name="glyphLowSurrogate"/> does not represent a UTF-16 low surrogate code point
	/// </exception>
	public bool TryGetGlyphMetrics(char glyphHighSurrogate, char glyphLowSurrogate, out int minX, out int maxX, out int minY, out int maxY, out int advance)
		=> TryGetGlyphMetrics(new Rune(glyphHighSurrogate, glyphLowSurrogate), out minX, out maxX, out minY, out maxY, out advance);

	/// <summary>
	/// Tries to get the script used by a glyph for a given Unicode code point
	/// </summary>
	/// <param name="glyph">The Unicode code point of the glyph to get the script for</param>
	/// <param name="script">The script used by the glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="Sdl3Sharp.Ttf.Script"/>)</c></param>
	/// <returns><c><see langword="true"/></c>, if the script was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public static bool TryGetGlyphScript(Rune glyph, out Script script)
	{
		unsafe
		{
			script = TTF_GetGlyphScript(Unsafe.BitCast<Rune, uint>(glyph));

			return script is not 0;
		}
	}

	/// <summary>
	/// Tries to get the script used by a glyph for a given UTF-16 unit
	/// </summary>
	/// <param name="glyph">The UTF-16 unit of the glyph to get the script for</param>
	/// <param name="script">The script used by the glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="Sdl3Sharp.Ttf.Script"/>)</c></param>
	/// <returns><c><see langword="true"/></c>, if the script was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetGlyphScript(Rune, out Script)"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="glyph"/> represents a UTF-16 surrogate code point (U+D800..U+DFFF, inclusive)</exception>
	public static bool TryGetGlyphScript(char glyph, out Script script)
		=> TryGetGlyphScript(new Rune(glyph), out script);

	/// <summary>
	/// Tries to get the script used by a glyph for a given UTF-16 surrogate pair
	/// </summary>
	/// <param name="glyphHighSurrogate">The UTF-16 high surrogate of the glyph to get the script for</param>
	/// <param name="glyphLowSurrogate">The UTF-16 low surrogate of the glyph to get the script for</param>
	/// <param name="script">The script used by the glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="Sdl3Sharp.Ttf.Script"/>)</c></param>
	/// <returns><c><see langword="true"/></c>, if the script was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetGlyphScript(Rune, out Script)"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="glyphHighSurrogate"/> does not represent a UTF-16 high surrogate code point
	/// - OR -
	/// <paramref name="glyphLowSurrogate"/> does not represent a UTF-16 low surrogate code point
	/// </exception>
	public static bool TryGetGlyphScript(char glyphHighSurrogate, char glyphLowSurrogate, out Script script)
		=> TryGetGlyphScript(new Rune(glyphHighSurrogate, glyphLowSurrogate), out script);

	internal unsafe static bool TryGetOrCreate(TTF_Font* font, [NotNullWhen(true)] out Font? result)
	{
		if (font is null)
		{
			result = null;
			return false;
		}

		var fontRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)font), createRef);

		if (!fontRef.TryGetTarget(out result))
		{
			fontRef.SetTarget(result = create(font));
		}

		return true;

		static WeakReference<Font> createRef(IntPtr font) => new(create(unchecked((TTF_Font*)font)));

		static Font create(TTF_Font* font) => new(font, register: false);
	}

	/// <summary>
	/// Tries to calculate the dimensions of a rendered text with this font
	/// </summary>
	/// <param name="text">The UTF-16 text to calculate the dimensions of when rendered with this font</param>
	/// <param name="width">The width of the rendered text, in pixels</param>
	/// <param name="height">The height of the rendered text, in pixels</param>
	/// <returns><c><see langword="true"/></c>, if the dimensions were calculated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates the <paramref name="width"/> and <paramref name="height"/>, in pixels, of the area that would be occupied by the <paramref name="text"/> fully rendered with this font,
	/// without actually rendering the text.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryGetStringSize(ReadOnlySpan<char> text, out int width, out int height)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryGetStringSize(textUtf8.Buffer, textUtf8.Length, out width, out height);
		}
	}

	/// <summary>
	/// Tries to calculate the dimensions of a rendered text with this font
	/// </summary>
	/// <param name="text">The UTF-8 text to calculate the dimensions of when rendered with this font</param>
	/// <param name="width">The width of the rendered text, in pixels</param>
	/// <param name="height">The height of the rendered text, in pixels</param>
	/// <returns><c><see langword="true"/></c>, if the dimensions were calculated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates the <paramref name="width"/> and <paramref name="height"/>, in pixels, of the area that would be occupied by the <paramref name="text"/> fully rendered with this font,
	/// without actually rendering the text.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryGetStringSize(ReadOnlySpan<byte> text, out int width, out int height)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryGetStringSize(textPtr, unchecked((nuint)text.Length), out width, out height);
			}
		}
	}

	/// <summary>
	/// Tries to calculate the dimensions of a rendered text with this font
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to calculate the dimensions of when rendered with this font</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="width">The width of the rendered text, in pixels</param>
	/// <param name="height">The height of the rendered text, in pixels</param>
	/// <returns><c><see langword="true"/></c>, if the dimensions were calculated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates the <paramref name="width"/> and <paramref name="height"/>, in pixels, of the area that would be occupied by the <paramref name="text"/> fully rendered with this font,
	/// without actually rendering the text.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryGetStringSize(byte* text, nuint textLength, out int width, out int height)
	{
		Unsafe.SkipInit(out int widthTmp);
		Unsafe.SkipInit(out int heightTmp);

		bool result = TTF_GetStringSize(mFont, text, textLength, &widthTmp, &heightTmp);

		// TTF_GetStringSize sets the width and height to 0 if it fails
		width = widthTmp;
		height = heightTmp;

		return result;
	}

	/// <summary>
	/// Tries to calculate the dimensions of a rendered text with this font, with wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-16 text to calculate the dimensions of when rendered with this font</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="width">The width of the rendered text, in pixels</param>
	/// <param name="height">The height of the rendered text, in pixels</param>
	/// <returns><c><see langword="true"/></c>, if the dimensions were calculated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates the <paramref name="width"/> and <paramref name="height"/>, in pixels, of the area that would be occupied by the <paramref name="text"/> fully rendered with this font,
	/// taking into account the specified <paramref name="wrapWidth"/> and without actually rendering the text.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryGetWrappedStringSize(ReadOnlySpan{char}, out int, out int)"/> overload instead.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryGetWrappedStringSize(ReadOnlySpan<char> text, int wrapWidth, out int width, out int height)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryGetWrappedStringSize(textUtf8.Buffer, textUtf8.Length, wrapWidth, out width, out height);
		}
	}

	/// <summary>
	/// Tries to calculate the dimensions of a rendered text with this font, with wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-16 text to calculate the dimensions of when rendered with this font</param>
	/// <param name="width">The width of the rendered text, in pixels</param>
	/// <param name="height">The height of the rendered text, in pixels</param>
	/// <returns><c><see langword="true"/></c>, if the dimensions were calculated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates the <paramref name="width"/> and <paramref name="height"/>, in pixels, of the area that would be occupied by the <paramref name="text"/> fully rendered with this font,
	/// without actually rendering the text.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetWrappedStringSize(ReadOnlySpan{char}, int, out int, out int)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryGetWrappedStringSize(ReadOnlySpan<char> text, out int width, out int height)
		=> TryGetWrappedStringSize(text, wrapWidth: 0, out width, out height);

	/// <summary>
	/// Tries to calculate the dimensions of a rendered text with this font, with wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-8 text to calculate the dimensions of when rendered with this font</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="width">The width of the rendered text, in pixels</param>
	/// <param name="height">The height of the rendered text, in pixels</param>
	/// <returns><c><see langword="true"/></c>, if the dimensions were calculated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates the <paramref name="width"/> and <paramref name="height"/>, in pixels, of the area that would be occupied by the <paramref name="text"/> fully rendered with this font,
	/// taking into account the specified <paramref name="wrapWidth"/> and without actually rendering the text.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryGetWrappedStringSize(ReadOnlySpan{byte}, out int, out int)"/> overload instead.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryGetWrappedStringSize(ReadOnlySpan<byte> text, int wrapWidth, out int width, out int height)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryGetWrappedStringSize(textPtr, unchecked((nuint)text.Length), wrapWidth, out width, out height);
			}
		}
	}

	/// <summary>
	/// Tries to calculate the dimensions of a rendered text with this font, with wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-8 text to calculate the dimensions of when rendered with this font</param>
	/// <param name="width">The width of the rendered text, in pixels</param>
	/// <param name="height">The height of the rendered text, in pixels</param>
	/// <returns><c><see langword="true"/></c>, if the dimensions were calculated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates the <paramref name="width"/> and <paramref name="height"/>, in pixels, of the area that would be occupied by the <paramref name="text"/> fully rendered with this font,
	/// without actually rendering the text.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetWrappedStringSize(ReadOnlySpan{byte}, int, out int, out int)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryGetWrappedStringSize(ReadOnlySpan<byte> text, out int width, out int height)
		=> TryGetWrappedStringSize(text, wrapWidth: 0, out width, out height);

	/// <summary>
	/// Tries to calculate the dimensions of a rendered text with this font, with wrapping enabled
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to calculate the dimensions of when rendered with this font</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="width">The width of the rendered text, in pixels</param>
	/// <param name="height">The height of the rendered text, in pixels</param>
	/// <returns><c><see langword="true"/></c>, if the dimensions were calculated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates the <paramref name="width"/> and <paramref name="height"/>, in pixels, of the area that would be occupied by the <paramref name="text"/> fully rendered with this font,
	/// taking into account the specified <paramref name="wrapWidth"/> and without actually rendering the text.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryGetWrappedStringSize(byte*, nuint, out int, out int)"/> overload instead.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryGetWrappedStringSize(byte* text, nuint textLength, int wrapWidth, out int width, out int height)
	{
		unsafe
		{
			Unsafe.SkipInit(out int widthTmp);
			Unsafe.SkipInit(out int heightTmp);

			bool result = TTF_GetStringSizeWrapped(mFont, text, textLength, wrapWidth, &widthTmp, &heightTmp);

			// TTF_GetStringSizeWrapped sets the width and height to 0 if it fails
			width = widthTmp;
			height = heightTmp;

			return result;
		}
	}

	/// <summary>
	/// Tries to calculate the dimensions of a rendered text with this font, with wrapping enabled
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to calculate the dimensions of when rendered with this font</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="width">The width of the rendered text, in pixels</param>
	/// <param name="height">The height of the rendered text, in pixels</param>
	/// <returns><c><see langword="true"/></c>, if the dimensions were calculated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates the <paramref name="width"/> and <paramref name="height"/>, in pixels, of the area that would be occupied by the <paramref name="text"/> fully rendered with this font,
	/// without actually rendering the text.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryGetWrappedStringSize(byte*, nuint, int, out int, out int)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryGetWrappedStringSize(byte* text, nuint textLength, out int width, out int height)
		=> TryGetWrappedStringSize(text, textLength, wrapWidth: 0, out width, out height);

	/// <summary>
	/// Tries to measure how much of a given text can fit within a specified width when rendered with this font
	/// </summary>
	/// <param name="text">The UTF-16 text to measure when rendered with this font</param>
	/// <param name="maxWidth">The maximum width, in pixels, that the text can occupy, or <c>0</c> for an unbounded width</param>
	/// <param name="width">The width of the text that fits within the specified maximum width, in pixels</param>
	/// <param name="length">The number of characters of <paramref name="text"/> that fit within the specified maximum width</param>
	/// <returns><c><see langword="true"/></c> if the measurement was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates how many characters from the start of the given <paramref name="text"/> can fit within the specified <paramref name="maxWidth"/>
	/// and the actual width, in pixels, that those characters would occupy when rendered with this font.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryMeasureString(ReadOnlySpan<char> text, int maxWidth, out int width, out int length)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			bool result = TryMeasureString(textUtf8.Buffer, textUtf8.Length, maxWidth, out width, out var byteLength);

			// TODO: is this good enough for now? 
			// I mean the only real issues here are performance and that we potentially have to truncate the text if its byte length exceeds int.MaxValue.
			// Both of these issues come down to the length of the text.
			// But to be honest, how often are people actually going to use overly long strings?
			length = Encoding.UTF8.GetCharCount(textUtf8.Buffer, unchecked((int)nuint.Min(byteLength, int.MaxValue)));

			return result;
		}
	}

	/// <summary>
	/// Tries to measure how much of a given text can fit within a specified width when rendered with this font
	/// </summary>
	/// <param name="text">The UTF-8 text to measure when rendered with this font</param>
	/// <param name="maxWidth">The maximum width, in pixels, that the text can occupy, or <c>0</c> for an unbounded width</param>
	/// <param name="width">The width of the text that fits within the specified maximum width, in pixels</param>
	/// <param name="length">The number of bytes of <paramref name="text"/> that fit within the specified maximum width</param>
	/// <returns><c><see langword="true"/></c> if the measurement was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates how many bytes from the start of the given <paramref name="text"/> can fit within the specified <paramref name="maxWidth"/>
	/// and the actual width, in pixels, that those bytes as rendered characters would occupy when rendered with this font.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryMeasureString(ReadOnlySpan<byte> text, int maxWidth, out int width, out int length)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				bool result = TryMeasureString(textPtr, unchecked((nuint)text.Length), maxWidth, out width, out var byteLength);

				// Conversly to the UTF-16 overload, here we're going to just report the byte length.
				// I believe this is more useful for a ReadOnlySpan<byte> UTF-8 sequence than actual character count.
				length = unchecked((int)byteLength); // This is safe because the returned byte length won't exceed the passed in text length, which naturally fits into an int

				return result;
			}
		}
	}

	/// <summary>
	/// Tries to measure how much of a given text can fit within a specified width when rendered with this font
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to measure when rendered with this font</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="maxWidth">The maximum width, in pixels, that the text can occupy, or <c>0</c> for an unbounded width</param>
	/// <param name="width">The width of the text that fits within the specified maximum width, in pixels</param>
	/// <param name="length">The number of bytes of <paramref name="text"/> that fit within the specified maximum width</param>
	/// <returns><c><see langword="true"/></c> if the measurement was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method calculates how many bytes from the start of the given <paramref name="text"/> can fit within the specified <paramref name="maxWidth"/>
	/// and the actual width, in pixels, that those bytes as rendered characters would occupy when rendered with this font.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryMeasureString(byte* text, nuint textLength, int maxWidth, out int width, out nuint length)
	{
		unsafe
		{
			Unsafe.SkipInit(out int widthTmp);
			Unsafe.SkipInit(out nuint lengthTmp);

			bool result = TTF_MeasureString(mFont, text, textLength, maxWidth, &widthTmp, &lengthTmp);

			// TTF_MeasureString sets the width and length to 0 if it fails
			width = widthTmp;
			length = lengthTmp;

			return result;
		}
	}

	/// <summary>
	/// Tries to render a single glyph for a given Unicode code point with this font, with blended rendering
	/// </summary>
	/// <param name="glyph">The Unicode code point of the glyph to render</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the glyph with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdGlyph(Rune, Color{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedGlyph(Rune, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidGlyph(Rune, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderBlendedGlyph(Rune glyph, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			var surfacePtr = TTF_RenderGlyph_Blended(mFont, Unsafe.BitCast<Rune, uint>(glyph), foregroundColor);

			if (surfacePtr is null)
			{
				surface = default;
				return false;
			}

			surface = new(surfacePtr, register: true);
			return true;
		}
	}

	/// <summary>
	/// Tries to render a single glyph for a given UTF-16 unit with this font, with blended rendering
	/// </summary>
	/// <param name="glyph">The UTF-16 unit of the glyph to render</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the glyph with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdGlyph(char, Color{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedGlyph(char, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidGlyph(char, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderBlendedGlyph(Rune, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="glyph"/> represents a UTF-16 surrogate code point (U+D800..U+DFFF, inclusive)</exception>
	public bool TryRenderBlendedGlyph(char glyph, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderBlendedGlyph(new Rune(glyph), foregroundColor, out surface);

	/// <summary>
	/// Tries to render a single glyph for a given UTF-16 surrogate pair with this font, with blended rendering
	/// </summary>
	/// <param name="glyphHighSurrogate">The high surrogate of the UTF-16 pair</param>
	/// <param name="glyphLowSurrogate">The low surrogate of the UTF-16 pair</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the glyph with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdGlyph(char, char, Color{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedGlyph(char, char, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidGlyph(char, char, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderBlendedGlyph(Rune, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="glyphHighSurrogate"/> does not represent a UTF-16 high surrogate code point
	/// - OR -
	/// <paramref name="glyphLowSurrogate"/> does not represent a UTF-16 low surrogate code point
	/// </exception>
	public bool TryRenderBlendedGlyph(char glyphHighSurrogate, char glyphLowSurrogate, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderBlendedGlyph(new Rune(glyphHighSurrogate, glyphLowSurrogate), foregroundColor, out surface);

	/// <summary>
	/// Tries to render a single glyph for a given Unicode code point with this font, with LCD subpixel rendering
	/// </summary>
	/// <param name="glyph">The Unicode code point of the glyph to render</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="backgroundColor">The background color to render the glyph in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the glyph alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedGlyph(Rune, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedGlyph(Rune, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidGlyph(Rune, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderLcdGlyph(Rune glyph, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			var surfacePtr = TTF_RenderGlyph_LCD(mFont, Unsafe.BitCast<Rune, uint>(glyph), foregroundColor, backgroundColor);

			if (surfacePtr is null)
			{
				surface = default;
				return false;
			}

			surface = new(surfacePtr, register: true);
			return true;
		}
	}

	/// <summary>
	/// Tries to render a single glyph for a given UTF-16 unit with this font, with LCD subpixel rendering
	/// </summary>
	/// <param name="glyph">The Unicode code point of the glyph to render</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="backgroundColor">The background color to render the glyph in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the glyph alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedGlyph(char, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedGlyph(char, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidGlyph(char, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderLcdGlyph(Rune, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="glyph"/> represents a UTF-16 surrogate code point (U+D800..U+DFFF, inclusive)</exception>
	public bool TryRenderLcdGlyph(char glyph, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderLcdGlyph(new Rune(glyph), foregroundColor, backgroundColor, out surface);

	/// <summary>
	/// Tries to render a single glyph for a given UTF-16 surrogate pair with this font, with LCD subpixel rendering
	/// </summary>
	/// <param name="glyphHighSurrogate">The high surrogate of the UTF-16 surrogate pair</param>
	/// <param name="glyphLowSurrogate">The low surrogate of the UTF-16 surrogate pair</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="backgroundColor">The background color to render the glyph in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the glyph alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedGlyph(char, char, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedGlyph(char, char, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidGlyph(char, char, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderLcdGlyph(Rune, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="glyphHighSurrogate"/> does not represent a UTF-16 high surrogate code point
	/// - OR -
	/// <paramref name="glyphLowSurrogate"/> does not represent a UTF-16 low surrogate code point
	/// </exception>
	public bool TryRenderLcdGlyph(char glyphHighSurrogate, char glyphLowSurrogate, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderLcdGlyph(new Rune(glyphHighSurrogate, glyphLowSurrogate), foregroundColor, backgroundColor, out surface);

	/// <summary>
	/// Tries to render a single glyph for a given Unicode code point with this font, with high-quality palettized rendering
	/// </summary>
	/// <param name="glyph">The Unicode code point of the glyph to render</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="backgroundColor">The background color to render the glyph in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the glyph onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedGlyph(Rune, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdGlyph(Rune, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidGlyph(Rune, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderShadedGlyph(Rune glyph, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			var surfacePtr = TTF_RenderGlyph_Shaded(mFont, Unsafe.BitCast<Rune, uint>(glyph), foregroundColor, backgroundColor);

			if (surfacePtr is null)
			{
				surface = default;
				return false;
			}

			surface = new(surfacePtr, register: true);
			return true;
		}
	}

	/// <summary>
	/// Tries to render a single glyph for a given UTF-16 unit with this font, with high-quality palettized rendering
	/// </summary>
	/// <param name="glyph">The UTF-16 unit of the glyph to render</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="backgroundColor">The background color to render the glyph in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the glyph onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality <see cref="TryRenderBlendedGlyph(char, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdGlyph(char, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidGlyph(char, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderShadedGlyph(Rune, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="glyph"/> represents a UTF-16 surrogate code point (U+D800..U+DFFF, inclusive)</exception>
	public bool TryRenderShadedGlyph(char glyph, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderShadedGlyph(new Rune(glyph), foregroundColor, backgroundColor, out surface);

	/// <summary>
	/// Tries to render a single glyph for a given UTF-16 surrogate pair with this font, with high-quality palettized rendering
	/// </summary>
	/// <param name="glyphHighSurrogate">The high surrogate of the UTF-16 pair</param>
	/// <param name="glyphLowSurrogate">The low surrogate of the UTF-16 pair</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="backgroundColor">The background color to render the glyph in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the glyph onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedGlyph(char, char, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdGlyph(char, char, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidGlyph(char, char, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderShadedGlyph(Rune, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="glyphHighSurrogate"/> does not represent a UTF-16 high surrogate code point
	/// - OR -
	/// <paramref name="glyphLowSurrogate"/> does not represent a UTF-16 low surrogate code point
	/// </exception>
	public bool TryRenderShadedGlyph(char glyphHighSurrogate, char glyphLowSurrogate, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderShadedGlyph(new Rune(glyphHighSurrogate, glyphLowSurrogate), foregroundColor, backgroundColor, out surface);

	/// <summary>
	/// Tries to render a single glyph for a given Unicode code point with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="glyph">The Unicode code point of the glyph to render</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the glyph onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the glyph.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedGlyph(Rune, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdGlyph(Rune, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderShadedGlyph(Rune, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderSolidGlyph(Rune glyph, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			var surfacePtr = TTF_RenderGlyph_Solid(mFont, Unsafe.BitCast<Rune, uint>(glyph), foregroundColor);

			if (surfacePtr is null)
			{
				surface = default;
				return false;
			}

			surface = new(surfacePtr, register: true);
			return true;
		}
	}

	/// <summary>
	/// Tries to render a single glyph for a given UTF-16 unit with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="glyph">The UTF-16 unit of the glyph to render</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the glyph onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the glyph.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedGlyph(char, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdGlyph(char, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderShadedGlyph(char, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderSolidGlyph(Rune, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="glyph"/> represents a UTF-16 surrogate code point (U+D800..U+DFFF, inclusive)</exception>"
	public bool TryRenderSolidGlyph(char glyph, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderSolidGlyph(new Rune(glyph), foregroundColor, out surface);

	/// <summary>
	/// Tries to render a single glyph for a given UTF-16 surrogate pair with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="glyphHighSurrogate">The high surrogate of the UTF-16 pair</param>
	/// <param name="glyphLowSurrogate">The low surrogate of the UTF-16 pair</param>
	/// <param name="foregroundColor">The foreground color to render the glyph in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered glyph, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the glyph was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the glyph onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the glyph.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The specified glyph will be rendered without any padding or centering on the horizontal axis, and will be aligned normally on the vertical axis.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedGlyph(char, char, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdGlyph(char, char, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderShadedGlyph(char, char, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderSolidGlyph(Rune, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="glyphHighSurrogate"/> does not represent a UTF-16 high surrogate code point
	/// - OR -
	/// <paramref name="glyphLowSurrogate"/> does not represent a UTF-16 low surrogate code point
	/// </exception>
	public bool TryRenderSolidGlyph(char glyphHighSurrogate, char glyphLowSurrogate, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderSolidGlyph(new Rune(glyphHighSurrogate, glyphLowSurrogate), foregroundColor, out surface);

	/// <summary>
	/// Tries to render a text with this font, with blended rendering
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the text with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{char}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderBlendedString(ReadOnlySpan<char> text, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryRenderBlendedString(textUtf8.Buffer, textUtf8.Length, foregroundColor, out surface);
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with blended rendering
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the text with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{byte}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderBlendedString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryRenderBlendedString(textPtr, unchecked((nuint)text.Length), foregroundColor, out surface);
			}
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with blended rendering
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the text with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderBlendedWrappedString(byte*, nuint, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidString(byte*, nuint, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderBlendedString(byte* text, nuint textLength, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			var surfacePtr = TTF_RenderText_Blended(mFont, text, textLength, foregroundColor);

			if (surfacePtr is null)
			{
				surface = default;
				return false;
			}

			surface = new(surfacePtr, register: true);
			return true;
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with blended rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the text with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{char}, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderBlendedWrappedString(ReadOnlySpan<char> text, Color<byte> foregroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryRenderBlendedWrappedString(textUtf8.Buffer, textUtf8.Length, foregroundColor, wrapWidth, out surface);
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with blended rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the text with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{char}, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderBlendedWrappedString(ReadOnlySpan<char> text, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderBlendedWrappedString(text, foregroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with blended rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the text with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{byte}, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderBlendedWrappedString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryRenderBlendedWrappedString(textPtr, unchecked((nuint)text.Length), foregroundColor, wrapWidth, out surface);
			}
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with blended rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the text with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{byte}, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderBlendedWrappedString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderBlendedWrappedString(text, foregroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with blended rendering and wrapping enabled
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the text with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderBlendedWrappedString(byte*, nuint, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdWrappedString(byte*, nuint, Color{byte}, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(byte*, nuint, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(byte*, nuint, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderBlendedWrappedString(byte* text, nuint textLength, Color<byte> foregroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			var surfacePtr = TTF_RenderText_Blended_Wrapped(mFont, text, textLength, foregroundColor, wrapWidth);

			if (surfacePtr is null)
			{
				surface = default;
				return false;
			}

			surface = new(surfacePtr, register: true);
			return true;
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with blended rendering and wrapping enabled
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses high-quality alpha blending to render the text with the specified foreground color onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderLcdWrappedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(byte*, nuint, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderBlendedWrappedString(byte*, nuint, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderBlendedWrappedString(byte* text, nuint textLength, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderBlendedWrappedString(text, textLength, foregroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with LCD subpixel rendering
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the text alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderLcdWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderLcdString(ReadOnlySpan<char> text, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryRenderLcdString(textUtf8.Buffer, textUtf8.Length, foregroundColor, backgroundColor, out surface);
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with LCD subpixel rendering
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the text alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderLcdWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderLcdString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryRenderLcdString(textPtr, unchecked((nuint)text.Length), foregroundColor, backgroundColor, out surface);
			}
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with LCD subpixel rendering
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the text alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderLcdWrappedString(byte*, nuint, Color{byte}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedString(byte*, nuint, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidString(byte*, nuint, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderLcdString(byte* text, nuint textLength, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		var surfacePtr = TTF_RenderText_LCD(mFont, text, textLength, foregroundColor, backgroundColor);

		if (surfacePtr is null)
		{
			surface = default;
			return false;
		}

		surface = new(surfacePtr, register: true);
		return true;
	}

	/// <summary>
	/// Tries to render a text with this font, with LCD subpixel rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the text alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderLcdWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{char}, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{char}, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderLcdWrappedString(ReadOnlySpan<char> text, Color<byte> foregroundColor, Color<byte> backgroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryRenderLcdWrappedString(textUtf8.Buffer, textUtf8.Length, foregroundColor, backgroundColor, wrapWidth, out surface);
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with LCD subpixel rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the text alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderLcdWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderLcdWrappedString(ReadOnlySpan<char> text, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderLcdWrappedString(text, foregroundColor, backgroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with LCD subpixel rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the text alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderLcdWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{byte}, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{byte}, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderLcdWrappedString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, Color<byte> backgroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryRenderLcdWrappedString(textPtr, unchecked((nuint)text.Length), foregroundColor, backgroundColor, wrapWidth, out surface);
			}
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with LCD subpixel rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the text alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderLcdWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderLcdWrappedString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderLcdWrappedString(text, foregroundColor, backgroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with LCD subpixel rendering and wrapping enabled
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the text alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderLcdWrappedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(byte*, nuint, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(byte*, nuint, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(byte*, nuint, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderLcdWrappedString(byte* text, nuint textLength, Color<byte> foregroundColor, Color<byte> backgroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		var surfacePtr = TTF_RenderText_LCD_Wrapped(mFont, text, textLength, foregroundColor, backgroundColor, wrapWidth);

		if (surfacePtr is null)
		{
			surface = default;
			return false;
		}

		surface = new(surfacePtr, register: true);
		return true;
	}

	/// <summary>
	/// Tries to render a text with this font, with LCD subpixel rendering and wrapping enabled
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 32-bit ARGB surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses FreeType's LCD subpixel rendering to render the text alpha-blended with the specified foreground and background colors onto a new 32-bit ARGB surface.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(byte*, nuint, Color{byte}, out Surface?)"/>, <see cref="TryRenderShadedWrappedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(byte*, nuint, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderLcdWrappedString(byte*, nuint, Color{byte}, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderLcdWrappedString(byte* text, nuint textLength, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderLcdWrappedString(text, textLength, foregroundColor, backgroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with high-quality palettized rendering
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderShadedString(ReadOnlySpan<char> text, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryRenderShadedString(textUtf8.Buffer, textUtf8.Length, foregroundColor, backgroundColor, out surface);
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with high-quality palettized rendering
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderShadedWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderShadedString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryRenderShadedString(textPtr, unchecked((nuint)text.Length), foregroundColor, backgroundColor, out surface);
			}
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with high-quality palettized rendering
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderShadedWrappedString(byte*, nuint, Color{byte}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedString(byte*, nuint, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidString(byte*, nuint, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderShadedString(byte* text, nuint textLength, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		var surfacePtr = TTF_RenderText_Shaded(mFont, text, textLength, foregroundColor, backgroundColor);

		if (surfacePtr is null)
		{
			surface = default;
			return false;
		}

		surface = new(surfacePtr, register: true);
		return true;
	}

	/// <summary>
	/// Tries to render a text with this font, with high-quality palettized rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{char}, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{char}, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderShadedWrappedString(ReadOnlySpan<char> text, Color<byte> foregroundColor, Color<byte> backgroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryRenderShadedWrappedString(textUtf8.Buffer, textUtf8.Length, foregroundColor, backgroundColor, wrapWidth, out surface);
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with high-quality palettized rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderShadedWrappedString(ReadOnlySpan<char> text, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderShadedWrappedString(text, foregroundColor, backgroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with high-quality palettized rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderShadedWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{byte}, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{byte}, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderShadedWrappedString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, Color<byte> backgroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryRenderShadedWrappedString(textPtr, unchecked((nuint)text.Length), foregroundColor, backgroundColor, wrapWidth, out surface);
			}
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with high-quality palettized rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderShadedWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderShadedWrappedString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderShadedWrappedString(text, foregroundColor, backgroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with high-quality palettized rendering and wrapping enabled
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderShadedWrappedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(byte*, nuint, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(byte*, nuint, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(byte*, nuint, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderShadedWrappedString(byte* text, nuint textLength, Color<byte> foregroundColor, Color<byte> backgroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		var surfacePtr = TTF_RenderText_Shaded_Wrapped(mFont, text, textLength, foregroundColor, backgroundColor, wrapWidth);

		if (surfacePtr is null)
		{
			surface = default;
			return false;
		}

		surface = new(surfacePtr, register: true);
		return true;
	}

	/// <summary>
	/// Tries to render a text with this font, with high-quality palettized rendering and wrapping enabled
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="backgroundColor">The background color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with high-quality blending between the specified foreground and background colors.
	/// The resulting surface's 0-indexed palette pixel will be the specified background color, while other colors will be varying degrees of the specified foreground color blended with the background color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(byte*, nuint, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderSolidWrappedString(byte*, nuint, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderShadedWrappedString(byte*, nuint, Color{byte}, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderShadedWrappedString(byte* text, nuint textLength, Color<byte> foregroundColor, Color<byte> backgroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderShadedWrappedString(text, textLength, foregroundColor, backgroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the text.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderShadedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderSolidString(ReadOnlySpan<char> text, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryRenderSolidString(textUtf8.Buffer, textUtf8.Length, foregroundColor, out surface);
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the text.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderShadedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderSolidString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryRenderSolidString(textPtr, unchecked((nuint)text.Length), foregroundColor, out surface);
			}
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the text.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The rendered text will not be wrapped, even when encountering new line characters.
	/// The resulting surface will contain a single line of text and will be as wide as the rendered text requires.
	/// You can use <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/> instead, if you want to render text that wraps to multiple lines.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedString(byte*, nuint, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderShadedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderSolidString(byte* text, nuint textLength, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
	{
		var surfacePtr = TTF_RenderText_Solid(mFont, text, textLength, foregroundColor);

		if (surfacePtr is null)
		{
			surface = default;
			return false;
		}

		surface = new(surfacePtr, register: true);
		return true;
	}

	/// <summary>
	/// Tries to render a text with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the text.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderSolidWrappedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{char}, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderSolidWrappedString(ReadOnlySpan<char> text, Color<byte> foregroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryRenderSolidWrappedString(textUtf8.Buffer, textUtf8.Length, foregroundColor, wrapWidth, out surface);
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="text">The UTF-16 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the text.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{char}, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderShadedWrappedString(ReadOnlySpan{char}, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderSolidWrappedString(ReadOnlySpan{char}, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderSolidWrappedString(ReadOnlySpan<char> text, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderSolidWrappedString(text, foregroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the text.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderSolidWrappedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{byte}, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderShadedWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderSolidWrappedString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryRenderSolidWrappedString(textPtr, unchecked((nuint)text.Length), foregroundColor, wrapWidth, out surface);
			}
		}
	}

	/// <summary>
	/// Tries to render a text with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="text">The UTF-8 text to render</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the text.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(ReadOnlySpan{byte}, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderShadedWrappedString(ReadOnlySpan{byte}, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderSolidWrappedString(ReadOnlySpan{byte}, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TryRenderSolidWrappedString(ReadOnlySpan<byte> text, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderSolidWrappedString(text, foregroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to render a text with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="wrapWidth">The maximum width, in pixels, before the text is wrapped to a new line, or <c>0</c> if the text should only wrap on newline characters</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the text.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is wrapped to multiple lines on line endings and on word boundaries, if it would extend beyond the specified <paramref name="wrapWidth"/>.
	/// </para>
	/// <para>
	/// If <paramref name="wrapWidth"/> is <c>0</c>, the text will only wrap on newline characters.
	/// Alternatively, you can use the <see cref="TryRenderSolidWrappedString(byte*, nuint, Color{byte}, out Surface?)"/> overload instead.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(byte*, nuint, Color{byte}, int, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(byte*, nuint, Color{byte}, Color{byte}, int, out Surface?)"/>, or <see cref="TryRenderShadedWrappedString(byte*, nuint, Color{byte}, Color{byte}, int, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderSolidWrappedString(byte* text, nuint textLength, Color<byte> foregroundColor, int wrapWidth, [NotNullWhen(true)] out Surface? surface)
	{
		var surfacePtr = TTF_RenderText_Solid_Wrapped(mFont, text, textLength, foregroundColor, wrapWidth);

		if (surfacePtr is null)
		{
			surface = default;
			return false;
		}

		surface = new(surfacePtr, register: true);
		return true;
	}

	/// <summary>
	/// Tries to render a text with this font, with fast-quality solid rendering
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to render</param>
	/// <param name="textLength">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="foregroundColor">The foreground color to render the text in</param>
	/// <param name="surface">The resulting 8-bit palettized surface containing the rendered text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully rendered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method renders the text onto a new 8-bit palettized surface with the specified foreground color as the only color used to render the text.
	/// The resulting surface's 0-indexed palette pixel will be the color key, giving a transparent background, while the 1-indexed palette pixel will be the specified foreground color.
	/// </para>
	/// <para>
	/// Don't forget to <see cref="Surface.Dispose()">dispose</see> the returned surface when you're done using it.
	/// </para>
	/// <para>
	/// The text is only wrapped to multiple lines upon encountering newline characters.
	/// </para>
	/// <para>
	/// You can alternatively render with various other levels of quality using <see cref="TryRenderBlendedWrappedString(byte*, nuint, Color{byte}, out Surface?)"/>, <see cref="TryRenderLcdWrappedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>, or <see cref="TryRenderShadedWrappedString(byte*, nuint, Color{byte}, Color{byte}, out Surface?)"/>.
	/// </para>
	/// <para>
	/// This method is a convenience overload for <see cref="TryRenderSolidWrappedString(byte*, nuint, Color{byte}, int, out Surface?)"/> with "wrapWidth" set to <c>0</c>.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public unsafe bool TryRenderSolidWrappedString(byte* text, nuint textLength, Color<byte> foregroundColor, [NotNullWhen(true)] out Surface? surface)
		=> TryRenderSolidWrappedString(text, textLength, foregroundColor, wrapWidth: 0, out surface);

	/// <summary>
	/// Tries to set the language used for shaping text with this font
	/// </summary>
	/// <param name="language">A <see href="https://en.wikipedia.org/wiki/IETF_language_tag">BCP 47 language tag</see> representing the language to use for shaping text, or <c><see langword="null"/></c> to reset the language to the default</param>
	/// <returns><c><see langword="true"/></c>, if the language was successfully set; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method will update any <see cref="Text"/>s that use this font.
	/// </para>
	/// <para>
	/// This method should be only called on the thread that created the font.
	/// </para>
	/// </remarks>
	public bool TrySetLanguage(string? language)
	{
		unsafe
		{
			using var languageUtf8 = NativeStrings.FromUtf16ToUtf8(language);

			return TTF_SetFontLanguage(mFont, languageUtf8.Buffer);
		}
	}
}
