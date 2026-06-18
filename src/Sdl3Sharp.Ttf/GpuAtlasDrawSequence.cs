using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Gpu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a draw sequence of GPU geometry data needed to render a <see cref="Text"/> using a <see cref="GpuTextEngine"/>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly ref partial struct GpuAtlasDrawSequence :
	IEquatable<GpuAtlasDrawSequence>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly ref readonly TTF_GPUAtlasDrawSequence mTarget;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal GpuAtlasDrawSequence(ref readonly TTF_GPUAtlasDrawSequence target) => mTarget = ref target;

	/// <summary>
	/// Gets the texture atlas that stores the glyphs for this draw sequence
	/// </summary>
	/// <value>
	/// The texture atlas that stores the glyphs for this draw sequence, or <c><see langword="null"/></c> if this draw sequence represents a solid fill operation
	/// </value>
	public readonly GpuTexture? AtlasTexture
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				GpuTexture.TryGetOrCreate(mTarget.AtlasTexture, out var atlasTexture);
				return atlasTexture;
			}
		}
	}

	/// <summary>
	/// Gets the type of the image data in this draw sequence 
	/// </summary>
	/// <value>
	/// The type of the image data in this draw sequence
	/// </value>
	public readonly ImageType ImageType { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.ImageType; }

	/// <summary>
	/// Gets the indices into the <see cref="Xy"/> and <see cref="Uv"/> arrays that define the vertices to draw for this draw sequence
	/// </summary>
	/// <value>
	/// The indices into the <see cref="Xy"/> and <see cref="Uv"/> arrays that define the vertices to draw for this draw sequence
	/// </value>
	public readonly ReadOnlySpan<int> Indices
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				var indices = mTarget.Indices;

				if (indices is null)
				{
					return default;
				}

				return new ReadOnlySpan<int>(indices, mTarget.NumIndices);
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether this draw sequence represents a solid fill operation
	/// </summary>
	/// <value>
	/// A value indicating whether this draw sequence represents a solid fill operation
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of this property is <c><see langword="true"/></c>, then this draw sequence represents a solid fill operation,
	/// and the <see cref="AtlasTexture"/> property will be <c><see langword="null"/></c> and the <see cref="Uv"/> property will be <c><see langword="default"/>(<see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;<see cref="Point{T}">Point&lt;<see cref="float"/>&gt;</see>&gt;</see>)</c>.
	/// </para>
	/// </remarks>
	[MemberNotNullWhen(true, nameof(AtlasTexture))]
	public readonly bool IsSolidFill { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mTarget.AtlasTexture is null; } } }

	internal readonly ref readonly TTF_GPUAtlasDrawSequence Target { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => ref mTarget; }

	/// <summary>
	/// Gets the vertex positions for this draw sequence
	/// </summary>
	/// <value>
	/// The vertex positions for this draw sequence
	/// </value>
	/// <remarks>
	/// <para>
	/// The draw sequence will be indexed by the <see cref="Indices"/> array into the array returned by this property to form the vertices to be drawn for this draw sequence.
	/// </para>
	/// </remarks>
	public readonly ReadOnlySpan<Point<float>> Xy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				var xy = mTarget.Xy;

				if (xy is null)
				{
					return default;
				}

				return new ReadOnlySpan<Point<float>>(xy, mTarget.NumVertices);
			}
		}
	}

	/// <summary>
	/// Gets the texture coordinates for this draw sequence
	/// </summary>
	/// <value>
	/// The texture coordinates for this draw sequence, or <c><see langword="default"/>(<see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;<see cref="Point{T}">Point&lt;<see cref="float"/>&gt;</see>&gt;</see>)</c> if this draw sequence represents a solid fill operation
	/// </value>
	/// <remarks>
	/// <para>
	/// The draw sequence will be indexed by the <see cref="Indices"/> array into the array returned by this property to form the texture coordinates to be used for this draw sequence.
	/// </para>
	/// </remarks>
	public readonly ReadOnlySpan<Point<float>> Uv
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				var uv = mTarget.Uv;

				if (uv is null)
				{
					return default;
				}

				return new ReadOnlySpan<Point<float>>(uv, mTarget.NumVertices);
			}
		}
	}

	/// <summary>Not supported. Do not call this method, use the <see cref="Equals(GpuAtlasDrawSequence)"/> method instead. This method will always throw a <see cref="NotSupportedException"/> if called.</summary>
	/// <exception cref="NotSupportedException">Always</exception>
	[Obsolete($"Not supported. Do not call this method, use the {nameof(Equals)}({nameof(GpuAtlasDrawSequence)}) method instead. This method will always throw a {nameof(NotSupportedException)} if called.")]
	[DoesNotReturn]
