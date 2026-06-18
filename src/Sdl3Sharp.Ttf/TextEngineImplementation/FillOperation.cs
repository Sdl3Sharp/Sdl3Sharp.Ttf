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
/// Represents a fill draw operation in a custom text engine draw sequence
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly ref partial struct FillOperation : IDrawOperation<FillOperation>,
	IEquatable<DrawOperation>, IEquatable<FillOperation>, IFormattable, ISpanFormattable
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IDrawOperation<FillOperation>.Accepts(DrawCommand command) => command is DrawCommand.Fill;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static FillOperation IDrawOperation<FillOperation>.FromBase(DrawOperation operation) => new(in operation.Target.Fill);

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);


	private readonly ref readonly TTF_FillOperation mTarget;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private FillOperation(ref readonly TTF_FillOperation target) => mTarget = ref target;

	internal readonly ref readonly TTF_FillOperation Target { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => ref mTarget; }

	/// <summary>
	/// Gets the <see cref="DrawCommand"/> associated with this draw operation
	/// </summary>
	/// <value>
	/// The <see cref="DrawCommand"/> associated with this draw operation, which is always <see cref="DrawCommand.Fill"/> for instances of this type
	/// </value>
	public readonly DrawCommand Command { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.Cmd; }

	/// <summary>
	/// Gets the region of the text area to fill
	/// </summary>
	/// <value>
	/// The region of the text area to fill, in pixels
	/// </value>
	/// <remarks>
	/// <para>
	/// The <see cref="Rect{T}.Left"/> coordinate of the result is relative to the left edge of the text area, going rightwards until the <see cref="Rect{T}.Width"/> of the result is covered,
	/// the <see cref="Rect{T}.Top"/> coordinate of the result is relative to the top edge of the text area, going downwards until the <see cref="Rect{T}.Height"/> of the result is covered.
	/// </para>
	/// </remarks>
	public readonly Rect<int> Rect { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.Rect; }

	/// <summary>Not supported. Do not call this method, use the <see cref="Equals(FillOperation)"/> method instead. This method will always throw a <see cref="NotSupportedException"/> if called.</summary>
	/// <exception cref="NotSupportedException">Always</exception>
	[Obsolete($"Not supported. Do not call this method, use the {nameof(Equals)}({nameof(FillOperation)}) method instead. This method will always throw a {nameof(NotSupportedException)} if called.")]
	[DoesNotReturn]
#pragma warning disable CS0809 // That's just how it is for ref structs
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();
#pragma warning restore CS0809

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(DrawOperation other) => ((DrawOperation)this).Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(FillOperation other) => Equals((DrawOperation)other);

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
			nameof(Rect)}: {Rect.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Command)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Command, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Rect)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Rect, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(FillOperation left, DrawOperation right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(FillOperation left, DrawOperation right) => !(left == right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(FillOperation left, FillOperation right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(FillOperation left, FillOperation right) => !(left == right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator DrawOperation(FillOperation operation) => new(ref Unsafe.As<TTF_FillOperation, DrawOperation.TTF_DrawOperation>(ref Unsafe.AsRef(in operation.mTarget)));
}
