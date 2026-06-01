using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

[StructLayout(LayoutKind.Sequential)]
public readonly ref partial struct GpuAtlasDrawData
{
	private unsafe readonly Node* mNode;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal unsafe GpuAtlasDrawData(Node* node) => mNode = node;

	/// <summary>
	/// Copies the <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawData"/> to a given destination starting at a specified offset
	/// </summary>
	/// <param name="destination">The destination array to copy the <see cref="GpuAtlasDrawSequence"/>s to</param>
	/// <param name="offset">The offset in the destination array at which to start copying</param>
	/// <returns>The number of <see cref="GpuAtlasDrawSequence"/>s copied</returns>
	/// <remarks>
	/// <para>
	/// The <paramref name="destination"/> array will have been written to up until the returned number of copied <see cref="GpuAtlasDrawSequence"/>s elements, even in case of an exception being thrown, and potentially remaining elements will be left unmodified.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than <c>0</c> or greater than or equal to the length of <paramref name="destination"/></exception>
	/// <exception cref="ArgumentException"><paramref name="destination"/>, starting at <paramref name="offset"/>, is too small to hold all <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawData"/></exception>
	public readonly int CopyTo(GpuAtlasDrawSequence[] destination, int offset)
	{
		if (offset is < 0 || offset >= destination.Length)
		{
			failOffsetOutOfRange();
		}

		if (!TryCopyTo(destination.AsSpan(offset), out var sequencesWritten))
		{
			failDestinationTooSmall();
		}

		return sequencesWritten;

		[DoesNotReturn]
		static void failOffsetOutOfRange() => throw new ArgumentOutOfRangeException(nameof(offset), $"The offset into the {nameof(destination)} must be greater or equal to 0 and less than the {nameof(destination)}'s length.");

		[DoesNotReturn]
		static void failDestinationTooSmall() => throw new ArgumentException($"The {nameof(destination)} array is too small.", nameof(destination));
	}

	/// <summary>
	/// Copies the <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawData"/> to a given destination
	/// </summary>
	/// <param name="destination">The destination span to copy the <see cref="GpuAtlasDrawSequence"/>s to</param>
	/// <returns>The number of <see cref="GpuAtlasDrawSequence"/>s copied</returns>
	/// <remarks>
	/// <para>
	/// The <paramref name="destination"/> span will have been written to up until the returned number of copied <see cref="GpuAtlasDrawSequence"/>s elements, even in case of an exception being thrown, and potentially remaining elements will be left unmodified.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException"><paramref name="destination"/> is too small to hold all <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawData"/></exception>
	public readonly int CopyTo(Span<GpuAtlasDrawSequence> destination)
	{
		if (!TryCopyTo(destination, out int sequencesWritten))
		{
			failDestinationTooSmall();
		}

		return sequencesWritten;

		[DoesNotReturn]
		static void failDestinationTooSmall() => throw new ArgumentException($"The {nameof(destination)} span is too small.", nameof(destination));
	}

	/// <summary>
	/// Enumerates the <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawData"/>
	/// </summary>
	/// <returns>A <see cref="Enumerator"/> that can be used to enumerate the <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawData"/></returns>
	public readonly Enumerator GetEnumerator() => new(this);

	/// <summary>
	/// Copies the <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawData"/> to a new array and returns it
	/// </summary>
	/// <returns>A new array containing all <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawData"/></returns>
	public readonly GpuAtlasDrawSequence[] ToArray() => [..this];

	/// <summary>
	/// Tries to copy the <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawData"/> to a given destination starting at a specified offset
	/// </summary>
	/// <param name="destination">The destination array to copy the <see cref="GpuAtlasDrawSequence"/>s to</param>
	/// <param name="offset">The offset in the destination array at which to start copying</param>
	/// <param name="sequencesWritten">The number of <see cref="GpuAtlasDrawSequence"/>s copied; regardless of the successful completion of the copy operation</param>
	/// <returns><c><see langword="true"/></c>, if all <see cref="GpuAtlasDrawSequence"/>s were successfully copied; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <paramref name="sequencesWritten"/> will be set to the number of <see cref="GpuAtlasDrawSequence"/>s copied to <paramref name="destination"/> regardless of whether the copy operation was successful or not.
	/// The <paramref name="destination"/> array will have been written to up until <paramref name="sequencesWritten"/> elements in any case, and potentially remaining elements will be left unmodified.
	/// </para>
	/// </remarks>
	public readonly bool TryCopyTo(GpuAtlasDrawSequence[] destination, int offset, out int sequencesWritten)
	{
		if (offset is < 0 || offset >= destination.Length)
		{
			sequencesWritten = 0;
			return false;
		}

		return TryCopyTo(destination.AsSpan(offset), out sequencesWritten);
	}

	/// <summary>
	/// Tries to copy the <see cref="GpuAtlasDrawSequence"/>s in this <see cref="GpuAtlasDrawData"/> to a given destination
	/// </summary>
	/// <param name="destination">The destination span to copy the <see cref="GpuAtlasDrawSequence"/>s to</param>
	/// <param name="sequencesWritten">The number of <see cref="GpuAtlasDrawSequence"/>s copied; regardless of the successful completion of the copy operation</param>
	/// <returns><c><see langword="true"/></c>, if all <see cref="GpuAtlasDrawSequence"/>s were successfully copied; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <paramref name="sequencesWritten"/> will be set to the number of <see cref="GpuAtlasDrawSequence"/>s copied to <paramref name="destination"/> regardless of whether the copy operation was successful or not.
	/// The <paramref name="destination"/> span will have been written to up until <paramref name="sequencesWritten"/> elements in any case, and potentially remaining elements will be left unmodified.
	/// </para>
	/// </remarks>
	public readonly bool TryCopyTo(Span<GpuAtlasDrawSequence> destination, out int sequencesWritten)
	{
		sequencesWritten = 0;

		foreach (ref readonly var sequence in this)
		{
			if (sequencesWritten >= destination.Length)
			{
				return false;
			}

			destination[sequencesWritten++] = sequence;
		}

		return true;
	}
}
