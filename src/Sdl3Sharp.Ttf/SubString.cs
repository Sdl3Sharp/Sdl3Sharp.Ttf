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
/// Represents a substring within a <see cref="Text"/>
/// </summary>
/// <remarks>
/// <para>
/// Be aware that the <see cref="SubString"/> type is declared as a <c><see langword="struct"/></c> type for performance reasons, and it is designed to be immutable.
/// However, since it is a rather large type, be cautious about accidentally copying instances of this type, as it might lead to performance degradation.
/// It is recommended to instead keep references to instances of this type (e.g., <c><see langword="ref"/> <see langword="readonly"/></c>) and pass by reference when possible (e.g., <c><see langword="in"/></c>).
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct SubString : IEquatable<SubString>, IFormattable, ISpanFormattable, IEqualityOperators<SubString, SubString, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly SubStringFlags mFlags;
	private readonly int mOffset;
	private readonly int mLength;
	private readonly int mLineIndex;
	private readonly int mClusterIndex;
	private readonly Rect<int> mRect;

	/// <summary>
	/// Gets the flags associated with this subtring
	/// </summary>
	/// <value>
	/// The flags associated with this subtring
	/// </value>
	public readonly SubStringFlags Flags { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mFlags; }

	/// <summary>
	/// Gets the offset form the beginning of the text to the start of this substring in bytes
	/// </summary>
	/// <value>
	/// The offset form the beginning of the text to the start of this substring in bytes
	/// </value>
	public readonly int Offset { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOffset; }

	/// <summary>
	/// Gets the length of this substring in bytes
	/// </summary>
	/// <value>
	/// The length of this substring in bytes
	/// </value>
	public readonly int Length { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mLength; }

	/// <summary>
	/// Gets the index of the line that contains this substring within the text
	/// </summary>
	/// <value>
	/// The index of the line that contains this substring within the text
	/// </value>
	public readonly int LineIndex { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mLineIndex; }

	/// <summary>
	/// Gets the internal cluster index of the substring within the text
	/// </summary>
	/// <value>
	/// The internal cluster index of the substring within the text
	/// </value>
	/// <remarks>
	/// <para>
	/// Thes value of this property is mainly used for quickly interating within the text and might not be useful for most users.
	/// </para>
	/// </remarks>
	public readonly int ClusterIndex { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mClusterIndex; }

	/// <summary>
	/// Gets the bounding rectangle of this substring relative to the top-left corner of the containing text
	/// </summary>
	/// <value>
	/// The bounding rectangle of this substring relative to the top-left corner of the containing text
	/// </value>
	public readonly Rect<int> Rect { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mRect; }

	/// <summary>
	/// Gets the flow direction of this substring
	/// </summary>
	/// <value>
	/// The flow direction of this substring
	/// </value>
	public readonly Direction Direction { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((Direction)(mFlags & SubStringFlags.DirectionMask)); }

	/// <summary>
	/// Gets a value indicating whether this substring contains the beginning of the text
	/// </summary>
	/// <value>
	/// A value indicating whether this substring contains the beginning of the text
	/// </value>
	public readonly bool ContainsTextStart { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => (mFlags & SubStringFlags.TextStart) is not 0; }

	/// <summary>
	/// Gets a value indicating whether this substring contains the beginning of the line with line index <see cref="LineIndex"/>
	/// </summary>
	/// <value>
	/// A value indicating whether this substring contains the beginning of the line with line index <see cref="LineIndex"/>
	/// </value>
	public readonly bool ContainsLineStart { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => (mFlags & SubStringFlags.LineStart) is not 0; }

	/// <summary>
	/// Gets a value indicating whether this substring contains the end of the line with line index <see cref="LineIndex"/>
	/// </summary>
	/// <value>
	/// A value indicating whether this substring contains the end of the line with line index <see cref="LineIndex"/>
	/// </value>
	public readonly bool ContainsLineEnd { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => (mFlags & SubStringFlags.LineEnd) is not 0; }

	/// <summary>
	/// Gets a value indicating whether this substring contains the end of the text
	/// </summary>
	/// <value>
	/// A value indicating whether this substring contains the end of the text
	/// </value>
	public readonly bool ContainsTextEnd { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => (mFlags & SubStringFlags.TextEnd) is not 0; }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is SubString other && Equals(other);

	/// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
	public readonly bool Equals(in SubString other)
		=> mFlags == other.mFlags
		&& mOffset.Equals(other.mOffset)
		&& mLength.Equals(other.mLength)
		&& mLineIndex.Equals(other.mLineIndex)
		&& mClusterIndex.Equals(other.mClusterIndex)
		&& mRect.Equals(in other.mRect);

	readonly bool IEquatable<SubString>.Equals(SubString other) => Equals(other);

	/// <inheritdoc/>
	public readonly override int GetHashCode() => HashCode.Combine(mFlags, mOffset, mLength, mLineIndex, mClusterIndex, mRect);

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Flags)}: {mFlags}, {
			nameof(Offset)}: {mOffset.ToString(format, formatProvider)}, {
			nameof(Length)}: {mLength.ToString(format, formatProvider)}, {
			nameof(LineIndex)}: {mLineIndex.ToString(format, formatProvider)}, {
			nameof(ClusterIndex)}: {mClusterIndex.ToString(format, formatProvider)}, {
			nameof(Rect)}: {mRect.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Flags)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mFlags, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Offset)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mOffset, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Length)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mLength, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(LineIndex)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mLineIndex, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(ClusterIndex)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mClusterIndex, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Rect)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mRect, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	public static bool operator ==(in SubString left, in SubString right)
		=> left.mFlags == right.mFlags
		&& left.mOffset == right.mOffset
		&& left.mLength == right.mLength
		&& left.mLineIndex == right.mLineIndex
		&& left.mClusterIndex == right.mClusterIndex
		&& left.mRect == right.mRect;

	/// <inheritdoc/>
	static bool IEqualityOperators<SubString, SubString, bool>.operator ==(SubString left, SubString right) => left == right;

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	public static bool operator !=(in SubString left, in SubString right)
		=> left.mFlags != right.mFlags
		|| left.mOffset != right.mOffset
		|| left.mLength != right.mLength
		|| left.mLineIndex != right.mLineIndex
		|| left.mClusterIndex != right.mClusterIndex
		|| left.mRect != right.mRect;

	/// <inheritdoc/>
	static bool IEqualityOperators<SubString, SubString, bool>.operator !=(SubString left, SubString right) => left != right;
}
