#if SDL_TTF3_3_0_OR_GREATER

using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Drawing;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a single vertex in a <see cref="GLAtlasDrawSequence"/>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct GLAtlasDrawVertex :
	IEquatable<GLAtlasDrawVertex>, IFormattable, ISpanFormattable, IEqualityOperators<GLAtlasDrawVertex, GLAtlasDrawVertex, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly Point<float> mPosition;
	private readonly Point<float> mTexcoord;

	/// <summary>
	/// Gets the position of this vertex
	/// </summary>
	/// <value>
	/// The position of this vertex
	/// </value>
	public readonly Point<float> Position { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mPosition; }

	/// <summary>
	/// Gets the texture coordinate of this vertex
	/// </summary>
	/// <value>
	/// The texture coordinate of this vertex, or normalized rectangle coordinate if the <see cref="GLAtlasDrawSequence"/> this vertex belongs to is <see cref="GLAtlasDrawSequence.IsSolidFill">solid fill</see>
	/// </value>
	public readonly Point<float> TexCoord { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTexcoord; }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is GLAtlasDrawVertex other && Equals(other);

	/// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(in GLAtlasDrawVertex other)
		=> mPosition.Equals(in other.mPosition)
		&& mTexcoord.Equals(in other.mTexcoord);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly bool IEquatable<GLAtlasDrawVertex>.Equals(GLAtlasDrawVertex other) => Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => HashCode.Combine(mPosition, mTexcoord);

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Position)}: {mPosition.ToString(format, formatProvider)}, {
			nameof(TexCoord)}: {mTexcoord.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Position)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mPosition, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(TexCoord)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mTexcoord, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(in GLAtlasDrawVertex left, in GLAtlasDrawVertex right)
		=> left.mPosition == right.mPosition
		&& left.mTexcoord == right.mTexcoord;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<GLAtlasDrawVertex, GLAtlasDrawVertex, bool>.operator ==(GLAtlasDrawVertex left, GLAtlasDrawVertex right) => left == right;

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(in GLAtlasDrawVertex left, in GLAtlasDrawVertex right)
		=> left.mPosition != right.mPosition
		|| left.mTexcoord != right.mTexcoord;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<GLAtlasDrawVertex, GLAtlasDrawVertex, bool>.operator !=(GLAtlasDrawVertex left, GLAtlasDrawVertex right) => left != right;
}

#endif
