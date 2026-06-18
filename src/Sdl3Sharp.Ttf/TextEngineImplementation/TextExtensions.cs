using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

/// <summary>
/// Provides extension methods and properties for <see cref="Text"/> when used in custom text engine implementations
/// </summary>
public static class TextExtensions
{
	extension(Text text)
	{
		/// <summary>
		/// Gets the internal <see cref="TextData"/> of the <see cref="Text"/>, for use in custom text engine implementations
		/// </summary>
		/// <value>
		/// The internal <see cref="TextData"/> of the <see cref="Text"/>, for use in custom text engine implementations
		/// </value>
		/// <remarks>
		/// <para>
		/// This property is only really useful if used in custom text engine implementations.
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentNullException"><paramref name="text"/> is <c><see langword="null"/></c></exception>
		/// <exception cref="ObjectDisposedException"><paramref name="text"/> has already been disposed</exception>
		public TextData Data
		{
			get
			{
				unsafe
				{
					if (text is null)
					{
						[DoesNotReturn]
						static void failTextArgumentNull() => throw new ArgumentNullException(nameof(text));

						failTextArgumentNull();
					}

					var data = text.InternalData;

					if (data is null)
					{
						[DoesNotReturn]
						static void failTextDisposed() => throw new ObjectDisposedException(nameof(text), $"The {nameof(Text)} has already been disposed.");

						failTextDisposed();
					}

					return data;
				}
			}
		}
	}
}
