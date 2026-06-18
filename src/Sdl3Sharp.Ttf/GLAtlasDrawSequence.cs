#if SDL_TTF3_3_0_OR_GREATER

using Sdl3Sharp.Internal;
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
/// Represents a draw sequence of OpenGL geometry data needed to render a <see cref="Text"/> using a <see cref="GLTextEngine"/>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly ref partial struct GLAtlasDrawSequence :
	IEquatable<GLAtlasDrawSequence>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly ref readonly TTF_GLAtlasDrawSequence mTarget;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal GLAtlasDrawSequence(ref readonly TTF_GLAtlasDrawSequence target) => mTarget = ref target;

	/// <summary>
	/// Gets the texture atlas, that stores the glyphs for this draw sequence
	/// </summary>
	/// <value>
	/// The texture atlas, as an OpenGL texture name (same as <c>GLuint</c>), that stores the glyphs for this draw sequence or <c>0</c> if this draw sequence represents a solid fill operation
	/// </value>
	public readonly uint AtlasTexture { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.AtlasTexture; }

	/// <summary>
	/// Gets the type of the image data in this draw sequence 
	/// </summary>
	/// <value>
	/// The type of the image data in this draw sequence
	/// </value>
	public readonly ImageType ImageType { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.ImageType; }

	/// <summary>
	/// Gets the indices into the <see cref="Vertices"/> to draw for this draw sequence
	/// </summary>
	/// <value>
	/// The indices into the <see cref="Vertices"/> to draw for this draw sequence
	/// </value>
	public readonly ReadOnlySpan<ushort> Indices
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

				return new ReadOnlySpan<ushort>(indices, mTarget.NumIndices);
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
	/// and the <see cref="AtlasTexture"/> property will be <c>0</c>.
	/// </para>
	/// </remarks>
	public readonly bool IsSolidFill { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTarget.AtlasTexture is 0; }

	internal readonly ref readonly TTF_GLAtlasDrawSequence Target { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => ref mTarget; }

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
	public readonly ReadOnlySpan<GLAtlasDrawVertex> Vertices
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				var vertices = mTarget.Vertices;

				if (vertices is null)
				{
					return default;
				}

				return new ReadOnlySpan<GLAtlasDrawVertex>(vertices, mTarget.NumVertices);
			}
		}
	}

	/// <summary>Not supported. Do not call this method, use the <see cref="Equals(GLAtlasDrawSequence)"/> method instead. This method will always throw a <see cref="NotSupportedException"/> if called.</summary>
	/// <exception cref="NotSupportedException">Always</exception>
	[Obsolete($"Not supported. Do not call this method, use the {nameof(Equals)}({nameof(GLAtlasDrawSequence)}) method instead. This method will always throw a {nameof(NotSupportedException)} if called.")]
	[DoesNotReturn]
#pragma warning disable CS0809 // That's just how it is for ref structs
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();
#pragma warning restore CS0809

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public bool Equals(GLAtlasDrawSequence other) => Unsafe.AreSame(in mTarget, in other.mTarget);

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
		=> $"{{ {nameof(AtlasTexture)}: {AtlasTexture.ToString(format, formatProvider) }, {
			nameof(Vertices)}: [{Vertices switch { { Length: >= 1 } => string.Join(", ", ((IEnumerable<GLAtlasDrawVertex>)[..Vertices]).Select(p => p.ToString(format, formatProvider))), _ => string.Empty }}], {
			nameof(Indices)}: [{Indices switch { { Length: >= 1 } => string.Join(", ", ((IEnumerable<int>)[.. Indices]).Select(i => i.ToString(format, formatProvider))), _ => string.Empty }}], {
			nameof(ImageType)}: {ImageType} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		if ( !(SpanFormat.TryWrite($"{{ {nameof(AtlasTexture)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(AtlasTexture, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Vertices)}: [", ref destination, ref charsWritten)))
		{
			return false;
		}

		var verticesEnumerator = Vertices.GetEnumerator();

		if (verticesEnumerator.MoveNext())
		{
			if (!(SpanFormat.TryWrite(' ', ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(in verticesEnumerator.Current, ref destination, ref charsWritten, format, provider)))
			{
				return false;
			}

			while (verticesEnumerator.MoveNext())
			{
				if (!(SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
					&& SpanFormat.TryWrite(in verticesEnumerator.Current, ref destination, ref charsWritten, format, provider)))
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
			if (!(SpanFormat.TryWrite(' ', ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(indicesEnumerator.Current, ref destination, ref charsWritten, format, provider)))
			{
				return false;
			}

			while (indicesEnumerator.MoveNext())
			{
				if (!(SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
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
	public static bool operator ==(GLAtlasDrawSequence left, GLAtlasDrawSequence right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(GLAtlasDrawSequence left, GLAtlasDrawSequence right) => !(left == right);
}

#endif
