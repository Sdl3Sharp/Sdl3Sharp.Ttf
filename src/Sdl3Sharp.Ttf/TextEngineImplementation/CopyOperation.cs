using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Drawing;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

/// <summary>
/// Represents a texture copy draw operation in a custom text engine draw sequence
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly ref partial struct CopyOperation : IDrawOperation<CopyOperation>,
	IEquatable<DrawOperation>, IEquatable<CopyOperation>, IFormattable, ISpanFormattable
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IDrawOperation<CopyOperation>.Accepts(DrawCommand command) => command is DrawCommand.Copy;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static CopyOperation IDrawOperation<CopyOperation>.FromBase(DrawOperation operation) => new(in operation.Target.Copy);

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly ref readonly TTF_CopyOperation mTarget;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private CopyOperation(ref readonly TTF_CopyOperation target) => mTarget = ref target;

	internal readonly ref readonly TTF_CopyOperation Target { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => ref mTarget; }

	/// <summary>
	/// Gets the <see cref="DrawCommand"/> associated with this draw operation
	/// </summary>
	/// <value>
	/// The <see cref="DrawCommand"/> associated with this draw operation, which is always <see cref="DrawCommand.Copy"/> for instances of this type
	/// </value>
	public readonly DrawCommand Command { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.Cmd; }

	/// <summary>
	/// Gets the offset into the text corresponding with the glyph associated with this draw operation
	/// </summary>
	/// <value>
	/// The offset into the text corresponding with the glyph associated with this draw operation
	/// </value>
	/// <remarks>
	/// <para>
	/// There might be multiple glyphs with the same text offset and the next text offset might be several Unicode code points later.
	/// In that casee the glyphs and Unicode code points are grouped together and the group's bounding box is the union of the <see cref="DestinationRect"/>s of the corresponding glyphs.
	/// </para>
	/// </remarks>
	public readonly int TextOffset { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.TextOffset; }

	/// <summary>
	/// Gets the <see cref="Font"/> containing the glyph associated with this draw operation
	/// </summary>
	/// <value>
	/// The <see cref="Font"/> containing the glyph associated with this draw operation
	/// </value>
	/// <remarks>
	/// <para>
	/// You should call <see cref="FontExtensions.TryGetGlyphImageForIndex(Font, uint, out Video.Surface?, out ImageType)"/> on the <see cref="Font"/> returned by this property
	/// with the value of <see cref="GlyphIndex"/> from this instance to get the pixel image of the glyph associated with this draw operation.
	/// </para>
	/// </remarks>
	/// <exception cref="InvalidOperationException">The glyph font referenced by this draw operation is invalid</exception>
	public readonly Font GlyphFont
	{
		get
		{
			unsafe
			{
				if (!Font.TryGetOrCreate(mTarget.GlyphFont, out var result))
				{
					// this is bad, the only way for Font.TryGetOrCreate to not succeed is if the pointer is null, which should never be the case for a valid TTF_CopyOperation

					[DoesNotReturn]
					static void failInvalidGlpyhFont() => throw new InvalidOperationException($"The glyph font referenced by the {nameof(CopyOperation)} is invalid");

					failInvalidGlpyhFont();
				}

				return result;
			}
		}
	}

	/// <summary>
	/// The index of the glyph in the <see cref="GlyphFont"/> associated with this draw operation
	/// </summary>
	/// <value>
	/// The index of the glyph in the <see cref="GlyphFont"/> associated with this draw operation
	/// </value>
	/// <remarks>
	/// <para>
	/// You should call <see cref="FontExtensions.TryGetGlyphImageForIndex(Font, uint, out Video.Surface?, out ImageType)"/> on the <see cref="GlyphFont"/> returned by this instance
	/// with the value of this property to get the pixel image of the glyph associated with this draw operation.
	/// </para>
	/// </remarks>
	public readonly uint GlyphIndex { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.GlyphIndex; }

	/// <summary>
	/// The area within the glyph associated with this draw operation to be drawn
	/// </summary>
	/// <value>
	/// The area within the glyph associated with this draw operation to be drawn
	/// </value>
	public readonly Rect<int> SourceRect { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.Src; }

	/// <summary>
	/// The region within the text area where the glyph associated with this draw operation should be drawn
	/// </summary>
	/// <value>
	/// The region within the text area where the glyph associated with this draw operation should be drawn, in pixels
	/// </value>
	/// <remarks>
	/// <para>
	/// The <see cref="Rect{T}.Left"/> coordinate of the result is relative to the left edge of the text area, going rightwards until the <see cref="Rect{T}.Width"/> of the result is covered,
	/// the <see cref="Rect{T}.Top"/> coordinate of the result is relative to the top edge of the text area, going downwards until the <see cref="Rect{T}.Height"/> of the result is covered.
	/// </para>
	/// </remarks>
	public readonly Rect<int> DestinationRect { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.Dst; }

	/// <summary>Not supported. Do not call this method, use the <see cref="Equals(CopyOperation)"/> method instead. This method will always throw a <see cref="NotSupportedException"/> if called.</summary>
	/// <exception cref="NotSupportedException">Always</exception>
	[Obsolete($"Not supported. Do not call this method, use the {nameof(Equals)}({nameof(CopyOperation)}) method instead. This method will always throw a {nameof(NotSupportedException)} if called.")]
	[DoesNotReturn]
#pragma warning disable CS0809 // That's just how it is for ref structs
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();
#pragma warning restore CS0809

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(DrawOperation other) => ((DrawOperation)this).Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(CopyOperation other) => Equals((DrawOperation)other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => ((DrawOperation)this).GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Command)}: {Command}, {
			nameof(TextOffset)}: {TextOffset.ToString(format, formatProvider)}, {
			nameof(GlyphFont)}: {GlyphFont switch { null => "null", var font => font.ToString() }}, {
			nameof(GlyphIndex)}: {GlyphIndex.ToString(format, formatProvider)}, {
			nameof(SourceRect)}: {SourceRect.ToString(format, formatProvider)}, {
			nameof(DestinationRect)}: {DestinationRect.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Command)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Command, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(TextOffset)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(TextOffset, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(GlyphFont)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(GlyphFont switch { null => "null", var font => font.ToString() }, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(GlyphIndex)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(GlyphIndex, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(SourceRect)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(SourceRect, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(DestinationRect)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(DestinationRect, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(CopyOperation left, DrawOperation right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(CopyOperation left, DrawOperation right) => !(left == right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(CopyOperation left, CopyOperation right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(CopyOperation left, CopyOperation right) => !(left == right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator DrawOperation(CopyOperation operation) => new(ref Unsafe.As<TTF_CopyOperation, DrawOperation.TTF_DrawOperation>(ref Unsafe.AsRef(in operation.mTarget)));
}
