using Sdl3Sharp.Internal;
using Sdl3Sharp.Ttf.Internal;
using Sdl3Sharp.Ttf.TextEngineImplementation;
using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Gpu;
using Sdl3Sharp.Video.Rendering;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a virtual text layout that can be rendered with a <see cref="TextEngine"/>
/// </summary>
/// <remarks>
/// <para>
/// Make sure that you don't <see cref="Font.Dispose()">dispose</see> <see cref="Sdl3Sharp.Ttf.Font"/>s or <see cref="TextEngine.Dispose()">dispose</see> <see cref="TextEngine"/>s that are currently in use by a <see cref="Text"/>.
/// Always <see cref="Dispose()">dispose</see> the <see cref="Text"/> first before disposing any <see cref="Sdl3Sharp.Ttf.Font"/>s or <see cref="TextEngine"/>s that it uses.
/// </para>
/// </remarks>
public sealed partial class Text : IDisposable
{
	private interface IUnsafeConstructorDispatch;

	private static readonly ConcurrentDictionary<IntPtr, WeakReference<Text>> mKnownInstances = [];

	private unsafe TTF_Text* mText;
	private TextData? mData;
	private TextEngine? mEngine = null; // We need to keep the managed text engine alive as long as the managed text is alive.
										// That's why we store a reference to it here.
										// We don't use this reference for anything else, and rely on TextEngine.TryGetOrCreate to retrieve the managed text engine.
										// Since we kept the instance alive, it should be still registered and TextEngine.TryGetOrCreate will retrieve the correct one (the same instance).

	private unsafe Text(TTF_Text* text, TextEngine? engine, bool register)
	{
		// We don't need to worry about the ref count of the underlying TTF_Text here and in general,
		// since it seems totally unused by SDL in any meaningful way.
		// Therefore we can just treat Text objects like any other ordinary managed wrapper around an unmanaged resource.

		mText = text;
		mData = new(this);
		mEngine = engine;

		if (register)
		{
			if (text is not null)
			{
				mKnownInstances.AddOrUpdate(unchecked((IntPtr)mText), addRef, updateRef, this);
			}

			static WeakReference<Text> addRef(IntPtr text, Text newText) => new(newText);

			static WeakReference<Text> updateRef(IntPtr text, WeakReference<Text> existingTextRef, Text newText)
			{
				if (existingTextRef.TryGetTarget(out var exisitingText))
				{
#pragma warning disable IDE0079
#pragma warning disable CA1816
					GC.SuppressFinalize(exisitingText);
#pragma warning restore CA1816
#pragma warning restore IDE0079
					exisitingText.Dispose(forget: false);
				}

				existingTextRef.SetTarget(newText);

				return existingTextRef;
			}
		}
	}

	/// <exception cref="SdlException">The <see cref="Text"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	[return: NotNull]
	private unsafe static TTF_Text* ValidateText([NotNull] TTF_Text* text)
	{
		if (text is null)
		{
			[DoesNotReturn]
			static void failCouldNotCreateText() => throw new SdlException($"Could not create the {nameof(Text)}.");

			failCouldNotCreateText();
		}

		return text;
	}

	private unsafe static TTF_Text* CreateWithManagedString(string? text, Font? font, TextEngine? engine)
	{
		using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text); // textUtf8 is null if text is null

