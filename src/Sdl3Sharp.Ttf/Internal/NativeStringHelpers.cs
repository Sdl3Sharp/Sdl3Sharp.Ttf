using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Sdl3Sharp.Ttf.Internal;

internal static partial class NativeStringHelpers
{
	public static ReadOnlySpan<byte> EmptyNullTerminatedUtf8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => "\0"u8; }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static ReadOnlySpan<byte> NullTerminateUtf8IfEmpty(ReadOnlySpan<byte> utf8Bytes) => utf8Bytes.IsEmpty
		? EmptyNullTerminatedUtf8
		: utf8Bytes;

	public enum SurrogatePairSplittingBehavior
	{
		Fail,
		IncludeWholePair,
		ExcludeWholePair,
	}

	public unsafe static bool TryGetUtf8OffsetAndLength(byte* utf8Text, int utf16Offset, int utf16Length, out int utf8Offset, out int utf8Length, SurrogatePairSplittingBehavior surrogatePairSplittingBehavior = SurrogatePairSplittingBehavior.Fail)
	{
		utf8Offset = 0;
		utf8Length = 0;

		if (utf8Text is null || utf16Length is < 0)
		{
			return false;
		}

		if (utf16Offset is 0 && utf16Length is 0)
		{
			// There's nothing to do for us. The resulting UTF-8 offset and length have to be 0 in this case.
			return true;
		}

		var utf16CountedOffset = 0;
		var utf16CountedLength = 0;

		// It's better to scan the UTF-8 for a null-terminator beforehand with the optimized methods the runtime provides,
		// instead of trying to do it ourselves, actively while simultaneously decoding the UTF-8 text, and most detrimentally, doing it byte-wise.
		// Even though we have to double-scan the whole text, it should be still better to rely on the optimized vectorization the runtime offers for that.
		// Also, it makes the following loop code so much cleaner and simpler.
		// Additionally, with the newly added functionality to his method (counting negative offsets from the end of the text),
		// we're actually required to know the whole text (length) in advance.
		var utf8TextSpan = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(utf8Text);

		if (utf16Offset is not < 0)
		{
			// This is the "normal" case, where we count the offsets positively from the start of the text.

			while (true)
			{
				if (utf8TextSpan.IsEmpty)
				{
					// This means we've reached the end of the given UTF-8 text without successfully counting the given UTF-16 offset,
					// which means the UTF-8 text must have been too short to encompass the total of the UTF-16 offset and length.
					// For us, this is a failure case.

					return false;
				}

				if (Rune.DecodeFromUtf8(utf8TextSpan, out var rune, out var bytesConsumed) is not OperationStatus.Done)
				{
					// Rune.DecodeFromUtf8 only has two failure modes:
					// - OperationStatus.InvalidData: The UTF-8 sequence is malformed. We can't really recover from this.
					// - OperationStatus.NeedMoreData: This is just as bad, because it can only mean one of two things:
					//   - We messed up and our input sequence doesn't actually start at a UTF-8 code point boundary.
					//   - The UTF-8 sequence is malformed in the sense that it encodes a standalone surrogate character, which it not being in a pair with another surrogate character means it's an invalid Unicode scalar.
					//   Either way, again, we can't really recover from this.

					return false;
				}

				// We're currently counting the UTF-8 offset
				utf16CountedOffset += rune.Utf16SequenceLength;
				utf8TextSpan = utf8TextSpan[bytesConsumed..];

				if (utf16CountedOffset > utf16Offset)
				{
					// This means the given UTF-16 offset falls in the middle of a surrogate pair, and rune.Utf16SequenceLength must be 2.

					// First, we need to adjust the counted UTF-16 offset and length.
					utf16CountedOffset--; // We exclude the second code unit of the surrogate pair from the counted UTF-16 offset...
					utf16CountedLength++; // ... and we include it in the counted UTF-16 length.

					// Then we have to handle the issue of splitting an Unicode scalar according to the given surrogate pair splitting behavior:
					// - SurrogatePairSplittingBehavior.Fail -> We fail and return false.
					// - SurrogatePairSplittingBehavior.IncludeWholePair -> We include the whole surrogate pair in the resulting UTF-8 offset. Because of that we exclude it from the resulting UTF-8 length. In short: offset includes it, length excludes it.
					// - SurrogatePairSplittingBehavior.ExcludeWholePair -> We exclude the whole surrogate pair from the resulting UTF-8 offset. If we need to count the UTF-8 length (the given UTF-16 length is not 0), we include the whole pair (as byte length) as the first UTF-8 scalar in the resulting UTF-8 length. In short: offset excludes it, length includes it.

					if (surrogatePairSplittingBehavior is SurrogatePairSplittingBehavior.ExcludeWholePair)
					{
						// We exclude the whole surrogate pair by simply skipping adding to the UTF-8 offset, and instead adding to the UTF-8 length if necessary.
						if (utf16Length is not 0)
						{
							utf8Length += bytesConsumed;
						}
						else
						{
							// There's nothing more to do for us. We can early return here.
							return true;
						}

						// Then we can break out of the loop and proceed with counting the UTF-8 length.
						break;
					}
					else if (surrogatePairSplittingBehavior is not SurrogatePairSplittingBehavior.IncludeWholePair) // SurrogatePairSplittingBehavior.Fail or any unrecognized value
					{
						return false;
					}

					// Note that every branch of the preceding if-else block either breaks out of the loop or returns.
					// Thus, we handle the missing IncludeWholePair case by simply falling through and doing nothing extra,
					// which is correct, as we add to the UTF-8 offset and because the previously adjusted utf16CountedOffset must be equal to utf16Offset at this point,
					// we will exit the loop right after this iteration.
				}

				// We can safely just add the full length of consumed bytes.
				utf8Offset += bytesConsumed;

				if (utf16CountedOffset == utf16Offset)
				{
					// Note that this is an if-condition instead of a loop exit condition,
					// so we can factor in the early return case for when the given UTF-16 length is 0,
					// instead of doing that check right after the loop.
					// That's because the only other point in the code that breaks out of the loop already performs this check,
					// and we don't want to unnecessarily double-check.

					if (utf16Length is 0)
					{
						// There's nothing more to do for us.
						return true;
					}

					// We can break out of the loop and proceed with counting the UTF-8 length.
					break;
				}
			}
		}
		else
		{
			// This is the case where we were given a negative UTF-16 offset, i.e., its absolute value gives the distance in UTF-16 characters from the end of the text.
			// Thus, we count the UTF-8 offset negatively from the end of the text as well.

			// Have a copy of the span for the purpose of downwards counting.
			var utf8TextDownwardsSpan = utf8TextSpan;

			while (true)
			{
				if (utf8TextDownwardsSpan.IsEmpty)
				{
					// This means we've reached the start of the given UTF-8 text without successfully counting the given UTF-16 offset,
					// which means the UTF-8 text must have been too short to encompass the total of the UTF-16 offset and length.
					// For us, this is a failure case.

					return false;
				}

				if (Rune.DecodeLastFromUtf8(utf8TextDownwardsSpan, out var rune, out var bytesConsumed) is not OperationStatus.Done)
				{
					// Rune.DecodeFromUtf8 only has two failure modes:
					// - OperationStatus.InvalidData: The UTF-8 sequence is malformed. We can't really recover from this.
					// - OperationStatus.NeedMoreData: This is just as bad, because it can only mean one of two things:
					//   - We messed up and our input sequence doesn't actually end at the end of an UTF-8 code point boundary.
					//   - The UTF-8 sequence is malformed in the sense that it encodes a standalone surrogate character, which it not being in a pair with another surrogate character means it's an invalid Unicode scalar.
					//   Either way, again, we can't really recover from this.

					return false;
				}

				// We're currently counting the UTF-8 offset
				utf16CountedOffset -= rune.Utf16SequenceLength;

				if (utf16CountedOffset < utf16Offset)
				{
					// This means the given UTF-16 offset falls in the middle of a surrogate pair, and rune.Utf16SequenceLength must be 2.

					// First, we need to adjust the counted UTF-16 offset and length.
					utf16CountedOffset++; // We exclude the second code unit of the surrogate pair from the counted UTF-16 offset (in this case we're doing that by addition)...
					utf16CountedLength++; // ... and we include it in the counted UTF-16 length (that's always just addition).

					// Then we have to handle the issue of splitting an Unicode scalar according to the given surrogate pair splitting behavior:
					// - SurrogatePairSplittingBehavior.Fail -> We fail and return false.
					// - SurrogatePairSplittingBehavior.IncludeWholePair -> We include the whole surrogate pair in the resulting UTF-8 offset. Because we're downwards counting the UTF-8 offset, this means we must include it the resulting UTF-8 length as well. In short: offset includes it, length includes it.
					// - SurrogatePairSplittingBehavior.ExcludeWholePair -> We exclude the whole surrogate pair from the resulting UTF-8 offset. If we need to count the UTF-8 length (the given UTF-16 length is not 0), we must exclude the whole pair from the resulting UTF-8 length as well. In short: offset excludes it, length excludes it.

					if (surrogatePairSplittingBehavior is SurrogatePairSplittingBehavior.ExcludeWholePair)
					{ 
						// We exclude the whole surrogate pair by doint nothing special and simply breaking out of the loop and proceeding with counting the UTF-8 length.
						// Note that in this case (i.e., the downwards counting case), the statement that modifies the text span for the next iteration happens after this if-block,
						// so, since we break out here, the text span will the correct inverse for the following UTF-8 length counting loop for the ExcludeWholePair case.
						if (utf16Length is 0)
						{
							// There's nothing more to do for us. We can early return here.
							return true;
						}

						// Then we can break out of the loop and proceed with counting the UTF-8 length.
						break;
					}
					else if (surrogatePairSplittingBehavior is not SurrogatePairSplittingBehavior.IncludeWholePair) // SurrogatePairSplittingBehavior.Fail or any unrecognized value
					{
						return false;
					}

					// Note that every branch of the preceding if-else block either breaks out of the loop or returns.
					// Thus, we handle the missing IncludeWholePair case by simply falling through and doing nothing extra,
					// which is correct, as we subtract from the UTF-8 offset and adjust the text span, and because the previously adjusted utf16CountedOffset must be equal to utf16Offset at this point,
					// we will exit the loop right after this iteration.
					// Also, since we're downwards counting and this point represents the IncludeWholePair case, after the modification of the text span,
					// it will be the correct inverse for the following UTF-8 length counting loop.
				}

				// We can safely just subtract the full length of consumed bytes and adjust the text span for the next iteration.
				utf8TextDownwardsSpan = utf8TextDownwardsSpan[..^bytesConsumed];
				utf8Offset -= bytesConsumed;

				if (utf16CountedOffset == utf16Offset)
				{
					// Note that this is an if-condition instead of a loop exit condition,
					// so we can factor in the early return case for when the given UTF-16 length is 0,
					// instead of doing that check right after the loop.
					// That's because the only other point in the code that breaks out of the loop already performs this check,
					// and we don't want to unnecessarily double-check.

					if (utf16Length is 0)
					{
						// There's nothing more to do for us.
						return true;
					}

					// We can break out of the loop and proceed with counting the UTF-8 length.
					break;
				}
			}

			// The temporary utf8TextDownwardsSpan is the inverse (with respect to the original text span) of the actual text span/ with which we will be counting the UTF-8 length in the following loop,
			// so we need to adjust the original utf8TextSpan accordingly.
			utf8TextSpan = utf8TextSpan[utf8TextDownwardsSpan.Length..];
		}

		// The UTF-8 length will always be counted positively from the offset towards the end of the text,
		// regardless of the direction we counted the UTF-8 offset before.

		while (true)
		{
			if (utf8TextSpan.IsEmpty)
			{
				// This means we've reached the end of the given UTF-8 text without successfully counting the given UTF-16 length,
				// which means the UTF-8 text must have been too short to encompass the total of the UTF-16 offset and length.
				// For us, this is a failure case.

				return false;
			}

			if (Rune.DecodeFromUtf8(utf8TextSpan, out var rune, out var bytesConsumed) is not OperationStatus.Done)
			{
				// Rune.DecodeFromUtf8 only has two failure modes:
				// - OperationStatus.InvalidData: The UTF-8 sequence is malformed. We can't really recover from this.
				// - OperationStatus.NeedMoreData: This is just as bad, because it can only mean one of two things:
				//   - We messed up and our input sequence doesn't actually start at a UTF-8 code point boundary.
				//   - The UTF-8 sequence is malformed in the sense that it encodes a standalone surrogate character, which it not being in a pair with another surrogate character means it's an invalid Unicode scalar.
				//   Either way, again, we can't really recover from this.

				return false;
			}

			// We're currently counting the UTF-8 length
			utf16CountedLength += rune.Utf16SequenceLength;
			utf8TextSpan = utf8TextSpan[bytesConsumed..];

			if (utf16CountedLength > utf16Length)
			{
				// This means the given UTF-16 length (and offset) falls in the middle of a surrogate pair, and rune.Utf16SequenceLength must be 2.

				// Because of the loops "exit condition" (i.e., the if-condition right at the end of the loop) and for the sake of cleanliness, we adjust the counted UTF-16 length in the same way as we did before.
				utf16CountedLength--;

				// This time we don't need to adjust anything, as we can return from the method in all cases after handling the surrogate pair splitting issue.
				// For that, we have to handle the issue of splitting an Unicode scalar according to the given surrogate pair splitting behavior:
				// - SurrogatePairSplittingBehavior.Fail -> We fail and return false.
				// - SurrogatePairSplittingBehavior.IncludeWholePair -> We include the whole surrogate pair in the resulting UTF-8 length. Then we can already return with success.
				// - SurrogatePairSplittingBehavior.ExcludeWholePair -> We exclude the whole surrogate pair from the resulting UTF-8 length. Then we can already return with success.

				if (surrogatePairSplittingBehavior is SurrogatePairSplittingBehavior.ExcludeWholePair)
				{
					// We can just exclude the whole surrogate pair by simply doing nothing and breaking out of the loop to the return with success right after it.
					break;
				}
				else if (surrogatePairSplittingBehavior is not SurrogatePairSplittingBehavior.IncludeWholePair) // SurrogatePairSplittingBehavior.Fail or any unrecognized value
				{
					return false;
				}

				// Note that every branch of the preceding if-else block either breaks out of the loop or returns.
				// Thus, we handle the missing IncludeWholePair case by simply falling through and doing nothing extra,
				// which is correct, as we add to the UTF-8 length and because the previously adjusted utf16CountedLength must be equal to utf16Length at this point,
				// we will exit the loop right after this iteration.
			}

			// We can safely just add the full length of consumed bytes.
			utf8Length += bytesConsumed;

			if (utf16CountedLength == utf16Length)
			{
				// Note that this is an if-condition instead of a loop exit condition,
				// to be consistent with the way we've done it in the UTF-8 offset counting loop,
				// where it was necessary to do it in this way.
				// But of course, this whole loop could be just a `do { ... } while (utf16CountedLength != utf16Length)` instead.

				// We're done.
				break;
			}
		}

		// Add this point we're surely done and everything must be correct, so we can return with success.
		return true;
	}
}
