using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a collection of <see cref="SubString"/>s
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly ref partial struct SubStringCollection
{
	private readonly ReadOnlySpan<IntPtr> mSubStrings;

	internal unsafe SubStringCollection(SubString** subStrings, int count)
	{
		if (subStrings is null)
		{
			mSubStrings = [];
		}
		else
		{
			if (count is < 0)
			{
				count = 0;
				var subStringsPtr = subStrings;
				while (*subStringsPtr++ is not null)
				{
					count++;
				}
			}

			if (count is 0)
			{
				mSubStrings = [];
			}
			else
			{
				var subStringArray = GC.AllocateUninitializedArray<IntPtr>(count);

				MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<IntPtr>(*subStrings), count).CopyTo(subStringArray);

				mSubStrings = subStringArray;
			}
		}
	}

	/// <summary>
	/// Gets the number of <see cref="SubString"/>s in this <see cref="SubStringCollection"/>
	/// </summary>
	public readonly int Count { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mSubStrings.Length; }

	/// <summary>
	/// Gets a reference to the <see cref="SubString"/> at a specified index in this <see cref="SubStringCollection"/>
	/// </summary>
	/// <param name="index">The index of the <see cref="SubString"/> to get a reference to</param>
	/// <returns>A reference to the <see cref="SubString"/> at the specified <paramref name="index"/></returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c> or greater than or equal to <see cref="Count"/></exception>
	public readonly ref readonly SubString this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				if (index is < 0 || index >= mSubStrings.Length)
				{
					[DoesNotReturn]
					static void failIndexOutOfRange() => throw new ArgumentOutOfRangeException(nameof(index), $"The {nameof(index)} must be greater or equal to 0 and less than the number of substrings in the {nameof(SubStringCollection)}");

					failIndexOutOfRange();
				}

				return ref Unsafe.AsRef<SubString>(unchecked((SubString*)mSubStrings[index]));
			}
		}
	}

	/// <summary>
	/// Copies the <see cref="SubString"/>s in this <see cref="SubStringCollection"/> to a given destination, starting at a specified offset
	/// </summary>
	/// <param name="destination">The destination array to copy the <see cref="SubString"/>s to</param>
	/// <param name="offset">The offset in the destination array at which to start copying</param>
	/// <returns>The number of <see cref="SubString"/>s copied</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than <c>0</c> or greater than or equal to the length of <paramref name="destination"/></exception>
	/// <exception cref="ArgumentException"><paramref name="destination"/>, starting at <paramref name="offset"/>, is too small to hold all <see cref="SubString"/>s in this <see cref="SubStringCollection"/></exception>
	public readonly int CopyTo(SubString[] destination, int offset)
	{
		if (offset is < 0 || offset >= destination.Length)
		{
			[DoesNotReturn]
			static void failOffsetOutOfRange() => throw new ArgumentOutOfRangeException(nameof(offset), $"The offset into the {nameof(destination)} must be greater or equal to 0 and less than the {nameof(destination)}'s length.");

			failOffsetOutOfRange();
		}

		if (!TryCopyTo(destination.AsSpan(offset), out var subStringsWritten))
		{
			[DoesNotReturn]
			static void failDestinationTooSmall() => throw new ArgumentException($"The {nameof(destination)}, starting at the specified {nameof(offset)}, is too small to contain all substrings.", nameof(destination));

			failDestinationTooSmall();
		}

		return subStringsWritten;
	}

	/// <summary>
	/// Copies the <see cref="SubString"/>s in this <see cref="SubStringCollection"/> to a given destination
	/// </summary>
	/// <param name="destination">The destination span to copy the <see cref="SubString"/>s to</param>
	/// <returns>The number of <see cref="SubString"/>s copied</returns>
	/// <exception cref="ArgumentException"><paramref name="destination"/> is too small to hold all <see cref="SubString"/>s in this <see cref="SubStringCollection"/></exception>
	public readonly int CopyTo(Span<SubString> destination)
	{
		if (!TryCopyTo(destination, out var subStringsWritten))
		{
			[DoesNotReturn]
			static void failDestinationTooSmall() => throw new ArgumentException($"The {nameof(destination)} span is too small to contain all substrings.", nameof(destination));

			failDestinationTooSmall();
		}

		return subStringsWritten;
	}

	/// <summary>
	/// Enumerates the <see cref="SubString"/>s in this <see cref="SubStringCollection"/>
	/// </summary>
	/// <returns>An <see cref="Enumerator"/> that can be used to enumerate the <see cref="SubString"/>s in this <see cref="SubStringCollection"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly Enumerator GetEnumerator() => new(this);

	/// <summary>
	/// Copies the <see cref="SubString"/>s in this <see cref="SubStringCollection"/> to a new array and returns it
	/// </summary>
	/// <returns>A new array containing all <see cref="SubString"/>s in this <see cref="SubStringCollection"/></returns>
	public readonly SubString[] ToArray() => [..this];

	/// <summary>
	/// Tries to copy the <see cref="SubString"/>s in this <see cref="SubStringCollection"/> to a given destination, starting at a specified offset
	/// </summary>
	/// <param name="destination">The destination array to copy the <see cref="SubString"/>s to</param>
	/// <param name="offset">The offset in the destination array at which to start copying</param>
	/// <param name="subStringsWritten">The number of <see cref="SubString"/>s copied; this will be either <c>0</c> if the copy operation failed, or <see cref="Count"/> if all <see cref="SubString"/>s were successfully copied</param>
	/// <returns><c><see langword="true"/></c>, if all <see cref="SubString"/>s were successfully copied; otherwise, <c><see langword="false"/></c></returns>
	public readonly bool TryCopyTo(SubString[] destination, int offset, out int subStringsWritten)
	{
		if (offset is < 0 || offset >= destination.Length)
		{
			subStringsWritten = 0;
			return false;
		}

		return TryCopyTo(destination.AsSpan(offset), out subStringsWritten);
	}

	/// <summary>
	/// Tries to copy the <see cref="SubString"/>s in this <see cref="SubStringCollection"/> to a given destination
	/// </summary>
	/// <param name="destination">The destination span to copy the <see cref="SubString"/>s to</param>
	/// <param name="subStringsWritten">The number of <see cref="SubString"/>s copied; this will be either <c>0</c> if the copy operation failed, or <see cref="Count"/> if all <see cref="SubString"/>s were successfully copied</param>
	/// <returns><c><see langword="true"/></c>, if all <see cref="SubString"/>s were successfully copied; otherwise, <c><see langword="false"/></c></returns>
	public readonly bool TryCopyTo(Span<SubString> destination, out int subStringsWritten)
	{
		unsafe
		{
			subStringsWritten = 0;

			if (destination.Length < Count)
			{
				return false;
			}

			foreach (ref readonly var subString in this)
			{
				destination[subStringsWritten++] = subString;
			}

			return true;
		}
	}
}
