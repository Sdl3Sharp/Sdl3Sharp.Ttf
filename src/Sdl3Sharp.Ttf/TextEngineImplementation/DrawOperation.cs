using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

/// <summary>
/// Represents a single draw operation in a custom text engine draw sequence
/// </summary>
/// <remarks>
/// <para>
/// Despite the fact that this is a value type, it is treated as a base type for more specific draw operation types such as <see cref="FillOperation"/> and <see cref="CopyOperation"/>.
/// To convert an instance of this type to a more specific draw operation type, you can use the <see cref="TryAs{TDrawOperation}(out TDrawOperation)"/> method.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly ref partial struct DrawOperation : IDrawOperation<DrawOperation>,
	IEquatable<DrawOperation>, IFormattable, ISpanFormattable
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IDrawOperation<DrawOperation>.Accepts(DrawCommand command) => true;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static DrawOperation IDrawOperation<DrawOperation>.FromBase(DrawOperation operation) => operation;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static DrawOperation ToSpecific<TDrawOperation>(DrawOperation operation)
		where TDrawOperation : struct, IDrawOperation<TDrawOperation>, allows ref struct
		=> TDrawOperation.FromBase(operation);

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly ref readonly TTF_DrawOperation mTarget;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal DrawOperation(ref readonly TTF_DrawOperation target) => mTarget = ref target;

	/// <inheritdoc/>
	public readonly DrawCommand Command { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.Cmd; }

	internal readonly ref readonly TTF_DrawOperation Target { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => ref mTarget; }

	/// <summary>Not supported. Do not call this method, use the <see cref="Equals(DrawOperation)"/> method instead. This method will always throw a <see cref="NotSupportedException"/> if called.</summary>
	/// <exception cref="NotSupportedException">Always</exception>
	[Obsolete($"Not supported. Do not call this method, use the {nameof(Equals)}({nameof(DrawOperation)}) method instead. This method will always throw a {nameof(NotSupportedException)} if called.")]
	[DoesNotReturn]
#pragma warning disable CS0809 // That's just how it is for ref structs
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();
#pragma warning restore CS0809

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(DrawOperation other) => Unsafe.AreSame(in mTarget, in other.mTarget);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() { unsafe { return unchecked(((IntPtr)Unsafe.AsPointer(ref Unsafe.AsRef(in mTarget))).GetHashCode()); } }

	/// <summary>
	/// Tries to reinterpret this <see cref="DrawOperation"/> as a more specific draw operation type, based on the actual <see cref="Command"/> value
	/// </summary>
	/// <typeparam name="TDrawOperation">The type of the draw operation to try to reinterpret as</typeparam>
	/// <param name="result">The draw operation of type <typeparamref name="TDrawOperation"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<typeparamref name="TDrawOperation"/>)</c></param>
	/// <returns><c><see langword="true"/></c>, if the conversion succeeded, and the actual <see cref="Command"/> value is accepted by the requested <typeparamref name="TDrawOperation"/> type; otherwise, <c><see langword="false"/></c></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool TryAs<TDrawOperation>(out TDrawOperation result)
		where TDrawOperation : struct, IDrawOperation<TDrawOperation>, allows ref struct
	{
		if (TDrawOperation.Accepts(mTarget.Cmd))
		{
			result = TDrawOperation.FromBase(this);
			return true;
		}

		result = default;
		return false;
	}

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => Command switch
	{
		DrawCommand.Fill => $"{nameof(FillOperation)} {ToSpecific<FillOperation>(this).ToString(format, formatProvider)}",
		DrawCommand.Copy => $"{nameof(CopyOperation)} {ToSpecific<CopyOperation>(this).ToString(format, formatProvider)}",
		_ => $"{{ {nameof(Command)}: {Command} }}"
	};

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return Command switch
		{
			DrawCommand.Fill => SpanFormat.TryWrite($"{nameof(FillOperation)} ", ref destination, ref charsWritten)
							 && SpanFormat.TryWrite(ToSpecific<FillOperation>(this), ref destination, ref charsWritten, format, provider),
			DrawCommand.Copy => SpanFormat.TryWrite($"{nameof(CopyOperation)} ", ref destination, ref charsWritten)
							 && SpanFormat.TryWrite(ToSpecific<CopyOperation>(this), ref destination, ref charsWritten, format, provider),
			_ => SpanFormat.TryWrite($"{{ {nameof(Command)}: ", ref destination, ref charsWritten)
			  && SpanFormat.TryWrite(Command, ref destination, ref charsWritten)
			  && SpanFormat.TryWrite(" }", ref destination, ref charsWritten)
		};
	}

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(DrawOperation left, DrawOperation right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(DrawOperation left, DrawOperation right) => !(left == right);

	// this is still a thing, huh? I was under the impression that by C#14, the compiler would implement this by itself, if it was missing
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static implicit IDrawOperation<DrawOperation>.operator DrawOperation(DrawOperation operation) => operation;
}