		return TTF_CreateText(engine is not null ? engine.Pointer : null, font is not null ? font.Pointer : null, textUtf8.Buffer, textUtf8.Length);
	}

	private unsafe Text(string? text, Font? font, TextEngine? engine, IUnsafeConstructorDispatch? _ = default) :
		this(ValidateText(CreateWithManagedString(text, font, engine)), engine, register: true)
	{ }

	/// <summary>
	/// Creates a new <see cref="Text"/> with the specified text
	/// </summary>
	/// <param name="text">The initial text of the <see cref="Text"/></param>
	/// <param name="font">The initial font of the <see cref="Text"/></param>
	/// <param name="engine">The initial text engine to use for rendering the <see cref="Text"/></param>
	/// <remarks>
	/// <para>
	/// This constructor should be only called on the thread that created the font and the text engine, if they are specified.
	/// </para>
	/// </remarks>
	public Text(string? text, Font? font = null, TextEngine? engine = null) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(text, font, engine, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	private unsafe static TTF_Text* CreateWithUtf16Text(ReadOnlySpan<char> text, Font? font, TextEngine? engine)
	{
		using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

		return TTF_CreateText(engine is not null ? engine.Pointer : null, font is not null ? font.Pointer : null, textUtf8.Buffer, textUtf8.Length);
	}

	private unsafe Text(ReadOnlySpan<char> text, Font? font, TextEngine? engine, IUnsafeConstructorDispatch? _ = default) :
		this(ValidateText(CreateWithUtf16Text(text, font, engine)), engine, register: true)
	{ }

	/// <summary>
	/// Creates a new <see cref="Text"/> with the specified text
	/// </summary>
	/// <param name="text">The initial UTF-16 text of the <see cref="Text"/></param>
	/// <param name="font">The initial font of the <see cref="Text"/></param>
	/// <param name="engine">The initial text engine to use for rendering the <see cref="Text"/></param>
	/// <remarks>
	/// <para>
	/// This constructor should be only called on the thread that created the font and the text engine, if they are specified.
	/// </para>
	/// </remarks>
	public Text(ReadOnlySpan<char> text, Font? font = null, TextEngine? engine = null) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(text, font, engine, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	private unsafe static TTF_Text* CreateWithUtf8Text(ReadOnlySpan<byte> text, Font? font, TextEngine? engine)
	{
		fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
		{
			return TTF_CreateText(engine is not null ? engine.Pointer : null, font is not null ? font.Pointer : null, textPtr, unchecked((nuint)text.Length));
		}
	}

	private unsafe Text(ReadOnlySpan<byte> text, Font? font, TextEngine? engine, IUnsafeConstructorDispatch? _ = default) :
		this(ValidateText(CreateWithUtf8Text(text, font, engine)), engine, register: true)
	{ }

	/// <summary>
	/// Creates a new <see cref="Text"/> with the specified text
	/// </summary>
	/// <param name="text">The initial UTF-8 text of the <see cref="Text"/></param>
	/// <param name="font">The initial font of the <see cref="Text"/></param>
	/// <param name="engine">The initial text engine to use for rendering the <see cref="Text"/></param>
	/// <remarks>
	/// <para>
	/// This constructor should be only called on the thread that created the font and the text engine, if they are specified.
	/// </para>
	/// </remarks>
	public Text(ReadOnlySpan<byte> text, Font? font = null, TextEngine? engine = null) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(text, font, engine, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <summary>
	/// Creates a new <see cref="Text"/> with the specified text
	/// </summary>
	/// <param name="text">A pointer to the initial UTF-8 text of the <see cref="Text"/></param>
	/// <param name="length">The length of the initial UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="font">The initial font of the <see cref="Text"/></param>
	/// <param name="engine">The initial text engine to use for rendering the <see cref="Text"/></param>
	/// <remarks>
	/// <para>
	/// This constructor should be only called on the thread that created the font and the text engine, if they are specified.
	/// </para>
	/// </remarks>
	public unsafe Text(byte* text, nuint length, Font? font = null, TextEngine? engine = null) :
		this(ValidateText(TTF_CreateText(engine is not null ? engine.Pointer : null, font is not null ? font.Pointer : null, text, length)), engine, register: true)
	{ }

	/// <inheritdoc/>
	~Text() => Dispose(forget: true);

	/// <summary>
	/// Gets or sets the color of this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The color of this <see cref="Text"/>, as RGBA components with each component being in the range of <c>0</c> to <c>255</c>
	/// </value>
	/// <remarks>
	/// <para>
	/// The component valus of this property are equivalent to the component values of the <see cref="ColorFloat"/> property, multiplied by <c>255</c> and rounded towards zero.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When getting or setting this property, the color of this <see cref="Text"/> could not be retrieved or set (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public Color<byte> Color
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (byte R, byte G, byte B, byte A) color);

				SdlErrorHelper.ThrowIfFailed(TTF_GetTextColor(mText, &color.R, &color.G, &color.B, &color.A), filterError: GetInvalidTextErrorMessageUtf8());

				return Unsafe.BitCast<(byte R, byte G, byte B, byte A), Color<byte>>(color);
			}
		}

		set
		{
			unsafe
			{
				SdlErrorHelper.ThrowIfFailed(TTF_SetTextColor(mText, value.R, value.G, value.B, value.A), filterError: GetInvalidTextErrorMessageUtf8());
			}
		}
	}

	/// <summary>
	/// Gets or sets the color of this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The color of this <see cref="Text"/>, as RGBA components with each component being in the range of <c>0.0</c> to <c>1.0</c>
	/// </value>
	/// <remarks>
	/// <para>
	/// The component valus of this property are equivalent to the component values of the <see cref="Color"/> property, divided by <c>255</c>.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When getting or setting this property, the color of this <see cref="Text"/> could not be retrieved or set (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public Color<float> ColorFloat
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (float R, float G, float B, float A) color);

				SdlErrorHelper.ThrowIfFailed(TTF_GetTextColorFloat(mText, &color.R, &color.G, &color.B, &color.A), filterError: GetInvalidTextErrorMessageUtf8());

				return Unsafe.BitCast<(float R, float G, float B, float A), Color<float>>(color);
			}
		}

		set
		{
			unsafe
			{
				SdlErrorHelper.ThrowIfFailed(TTF_SetTextColorFloat(mText, value.R, value.G, value.B, value.A), filterError: GetInvalidTextErrorMessageUtf8());
			}
		}
	}

	/// <summary>
	/// Gets or sets the direction used for text shaping of this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The direction used for text shaping of this <see cref="Text"/>
	/// </value>
	/// <remarks>
	/// This property should only be accessed from the thread that created the text.
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the direction of this <see cref="Text"/> could not be set (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public Direction Direction
	{
		get
		{
			unsafe
			{
				return TTF_GetTextDirection(mText);
			}
		}

		set
		{
			unsafe
			{
				SdlErrorHelper.ThrowIfFailed(TTF_SetTextDirection(mText, value), filterError: GetInvalidTextErrorMessageUtf8());
			}
		}
	}

	/// <summary>
	/// Gets or sets the current text engine used by this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The current text engine used by this <see cref="Text"/>, or <c><see langword="null"/></c> if no text engine is associated
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will be <c><see langword="null"/></c>, if this <see cref="Text"/> is not associated with an <see cref="TextEngine"/> at the moment.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the text engine of this <see cref="Text"/> could not be set (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public TextEngine? Engine
	{
		get
		{
			unsafe
			{
				TextEngine.TryGetOrCreate(TTF_GetTextEngine(mText), out var engine);

				mEngine = engine; // just to make sure that we keep the correct text engine alive, in case it wasn't already

				return engine;
			}
		}

		set
		{
			unsafe
			{
				bool success = TTF_SetTextEngine(mText, value is not null ? value.Pointer : null);

				if (success)
				{
					mEngine = value; // we need to keep the text engine alive as long as this text is alive
				}

				SdlErrorHelper.ThrowIfFailed(success, filterError: GetInvalidTextErrorMessageUtf8());
			}
		}
	}

	/// <summary>
	/// Gets or sets the current font used by this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The current font used by this <see cref="Text"/>, or <c><see langword="null"/></c> if no font is associated
	/// </value>
	/// <remarks>
	/// <para>
	/// When the font of a <see cref="Text"/> is changed, the text will be automatically regenerated with the new font.
	/// </para>
	/// <para>
	/// You can set the value of this property to <c><see langword="null"/></c>.
	/// Some text engines still continue to render the <see cref="Text"/> successfully in this case,
	/// but, of course, changes to any font won't be reflected in the rendering of the text.
	/// </para>
	/// <para>
	/// Setting the value of this property may cause this text to update.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the font of this <see cref="Text"/> could not be set (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public Font? Font
	{
		get
		{
			unsafe
			{
				Font.TryGetOrCreate(TTF_GetTextFont(mText), out var font);
				return font;
			}
		}

		set
		{
			unsafe
			{
				SdlErrorHelper.ThrowIfFailed(TTF_SetTextFont(mText, value is not null ? value.Pointer : null), filterError: GetInvalidTextErrorMessageUtf8());
			}
		}
	}

	internal TextData? InternalData { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mData; }

	/// <summary>
	/// Gets or sets a value indicating whether whitespace is visible when wrapping rendered text of this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// A value indicating whether whitespace is visible when wrapping rendered text of this <see cref="Text"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// If whitespace is visible when wrapping rendered text, it will take up space for the purpose of text alignment and wrapping.
	/// </para>
	/// <para>
	/// Setting the value of this property may cause this text to update.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the wrap whitespace visibility of this <see cref="Text"/> could not be set (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public bool IsWrapWhitespaceVisible
	{
		get
		{
			unsafe
			{
				return TTF_TextWrapWhitespaceVisible(mText);
			}
		}

		set
		{
			unsafe
			{
				SdlErrorHelper.ThrowIfFailed(TTF_SetTextWrapWhitespaceVisible(mText, value), filterError: GetInvalidTextErrorMessageUtf8());
			}
		}
	}

	/// <summary>
	/// Gets the number of lines of text in this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The number of lines of text in this <see cref="Text"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of this property is <c>0</c>, it means that this <see cref="Text"/> is empty, and contains no text at all.
	/// </para>
	/// </remarks>
	public int LineCount
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				return mText is not null
					? mText->NumLines
					: 0;
			}
		}
	}

	internal unsafe TTF_Text* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mText; }

	/// <summary>
	/// Gets or sets the position of this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The position of this <see cref="Text"/>, in pixels
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is the horizontal and vertical offset of the upper-left corner of this <see cref="Text"/>, in pixels.
	/// </para>
	/// <para>
	/// This property can be used to virtually position multiple <see cref="Text"/>s relative to each other and within a single virtual text layout.
	/// </para>
	/// <para>
	/// Setting the value of this property may cause this text to update.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When getting or setting this property, the position of this <see cref="Text"/> could not be retrieved or set (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public (int X, int Y) Position
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (int X, int Y) position);

				SdlErrorHelper.ThrowIfFailed(TTF_GetTextPosition(mText, &position.X, &position.Y), filterError: GetInvalidTextErrorMessageUtf8());

				return position;
			}
		}

		set
		{
			unsafe
			{
				SdlErrorHelper.ThrowIfFailed(TTF_SetTextPosition(mText, value.X, value.Y), filterError: GetInvalidTextErrorMessageUtf8());
			}
		}
	}

	/// <summary>
	/// Gets the properties associated with this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The properties associated with this <see cref="Text"/>, or <c><see langword="null"/></c> if the properties could not be retrieved successfully (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	public Properties? Properties
	{
		get
		{
			unsafe
			{
				return TTF_GetTextProperties(mText) switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		} 
	}

	/// <summary>
	/// Gets or sets the script used for text shaping of this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The script used for text shaping of this <see cref="Text"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defaults to the <see cref="Font.Script">script</see> of the <see cref="Font"/> currently associated with this <see cref="Text"/>,
	/// or to <c><see langword="default"/>(<see cref="Sdl3Sharp.Ttf.Script"/>)</c> if no font is associated or the <see cref="Font.Script">script</see> of the associated <see cref="Font"/> was not set.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the script of this <see cref="Text"/> could not be set (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public Script Script
	{
		get
		{
			unsafe
			{
				return TTF_GetTextScript(mText);
			}
		}

		set
		{
			unsafe
			{
				SdlErrorHelper.ThrowIfFailed(TTF_SetTextScript(mText, value), filterError: GetInvalidTextErrorMessageUtf8());
			}
		}
	}

	/// <summary>
	/// Gets the size of the rendered text of this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The size of the rendered text of this <see cref="Text"/>, in pixels
	/// </value>
	/// <remarks>
	/// <para>
	/// This value of this property is the width and height of the rendered text of this <see cref="Text"/>, in pixels.
	/// It may change when the associated <see cref="Font"/> or font <see cref="Font.Style">style</see> and <see cref="Font.Size">size</see> change.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// Tthe size of the rendered text of this <see cref="Text"/> could not be retrieved (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public (int Width, int Height) Size
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (int Width, int Height) size);

				SdlErrorHelper.ThrowIfFailed(TTF_GetTextSize(mText, &size.Width, &size.Height), filterError: GetInvalidTextErrorMessageUtf8());

				return size;
			}
		}
	}

	/// <summary>
	/// Gets or sets the current text of this <see cref="Text"/>
	/// </summary>
	/// <value>
	/// The current text of this <see cref="Text"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// This property allows you to modify the text of this <see cref="Text"/> object directly.
	/// It also allows you to set the text to <c><see langword="null"/></c>, which will make this <see cref="Text"/> object empty.
	/// Alternatively, you can use the <see cref="TryAddString(ReadOnlySpan{char})"/>, <see cref="TryInsertString(int, ReadOnlySpan{char}, bool)"/>, or <see cref="TryRemoveString(int, int, bool)"/> methods to modify the text.
	/// </para>
	/// <para>
	/// Setting the value of this property may cause this text to update.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the text of this <see cref="Text"/> could not be set (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public string? String
	{
		get
		{
			unsafe
			{
				if (mText is null)
				{
					return null;
				}

				using var textUtf16 = NativeStrings.FromUtf8ToUtf16(mText->Text);

				return textUtf16.ToManaged();
			}
		}

		set
		{
			unsafe
			{
				if (value is null)
				{
					SdlErrorHelper.ThrowIfFailed(TTF_SetTextString(mText, null, 0), filterError: GetInvalidTextErrorMessageUtf8());
				}
				else
				{
					using var textUtf8 = NativeStrings.FromUtf16ToUtf8(value);

					SdlErrorHelper.ThrowIfFailed(TTF_SetTextString(mText, textUtf8.Buffer, textUtf8.Length), filterError: GetInvalidTextErrorMessageUtf8());
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the maximum width of the rendered text of this <see cref="Text"/> before wrapping occurs
	/// </summary>
	/// <value>
	/// The maximum width of the rendered text of this <see cref="Text"/>, in pixels, before wrapping occurs, or <c>0</c> to only wrap on newline characters
	/// </value>
	/// <remarks>
	/// <para>
	/// Setting the value of this property may cause this text to update.
	/// </para>
	/// <para>
	/// This property should only be accessed from the thread that created the text.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When getting or setting this property, the wrap width of this <see cref="Text"/> could not be retrieved or set (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public int WrapWidth
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out int wrapWidth);

				SdlErrorHelper.ThrowIfFailed(TTF_GetTextWrapWidth(mText, &wrapWidth), filterError: GetInvalidTextErrorMessageUtf8());

				return wrapWidth;
			}
		}

		set
		{
			unsafe
			{
				SdlErrorHelper.ThrowIfFailed(TTF_SetTextWrapWidth(mText, value), filterError: GetInvalidTextErrorMessageUtf8());
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
			// We don't need to worry about the ref count of the underlying TTF_Text here and in general,
			// since it seems totally unused by SDL in any meaningful way.
			// Therefore we can just treat Text objects like any other ordinary managed wrapper around an unmanaged resource.

			if (mText is not null)
			{
				if (forget)
				{
					mKnownInstances.TryRemove(unchecked((IntPtr)mText), out _);
				}

				TTF_DestroyText(mText);
				
				mText = null;
			}

			mData = null;
			mEngine = null; // there's no issue with potentially letting the text engine die after the text is disposed
		}
	}

	/// <summary>
	/// Tries to append the specified string to the end of the text of this <see cref="Text"/>
	/// </summary>
	/// <param name="text">The UTF-16 text to append</param>
	/// <returns><c><see langword="true"/></c>, if the string was successfully appended; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method may cause this text to update.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryAddString(ReadOnlySpan<char> text)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryAddString(textUtf8.Buffer, textUtf8.Length);
		}
	}

	/// <summary>
	/// Tries to append the specified string to the end of the text of this <see cref="Text"/>
	/// </summary>
	/// <param name="text">The UTF-8 text to append</param>
	/// <returns><c><see langword="true"/></c>, if the string was successfully appended; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method may cause this text to update.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryAddString(ReadOnlySpan<byte> text)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryAddString(textPtr, (nuint)text.Length);
			}
		}
	}

	/// <summary>
	/// Tries to append the specified string to the end of the text of this <see cref="Text"/>
	/// </summary>
	/// <param name="text">A pointer to the UTF-8 text to append</param>
	/// <param name="length">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <returns><c><see langword="true"/></c>, if the string was successfully appended; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method may cause this text to update.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public unsafe bool TryAddString(byte* text, nuint length)
	{
		return TTF_AppendTextString(Pointer, text, length);
	}

	/// <summary>
	/// Tries to draw the rendered text of this <see cref="Text"/> to with a <see cref="Renderer"/> using a <see cref="RendererTextEngine"/>
	/// </summary>
	/// <param name="x">The X coordinate, in pixels, where the text should be drawn</param>
	/// <param name="y">The Y coordinate, in pixels, where the text should be drawn</param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully drawn; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The current <see cref="TextEngine"/> of this <see cref="Text"/> must be a <see cref="RendererTextEngine"/> for this method to succeed; otherwise, this method will fail and return <c><see langword="false"/></c>.
	/// This method will use the <see cref="Renderer"/> associated with the current <see cref="RendererTextEngine"/> of this <see cref="Text"/> and its current rendering target for drawing the text.
	/// </para>
	/// <para>
	/// The horizontal <paramref name="x"/> coordinate is positive from the left edge of the rendering target towards the right,
	/// and the vertical <paramref name="y"/> coordinate is positive from the top edge of the rendering target towards the bottom.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryDrawToRenderer(float x, float y)
	{
		unsafe
		{
			return TTF_DrawRendererText(mText, x, y);
		}
	}

	/// <summary>
	/// Tries to draw the rendered text of this <see cref="Text"/> to a <see cref="Surface"/> using a <see cref="SurfaceTextEngine"/>
	/// </summary>
	/// <param name="surface">The <see cref="Surface"/> to draw the text on</param>
	/// <param name="x">The X coordinate, in pixels, where the text should be drawn</param>
	/// <param name="y">The Y coordinate, in pixels, where the text should be drawn</param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully drawn; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The current <see cref="TextEngine"/> of this <see cref="Text"/> must be a <see cref="SurfaceTextEngine"/> for this method to succeed; otherwise, this method will fail and return <c><see langword="false"/></c>.
	/// This method will draw the text directly onto the specified <see cref="Surface"/> using the current <see cref="SurfaceTextEngine"/>.
	/// </para>
	/// <para>
	/// The horizontal <paramref name="x"/> coordinate is positive from the left edge of the surface towards the right,
	/// and the vertical <paramref name="y"/> coordinate is positive from the top edge of the surface towards the bottom.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryDrawToSurface(Surface surface, int x, int y)
	{
		unsafe
		{
			return TTF_DrawSurfaceText(mText, x, y, surface is not null ? surface.Pointer : null);
		}
	}