#pragma warning disable CS0809 // That's just how it is for ref structs
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();
#pragma warning restore CS0809

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public bool Equals(GpuAtlasDrawSequence other) => Unsafe.AreSame(in mTarget, in other.mTarget);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() { unsafe { return unchecked(((IntPtr)Unsafe.AsPointer(ref Unsafe.AsRef(in mTarget))).GetHashCode()); } }

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(AtlasTexture)}: {AtlasTexture switch { null => "null", var texture => texture.ToString() }}, {
			nameof(Xy)}: [{ Xy switch { { Length: >= 1 } => string.Join(", ", ((IEnumerable<Point<float>>)[..Xy]).Select(p => p.ToString(format, formatProvider))), _ => string.Empty } }], {
			nameof(Uv)}: [{ Uv switch { { Length: >= 1 } => string.Join(", ", ((IEnumerable<Point<float>>)[..Uv]).Select(p => p.ToString(format, formatProvider))), _ => string.Empty } }], {
			nameof(Indices)}: [{ Indices switch { { Length: >= 1 } => string.Join(", ", ((IEnumerable<int>)[..Indices]).Select(i => i.ToString(format, formatProvider))), _ => string.Empty } }], {
			nameof(ImageType)}: {ImageType} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		if ( !(SpanFormat.TryWrite($"{{ {nameof(AtlasTexture)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(AtlasTexture switch { null => "null", var texture => texture.ToString() }, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Xy)}: [", ref destination, ref charsWritten)))
		{
			return false;
		}

		var xyEnumerator = Xy.GetEnumerator();

		if (xyEnumerator.MoveNext())
		{
			if ( !(SpanFormat.TryWrite(' ', ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(xyEnumerator.Current, ref destination, ref charsWritten, format, provider)))
			{
				return false;
			}

			while (xyEnumerator.MoveNext())
			{
				if ( !(SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
					&& SpanFormat.TryWrite(xyEnumerator.Current, ref destination, ref charsWritten, format, provider)))
				{
					return false;
				}
			}

			if (!SpanFormat.TryWrite(' ', ref destination, ref charsWritten))
			{
				return false;
			}
		}

		if (!SpanFormat.TryWrite($"], {nameof(Uv)}: [", ref destination, ref charsWritten))
		{
			return false;
		}

		var uvEnumerator = Uv.GetEnumerator();

		if (uvEnumerator.MoveNext())
		{
			if ( !(SpanFormat.TryWrite(' ', ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(uvEnumerator.Current, ref destination, ref charsWritten, format, provider)))
			{
				return false;
			}

			while (uvEnumerator.MoveNext())
			{
				if ( !(SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
					&& SpanFormat.TryWrite(uvEnumerator.Current, ref destination, ref charsWritten, format, provider)))
				{
					return false;
				}
			}

			if (!SpanFormat.TryWrite(' ', ref destination, ref charsWritten))
			{
				return false;
			}
		}

		if (!SpanFormat.TryWrite($"], {nameof(Indices)}: [", ref destination, ref charsWritten))
		{
			return false;
		}

		var indicesEnumerator = Indices.GetEnumerator();

		if (indicesEnumerator.MoveNext())
		{
			if ( !(SpanFormat.TryWrite(' ', ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(indicesEnumerator.Current, ref destination, ref charsWritten, format, provider)))
			{
				return false;
			}

			while (indicesEnumerator.MoveNext())
			{
				if ( !(SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
					&& SpanFormat.TryWrite(indicesEnumerator.Current, ref destination, ref charsWritten, format, provider)))
				{
					return false;
				}
			}

			if (!SpanFormat.TryWrite(' ', ref destination, ref charsWritten))
			{
				return false;
			}
		}

		return SpanFormat.TryWrite($"], {nameof(ImageType)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(ImageType, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(GpuAtlasDrawSequence left, GpuAtlasDrawSequence right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(GpuAtlasDrawSequence left, GpuAtlasDrawSequence right) => !(left == right);
}
