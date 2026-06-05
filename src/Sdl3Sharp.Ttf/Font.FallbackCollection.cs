using Sdl3Sharp.Internal;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

partial class Font
{
	// It's highly debatable whether this should be a reference type, a value type, or a even a "ref struct".
	// That's because of its 1-to-1 mapping to "Font", its immutability and the fact it doesn't store any additional data, and its nature as just being used to
	// delegate calls to the underlying native TTF_Font* instance.
	// The only reason for its existence is to be used in object initializers for "Font" (e.g. "new Font { Fallbacks = { ... } }"). Well, it also adds a bit of separation.
	// I think it's best to make it a class, because the term "Collection" gives me the impression of it being a reference type.
	// Also, we literally just creating an instance of it in the "Font" constructor to be wired to the lifetime of that particular "Font" instance,
	// so I figured it wouldn't matter too much that it's a reference type where instances have to be allocated and where their lifetime needs to managed.
	/// <summary>
	/// Represents a collection of fallback fonts for a <see cref="Font"/> instance
	/// </summary>
	public sealed class FallbackCollection
	{
		private readonly Font mParent;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		internal FallbackCollection(Font parent) => mParent = parent;

		[DoesNotReturn]
		private static void FailParentDisposed() => throw new ObjectDisposedException(nameof(Font), $"The {nameof(Font)} instance has been already disposed.");

		[DoesNotReturn]
		private static void FailFallbackArgumentNull(Font? fallback = default) => throw new ArgumentNullException(nameof(fallback));

		[DoesNotReturn]
		private static void FailFallbackArgumentDisposed(Font? fallback = default) => throw new ObjectDisposedException(nameof(fallback), $"The {nameof(fallback)} {nameof(Font)} argument has been already disposed.");

		/// <summary>
		/// Adds a font as a fallback to the actual font
		/// </summary>
		/// <param name="fallback">The font to add as a fallback</param>
		/// <remarks>
		/// <para>
		/// Fallback fonts are used when the actual font doesn't contain a certain glyph.
		/// </para>
		/// <para>
		/// If there are multiple fallback fonts, they are used in the order they were added until a font is found that can render the missing glyph.
		/// </para>
		/// <para>
		/// Fonts to be added to the fallback font collection of an actual font should have the same point size and style as the actual font.
		/// </para>
		/// <para>
		/// Calling this method will update any <see cref="Text"/>s that use the actual font.
		/// </para>
		/// <para>
		/// This method should only be called an the thread that created both the actual font and the fallback font.
		/// </para>
		/// </remarks>
		/// <exception cref="ObjectDisposedException">
		/// The actual font has already been disposed
		/// - OR -
		/// <paramref name="fallback"/> has already been disposed
		/// </exception>
		/// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <c><see langword="null"/></c></exception>
		/// <exception cref="SdlException">Couldn't add the fallback font to the actual font (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
		public void Add(Font fallback)
		{
			unsafe
			{
				var fontPtr = mParent.mFont;

				if (fontPtr is null)
				{
					FailParentDisposed();
				}

				if (fallback is null)
				{
					FailFallbackArgumentNull();
				}

				var fallbackPtr = fallback.mFont;

				if (fallbackPtr is null)
				{
					FailFallbackArgumentDisposed();
				}

				SdlErrorHelper.ThrowIfFailed(TTF_AddFallbackFont(fontPtr, fallbackPtr));
			}
		}

		/// <summary>
		/// Clears all fallback fonts from the actual font
		/// </summary>
		/// <remarks>
		/// <para>
		/// Calling this method may update any <see cref="Text"/>s that use the actual font.
		/// </para>
		/// <para>
		/// This method should only be called an the thread that created the actual font.
		/// </para>
		/// </remarks>
		/// <exception cref="ObjectDisposedException">The actual font has already been disposed</exception>
		public void Clear()
		{
			unsafe
			{
				var fontPtr = mParent.mFont;

				if (fontPtr is null)
				{
					FailParentDisposed();
				}

				TTF_ClearFallbackFonts(fontPtr);
			}
		}

		/// <summary>
		/// Removes a font as a fallback from the actual font
		/// </summary>
		/// <param name="fallback">The font to remove as a fallback</param>
		/// <remarks>
		/// <para>
		/// <paramref name="fallback"/> should have been previously added as a fallback to the actual font using <see cref="Add"/>. If it wasn't, this method will do nothing.
		/// </para>
		/// <para>
		/// Calling this method will update any <see cref="Text"/>s that use the actual font.
		/// </para>
		/// <para>
		/// This method should only be called an the thread that created both the actual font and the fallback font.
		/// </para>
		/// </remarks>
		/// <exception cref="ObjectDisposedException">The actual font has already been disposed</exception>
		/// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <c><see langword="null"/></c></exception>
		public void Remove(Font fallback)
		{
			unsafe
			{
				var fontPtr = mParent.mFont;

				if (fontPtr is null)
				{
					FailParentDisposed();
				}

				if (fallback is null)
				{
					FailFallbackArgumentNull(); // We still throw an exception if the fallback argument is null, just to be consistent with the Add method and to be as clean as possible.
				}

				var fallbackPtr = fallback.mFont;

				// We don't need to throw an exception if fallbackPtr is null, because if the fallback font is already disposed,
				// then it doesn't need to be removed from the fallback font collection at all.
				// Even SDL reflects this behavior by just returning early without doing anything if the fallback argument is null.
				//
				// if (fallbackPtr is null) { }

				TTF_RemoveFallbackFont(fontPtr, fallbackPtr);
			}
		}
	}
}