#if SDL_TTF3_3_0_OR_GREATER

	/// <summary>
	/// Tries to get the geometry data needed for drawing the text of this <see cref="Text"/> using OpenGL from a <see cref="GLTextEngine"/>
	/// </summary>
	/// <param name="glDrawData">The OpenGL geometry data representing the rendered text of this <see cref="Text"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="GLAtlasDrawSequenceEnumerable"/>)</c></param>
	/// <returns><c><see langword="true"/></c>, if the OpenGL geometry data was successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method enables you to draw text using OpenGL. You have to render the resulting geometry yourself using the appropriate OpenGL APIs.
	/// </para>
	/// <para>
	/// This method will return <c><see langword="false"/></c> if the text of this <see cref="Text"/> is empty and contains no text at all.
	/// </para>
	/// <para>
	/// The current <see cref="TextEngine"/> of this <see cref="Text"/> must be a <see cref="GLTextEngine"/> for this method to succeed; otherwise, this method will fail and return <c><see langword="false"/></c>.
	/// This method will use the <see cref="GLTextEngine"/> associated with the current <see cref="GLTextEngine"/> of this <see cref="Text"/> for constructing the OpenGL geometry data.
	/// </para>
	/// <para>
	/// The horizontal X-axis is taken positive towards the right, and the vertical Y-axis is taken positive upwards for both the vertex and the texture coordinates.
	/// This follows the same convention as any other of SDL's OpenGL related APIs.
	/// If you want to use a different coordinate system, you'll have to transform the resulting vertices yourself.
	/// </para>
	/// <para>
	/// If the text looks blocky, try to use linear filtering.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryGetGLDrawData(out GLAtlasDrawSequenceEnumerable glDrawData)
	{
		unsafe
		{
			var sequence = TTF_GetGLTextDrawData(mText);

			if (sequence is null)
			{
				glDrawData = default;
				return false;
			}

			glDrawData = new(sequence);
			return true;
		}
	}

#endif

	/// <summary>
	/// Tries to get the geometry data needed for drawing the text of this <see cref="Text"/> using the <see cref="GpuDevice">GPU</see> from a <see cref="GpuTextEngine"/>
	/// </summary>
	/// <param name="gpuDrawData">The GPU geometry data representing the rendered text of this <see cref="Text"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="GpuAtlasDrawSequenceEnumerable"/>)</c></param>
	/// <returns><c><see langword="true"/></c>, if the GPU geometry data was successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method enables you to draw text as part of your GPU rendering pipeline. You have to render the resulting geometry yourself using the appropriate GPU APIs.
	/// </para>
	/// <para>
	/// This method will return <c><see langword="false"/></c> if the text of this <see cref="Text"/> is empty and contains no text at all.
	/// </para>
	/// <para>
	/// The current <see cref="TextEngine"/> of this <see cref="Text"/> must be a <see cref="GpuTextEngine"/> for this method to succeed; otherwise, this method will fail and return <c><see langword="false"/></c>.
	/// This method will use the <see cref="GpuDevice"/> associated with the current <see cref="GpuTextEngine"/> of this <see cref="Text"/> for constructing the GPU geometry data.
	/// </para>
	/// <para>
	/// The horizontal X-axis is taken positive towards the right, and the vertical Y-axis is taken positive upwards for both the vertex and the texture coordinates.
	/// This follows the same convention as any other of SDL's GPU related APIs.
	/// If you want to use a different coordinate system, you'll have to transform the resulting vertices yourself.
	/// </para>
	/// <para>
	/// If the text looks blocky, try to use linear filtering.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryGetGpuDrawData(out GpuAtlasDrawSequenceEnumerable gpuDrawData)
	{
		unsafe
		{
			var sequence = TTF_GetGPUTextDrawData(mText);

			if (sequence is null)
			{
				gpuDrawData = default;
				return false;
			}

			gpuDrawData = new(sequence);
			return true;
		}
	}

	internal unsafe static bool TryGetOrCreate(TTF_Text* text, TextEngine? engine, [NotNullWhen(true)] out Text? result)
	{
		// We don't need to worry about the ref count of the underlying TTF_Text here and in general,
		// since it seems totally unused by SDL in any meaningful way.
		// Therefore we can just treat Text objects like any other ordinary managed wrapper around an unmanaged resource.

		if (text is null)
		{
			result = null;
			return false;
		}

		var textRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)text), createRef, engine);

		if (!textRef.TryGetTarget(out result))
		{
			textRef.SetTarget(result = create(text, engine));
		}

		return true;

		static WeakReference<Text> createRef(IntPtr text, TextEngine? engine) => new(create(unchecked((TTF_Text*)text), engine));

		static Text create(TTF_Text* text, TextEngine? engine) => new(text, engine, register: false);
	}

	/// <summary>
	/// Tries to get the next substring of this <see cref="Text"/> for a specified predecessor substring
	/// </summary>
	/// <param name="subString">The substring of this <see cref="Text"/> preceding the substring to retrieve</param>
	/// <param name="nextSubString">The next substring of this <see cref="Text"/> following the specified <paramref name="subString"/>; otherwise, <c><see langword="default"/>(<see cref="SubString"/>)</c>, if <paramref name="subString"/> doesn't reference the same <see cref="SubString"/> as <paramref name="nextSubString"/> does</param>
	/// <returns><c><see langword="true"/></c>, if the next substring was successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If this method is called at the end of the text, it will return a <paramref name="nextSubString"/> with a <see cref="SubString.Length">length</see> of <c>0</c> and the <see cref="SubString.ContainsTextEnd"/> flag set.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryGetNextSubString(ref readonly SubString subString, out SubString nextSubString)
	{
		unsafe
		{
			fixed (SubString* subStringPtr = &subString, nextSubStringPtr = &nextSubString)
			{
				return TTF_GetNextTextSubString(mText, subStringPtr, nextSubStringPtr);
			}
		}
	}

	/// <summary>
	/// Tries to get the previous substring of this <see cref="Text"/> for a specified successor substring
	/// </summary>
	/// <param name="subString">The substring of this <see cref="Text"/> following the substring to retrieve</param>
	/// <param name="previousSubString">The previous substring of this <see cref="Text"/> preceding the specified <paramref name="subString"/>; otherwise, <c><see langword="default"/>(<see cref="SubString"/>)</c>, if <paramref name="subString"/> doesn't reference the same <see cref="SubString"/> as <paramref name="previousSubString"/> does</param>
	/// <returns><c><see langword="true"/></c>, if the previous substring was successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If this method is called at the beginning of the text, it will return a <paramref name="previousSubString"/> with a <see cref="SubString.Length">length</see> of <c>0</c> and the <see cref="SubString.ContainsTextStart"/> flag set.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryGetPreviousSubString(ref readonly SubString subString, out SubString previousSubString)
	{
		unsafe
		{
			fixed (SubString* subStringPtr = &subString, previousSubStringPtr = &previousSubString)
			{
				return TTF_GetPreviousTextSubString(mText, subStringPtr, previousSubStringPtr);
			}
		}
	}

	/// <summary>
	/// Tries to get the substring of this <see cref="Text"/> at a specified offset
	/// </summary>
	/// <param name="offset">The offset into the text</param>
	/// <param name="subString">The substring at the specified offset, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="SubString"/>)</c></param>
	/// <param name="offsetInUtf8Bytes">An optional value indicating whether the specified <paramref name="offset"/> is given in UTF-8 bytes rather than UTF-16 characters. Defaults to <c><see langword="false"/></c>, meaning the offset is given in UTF-16 characters by default.</param>
	/// <returns><c><see langword="true"/></c>, if the substring was successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If the <paramref name="offset"/> is less than <c>0</c>, this method will return a <paramref name="subString"/> with a <see cref="SubString.Length">length</see> of <c>0</c> and the <see cref="SubString.ContainsTextStart"/> flag set.
	/// </para>
	/// <para>
	/// If the <paramref name="offset"/> is greater than the length of the text, this method will return a <paramref name="subString"/> with a <see cref="SubString.Length">length</see> of <c>0</c> and the <see cref="SubString.ContainsTextEnd"/> flag set.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryGetSubStringForOffset(int offset, out SubString subString, bool offsetInUtf8Bytes = false)
	{
		unsafe
		{
			if (!offsetInUtf8Bytes)
			{
				// offset is given in UTF-16 code units in this case, so we must translate it into UTF-8 byte offset first.

				// To mirror the behavior of the original SDL implementation of TTF_GetTextSubstring, we fail with the appropriate error message set.
				// That's to prevent a call to the expensive TryGetUtf8OffsetAndLength method in the cases where it's not really needed.
				if (mText is null)
				{
					subString = default;
					return Error.Set(GetInvalidTextErrorMessageUtf16());
				}

				if (!NativeStringHelpers.TryGetUtf8OffsetAndLength(Pointer->Text, offset, utf16Length: 0, out offset, out _))
				{
					subString = default;
					return false;
				}
			}

			// offset is guaranteed to be in UTF-8 bytes at this point, so we can just call the native method directly.
			fixed (SubString* subStringPtr = &subString)
			{
				return TTF_GetTextSubString(mText, offset, subStringPtr);
			}
		}
	}

	/// <summary>
	/// Tries to get the substring of this <see cref="Text"/> that contains the specified line of text
	/// </summary>
	/// <param name="line">The zero-based line index of the text; usually in the range from inclusive <c>0</c> to exclusive <see cref="LineCount"/></param>
	/// <param name="subString">The substring that contains the specified line of text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="SubString"/>)</c></param>
	/// <returns><c><see langword="true"/></c>, if the substring was successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If the specified <paramref name="line"/> is less than <c>0</c>, this method will return a <paramref name="subString"/> with a <see cref="SubString.Length">length</see> of <c>0</c> and the <see cref="SubString.ContainsTextStart"/> flag set.
	/// </para>
	/// <para>
	/// If the specified <paramref name="line"/> is greater than or equal to the <see cref="LineCount"/> of this <see cref="Text"/>, this method will return a <paramref name="subString"/> with a <see cref="SubString.Length">length</see> of <c>0</c> and the <see cref="SubString.ContainsTextEnd"/> flag set.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryGetSubStringForLine(int line, out SubString subString)
	{
		unsafe
		{
			fixed (SubString* subStringPtr = &subString)
			{
				return TTF_GetTextSubStringForLine(mText, line, subStringPtr);
			}
		}
	}

	/// <summary>
	/// Tries to get the substring of this <see cref="Text"/> that is closest to the specified point
	/// </summary>
	/// <param name="x">The X coordinate of the point</param>
	/// <param name="y">The Y coordinate of the point</param>
	/// <param name="subString">The substring that is closest to the specified point, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="SubString"/>)</c></param>
	/// <returns><c><see langword="true"/></c>, if the substring was successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method tries to find the substring of this <see cref="Text"/> that is closest to the specified point, containing the point if possible.
	/// </para>
	/// <para>
	/// The horizontal <paramref name="x"/> coordinate is relative to the left edge of the text and may be outside of the bounds of the text area,
	/// and the vertical <paramref name="y"/> coordinate is relative to the top edge of the text and may also be outside of the bounds of the text area.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryGetSubStringForPoint(int x, int y, out SubString subString)
	{
		unsafe
		{
			fixed (SubString* subStringPtr = &subString)
			{
				return TTF_GetTextSubStringForPoint(mText, x, y, subStringPtr);
			}
		}
	}

	// We keep the claim from the official SDL docs, that it checks for length == -1, even though it actually checks for length < 0
	/// <summary>
	/// Tries to get the substrings of this <see cref="Text"/> that contain the specified range of text
	/// </summary>
	/// <param name="offset">The offset into the text</param>
	/// <param name="length">The length of the range of text, or <c>-1</c> for the remainder of the text</param>
	/// <param name="subStrings">The substrings that contain the specified range of text, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="SubStringCollection"/>)</c></param>
	/// <param name="offsetAndLengthInUtf8Bytes">An optional value indicating whether the specified <paramref name="offset"/> and <paramref name="length"/> are given in UTF-8 bytes rather than UTF-16 characters. Defaults to <c><see langword="false"/></c>, meaning the offset and length are given in UTF-16 characters by default.</param>
	/// <returns><c><see langword="true"/></c>, if the substrings were successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryGetSubStringsForRange(int offset, int length, out SubStringCollection subStrings, bool offsetAndLengthInUtf8Bytes = false)
	{
		unsafe
		{
			if (!offsetAndLengthInUtf8Bytes)
			{
				// offset and length are given in UTF-16 code units in this case, so we must translate them into UTF-8 byte offset and length first.

				// To mirror the behavior of the original SDL implementation of TTF_GetTextSubStringsForRange, we fail with the appropriate error message set
				// That's to prevent a call to the expensive TryGetUtf8OffsetAndLength method in the cases where it's not really needed.
				if (mText is null)
				{
					subStrings = default;
					return Error.Set(GetInvalidTextErrorMessageUtf16());
				}

				if (length is < 0)
				{
					// If the length is specified to be the remainder of the text, we don't change that when converting into UTF-8 bytes.
					// Therefore we just need to convert the offset.
					// (Fun fact: the original SDL implementation of TTF_GetTextSubStringsForRange does not just check for length == -1, as the official docs claim, but it checks for length < 0 instead.
					// So we have to mirror that behavior as well.)
					if (!NativeStringHelpers.TryGetUtf8OffsetAndLength(mText->Text, offset, utf16Length: 0, out offset, out _))
					{
						subStrings = default;
						return false;
					}
				}
				else
				{
					// We need to convert both the offset and the length in this case.
					if (!NativeStringHelpers.TryGetUtf8OffsetAndLength(mText->Text, offset, length, out offset, out length))
					{
						subStrings = default;
						return false;
					}
				}
			}

			// offset and length are guaranteed to be in UTF-8 bytes at this point, so we can just proceed with calling the native method and copying the results into managed memory.
			Unsafe.SkipInit(out int count);
			var subStringsPtr = TTF_GetTextSubStringsForRange(mText, offset, length, &count);

			if (subStringsPtr is null)
			{
				subStrings = default;
				return false;
			}

			try
			{
				subStrings = new(subStringsPtr, count);
				return true;
			}
			finally
			{
				// Official SDL docs ask to free the resulting array from TTF_GetTextSubStringsForRange, as a single alloc.
				NativeMemory.Free(subStringsPtr);
			}
		}
	}

	/// <summary>
	/// Tries to insert the specified string into the text of this <see cref="Text"/> at a specified offset
	/// </summary>
	/// <param name="offset">The offset at which to insert the string. Can be negative. Non-negative offsets are counted from the beginning of the text, while negative offsets are counted from the end of the text.</param>
	/// <param name="text">The UTF-16 text to insert</param>
	/// <param name="offsetInUtf8Bytes">An optional value indicating whether the specified <paramref name="offset"/> is given in UTF-8 bytes rather than UTF-16 characters. Defaults to <c><see langword="false"/></c>, meaning the offset is given in UTF-16 characters by default.</param>
	/// <returns><c><see langword="true"/></c>, if the string was successfully inserted; otherwise, <c>false</c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If the <paramref name="offset"/> is given in UTF-16 characters (<paramref name="offsetInUtf8Bytes"/> is <c><see langword="false"/></c>, which is the default),
	/// then this method will perform additional validation to ensure that you don't insert text in the middle of a UTF-16 surrogate pair, and a call to this method will fail and return <c><see langword="false"/></c> in that case. 
	/// Otherwise, if the <paramref name="offset"/> is given in UTF-8 bytes (<paramref name="offsetInUtf8Bytes"/> is <c><see langword="true"/></c>),
	/// then there's no additional UTF-8 validation performed, and you must make sure yourself that you only insert at valid UTF-8 code point boundaries, otherwise the text may become corrupted.
	/// </para>
	/// <para>
	/// Calling this method may cause this text to update.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryInsertString(int offset, ReadOnlySpan<char> text, bool offsetInUtf8Bytes = false)
	{
		unsafe
		{
			using var textUtf8 = NativeStrings.FromUtf16ToUtf8(text);

			return TryInsertString(offset, textUtf8.Buffer, textUtf8.Length, offsetInUtf8Bytes);
		}
	}

	/// <summary>
	/// Tries to insert the specified string into the text of this <see cref="Text"/> at a specified offset
	/// </summary>
	/// <param name="offset">The offset at which to insert the string. Can be negative. Non-negative offsets are counted from the beginning of the text, while negative offsets are counted from the end of the text.</param>
	/// <param name="text">The UTF-8 text to insert</param>
	/// <param name="offsetInUtf8Bytes">An optional value indicating whether the specified <paramref name="offset"/> is given in UTF-8 bytes rather than UTF-16 characters. Defaults to <c><see langword="false"/></c>, meaning the offset is given in UTF-16 characters by default.</param>
	/// <returns><c><see langword="true"/></c>, if the string was successfully inserted; otherwise, <c>false</c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If the <paramref name="offset"/> is given in UTF-16 characters (<paramref name="offsetInUtf8Bytes"/> is <c><see langword="false"/></c>, which is the default),
	/// then this method will perform additional validation to ensure that you don't insert text in the middle of a UTF-16 surrogate pair, and a call to this method will fail and return <c><see langword="false"/></c> in that case. 
	/// Otherwise, if the <paramref name="offset"/> is given in UTF-8 bytes (<paramref name="offsetInUtf8Bytes"/> is <c><see langword="true"/></c>),
	/// then there's no additional UTF-8 validation performed, and you must make sure yourself that you only insert at valid UTF-8 code point boundaries, otherwise the text may become corrupted.
	/// </para>
	/// <para>
	/// Calling this method may cause this text to update.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryInsertString(int offset, ReadOnlySpan<byte> text, bool offsetInUtf8Bytes = false)
	{
		unsafe
		{
			fixed (byte* textPtr = NativeStringHelpers.NullTerminateUtf8IfEmpty(text))
			{
				return TryInsertString(offset, textPtr, (nuint)text.Length, offsetInUtf8Bytes);
			}
		}
	}

	/// <summary>
	/// Tries to insert the specified string into the text of this <see cref="Text"/> at a specified offset
	/// </summary>
	/// <param name="offset">The offset at which to insert the string. Can be negative. Non-negative offsets are counted from the beginning of the text, while negative offsets are counted from the end of the text.</param>
	/// <param name="text">A pointer to the UTF-8 text to insert</param>
	/// <param name="length">The length of the UTF-8 text, in bytes, or <c>0</c> if the text is null-terminated</param>
	/// <param name="offsetInUtf8Bytes">An optional value indicating whether the specified <paramref name="offset"/> is given in UTF-8 bytes rather than UTF-16 characters. Defaults to <c><see langword="false"/></c>, meaning the offset is given in UTF-16 characters by default.</param>
	/// <returns><c><see langword="true"/></c>, if the string was successfully inserted; otherwise, <c>false</c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If the <paramref name="offset"/> is given in UTF-16 characters (<paramref name="offsetInUtf8Bytes"/> is <c><see langword="false"/></c>, which is the default),
	/// then this method will perform additional validation to ensure that you don't insert text in the middle of a UTF-16 surrogate pair, and a call to this method will fail and return <c><see langword="false"/></c> in that case. 
	/// Otherwise, if the <paramref name="offset"/> is given in UTF-8 bytes (<paramref name="offsetInUtf8Bytes"/> is <c><see langword="true"/></c>),
	/// then there's no additional UTF-8 validation performed, and you must make sure yourself that you only insert at valid UTF-8 code point boundaries, otherwise the text may become corrupted.
	/// </para>
	/// <para>
	/// Calling this method may cause this text to update.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public unsafe bool TryInsertString(int offset, byte* text, nuint length, bool offsetInUtf8Bytes = false)
	{
		unsafe
		{
			if (!offsetInUtf8Bytes)
			{
				// offset is given in UTF-16 code units in this case, so we must translate it into UTF-8 byte offset first.

				// To mirror the behavior of the original SDL implementation of TTF_InsertTextString, we fail with the appropriate error message set.
				// That's to prevent a call to the expensive TryGetUtf8OffsetAndLength method in the cases where it's not really needed.
				if (mText is null)
				{
					return Error.Set(GetInvalidTextErrorMessageUtf16());
				}

				if (mText->Text is not null)
				{
					// The original TTF_InsertTextString has an early on delegates to TTF_SetTextString, in the case the underlying text is not initialized.
					// Of course, that ignores the given offset in that case, so we can also save on computation by skipping the conversion of the offset to UTF-8 byte in this case.

					if (!NativeStringHelpers.TryGetUtf8OffsetAndLength(Pointer->Text, offset, utf16Length: 0, out offset, out _))
					{
						return false;
					}
				}
			}

			// offset is guaranteed to be in UTF-8 bytes at this point (if it's even needed at all), so we can just call the native method directly.
			return TTF_InsertTextString(mText, offset, text, length);
		}
	}

	// We keep the claim from the official SDL docs, that it checks for length == -1, even though it actually checks for length < 0
	/// <summary>
	/// Tries to remove a range of text from this <see cref="Text"/> at a specified offset
	/// </summary>
	/// <param name="offset">The offset at which to remove the string. Can be negative. Non-negative offsets are counted from the beginning of the text, while negative offsets are counted from the end of the text.</param>
	/// <param name="length">The length of the range of text, or <c>-1</c> for the remainder of the text</param>
	/// <param name="offsetAndLengthInUtf8Bytes">An optional value indicating whether the specified <paramref name="offset"/> and <paramref name="length"/> are given in UTF-8 bytes rather than UTF-16 characters. Defaults to <c><see langword="false"/></c>, meaning the offset and length are given in UTF-16 characters by default.</param>
	/// <returns><c><see langword="true"/></c>, if the string was successfully removed; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If the <paramref name="offset"/> and <paramref name="length"/> are given in UTF-16 characters (<paramref name="offsetAndLengthInUtf8Bytes"/> is <c><see langword="false"/></c>, which is the default),
	/// then this method will perform additional validation to ensure that you don't remove text starting in the middle of a UTF-16 surrogate pair, and a call to this method will fail and return <c><see langword="false"/></c> in that case. 
	/// Otherwise, if the <paramref name="offset"/> and <paramref name="length"/> are given in UTF-8 bytes (<paramref name="offsetAndLengthInUtf8Bytes"/> is <c><see langword="true"/></c>),
	/// then there's no additional UTF-8 validation performed, and you must make sure yourself that you only remove text from valid UTF-8 code point boundaries, otherwise the text may become corrupted.
	/// </para>
	/// <para>
	/// Calling this method may cause this text to update.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryRemoveString(int offset, int length, bool offsetAndLengthInUtf8Bytes = false)
	{
		unsafe
		{
			if (!offsetAndLengthInUtf8Bytes)
			{
				// offset and length are given in UTF-16 code units in this case, so we must translate them into UTf-8 byte offset and length first.

				// To mirror the behavior of the original SDL implementation of TTF_DeleteTextString, we fail with the appropriate error message set
				if (mText is null)
				{
					return Error.Set(GetInvalidTextErrorMessageUtf16());
				}

				// Again, to mirror the behavior of the original SDL implementation, we do the checks shown below manually ourselves beforehand.
				// That's to prevent a call to the expensive TryGetUtf8OffsetAndLength method in the cases where it's not really needed.
				if (length is 0 || mText->Text is null)
				{
					// Suprisingly enough, the original SDL implementation of TTF_DeleteTextString returns success if the internal text is not initialized.
					return true;
				}

				if (length is < 0)
				{
					// If the length is specified to be the remainder of the text, we don't change that when converting into UTF-8 bytes.
					// Therefore we just need to convert the offset.
					// (Fun fact: the original SDL implementation of TTF_DeleteTextString does not just check for length == -1, as the official docs claim, but it checks for length < 0 instead.
					// So we have to mirror that behavior as well.)
					if (!NativeStringHelpers.TryGetUtf8OffsetAndLength(mText->Text, offset, utf16Length: 0, out offset, out _))
					{
						return false;
					}
				}
				else
				{
					if (!NativeStringHelpers.TryGetUtf8OffsetAndLength(mText->Text, offset, length, out offset, out length))
					{
						return false;
					}
				}
			}

			// offset and length are guaranteed to be in UTF-8 bytes at this point, so we can just call the native method directly.
			return TTF_DeleteTextString(mText, offset, length);
		}
	}

	/// <summary>
	/// Tries to update the layout of this <see cref="Text"/>
	/// </summary>
	/// <returns><c><see langword="true"/></c>, if the layout was successfully updated; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Typically, you don't need to call this method manually, since that is done automatically whenever the layout of the text is requested or the text is rendered.
	/// However, you can call this method manually, if you need more control over the timing of when the layout and text engine updates happen.
	/// </para>
	/// <para>
	/// This method should only be called from the thread that created the text.
	/// </para>
	/// </remarks>
	public bool TryUpdate()
	{
		unsafe
		{
			return TTF_UpdateText(mText);
		}
	}
}
