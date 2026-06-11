using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;

namespace Sdl3Sharp.Ttf.Internal;

internal static class NativeStringHelpers
{
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

		if (utf8Text is null || utf16Offset is < 0 || utf16Length is < 0)
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
		var utf8TextSpan = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(utf8Text);

		while (!utf8TextSpan.IsEmpty)
		{
			if (Rune.DecodeFromUtf8(utf8TextSpan, out var rune, out var bytesConsumed) is not OperationStatus.Done)
			{
				// Rune.DecodeFromUtf8 only has two failure modes:
				// - OperationStatus.InvalidData: The UTF-8 sequence is malformed. We can't really recover from this.
				// - OperationStatus.NeedMoreData: This is just as bad, because, since we always input at most 4 bytes (only less if we encountered a null-terminator)
				//   and the sequence therefore must encompass at least a whole Unicode scalar, it must mean one of two things:
				//   - We messed up and our input sequence doesn't actually start at a UTF-8 code point boundary. This means the code is incorrect at some place and we can't hold the invariant that the input always starts at such a boundary.
				//   - The UTF-8 sequence is malformed in the sense that it encodes a standalone surrogate character, which it not being in a pair with another surrogate character means it's an invalid Unicode scalar.
				//   Either way, again, we can't really recover from this.

				return false;
			}

			if (utf16CountedOffset < utf16Offset)
			{
				// We're currently counting the UTF-8 offset

				utf16CountedOffset += rune.Utf16SequenceLength;

				if (utf16CountedOffset < utf16Offset)
				{
					// We can safely just add the full length of consumed bytes.
					utf8Offset += bytesConsumed;
				}
				else if (utf16CountedOffset == utf16Offset)
				{
					// Not only can we safely just add the full length of consumed bytes, ...
					utf8Offset += bytesConsumed;

					// ... but we can also already early return with success here, if the given UTF-16 length is 0, because we have nothing more left to do after this.
					if (utf16Length is 0)
					{
						return true;
					}
				}
				else
				{
					// This means the given UTF-16 offset falls in the middle of a surrogate pair, and rune.Utf16SequenceLength must be 2.

					// First, we need to adjust the counted UTF-16 offset and length.
					utf16CountedOffset--; // We exclude the second code unit of the surrogate pair from the counted UTF-16 offset...
					utf16CountedLength++; // ... and we include it in the counted UTF-16 length.

					// Then we have to handle the issue of splitting an Unicode scalar according to the given surrogate pair splitting behavior:
					// - SurrogatePairSplittingBehavior.Fail -> We fail and return false.
					// - SurrogatePairSplittingBehavior.IncludeWholePair -> We include the whole surrogate pair in the resulting UTF-8 offset. Because of that we exclude it from the resulting UTF-8 length. In short: offset includes it, length excludes it.
					// - SurrogatePairSplittingBehavior.ExcludeWholePair -> We exclude the whole surrogate pair from the resulting UTF-8 offset. If we need to count the UTF-8 length (the given UTF-16 length is not 0), we include the whole pair (as byte length) as the first UTF-8 scalar in the resulting UTF-8 length. In short: offset excludes it, length includes it.

					switch (surrogatePairSplittingBehavior)
					{
						case SurrogatePairSplittingBehavior.IncludeWholePair:
							utf8Offset += bytesConsumed;

							// Like mentioned before, we can early return with success here, if there's nothing more left to do for us.
							if (utf16Length is 0)
							{
								return true;
							}
							break;

						case SurrogatePairSplittingBehavior.ExcludeWholePair:
							if (utf16Length is not 0)
							{
								utf8Length += bytesConsumed;
							}
							else
							{
								// Again, the same reasoning for returning early with success, if there's nothing more left to do.
								// (The only reason we don't merge the two early return branches into one after the switch is that in this particular switch case,
								// we already have to check for the UTF-16 length being 0, and for optimization reasons, we don't want to double-check.)
								return true;
							}
							break;

						default: // SurrogatePairSplittingBehavior.Fail or any unrecognized value
							return false;
					}
				}
			}
			else if (utf16CountedLength < utf16Length)
			{
				// We're currently counting the UTF-8 length

				utf16CountedLength += rune.Utf16SequenceLength;

				if (utf16CountedLength < utf16Length)
				{
					// We can safely just add the full length of consumed bytes.
					utf8Length += bytesConsumed;
				}
				else if (utf16CountedLength == utf16Length)
				{
					// Not only can we safely just add the full length of consumed bytes, ...
					utf8Length += bytesConsumed;

					// ... but we can also already early return with success here.
					return true;
				}
				else
				{
					// This means the given UTF-16 length (and offset) falls in the middle of a surrogate pair, and rune.Utf16SequenceLength must be 2.

					// This time we don't need to adjust anything, as we can return from the method in all cases after handling the surrogate pair splitting issue.
					// For that, we have to handle the issue of splitting an Unicode scalar according to the given surrogate pair splitting behavior:
					// - SurrogatePairSplittingBehavior.Fail -> We fail and return false.
					// - SurrogatePairSplittingBehavior.IncludeWholePair -> We include the whole surrogate pair in the resulting UTF-8 length. Then we can already return with success.
					// - SurrogatePairSplittingBehavior.ExcludeWholePair -> We exclude the whole surrogate pair from the resulting UTF-8 length. Then we can already return with success.

					switch (surrogatePairSplittingBehavior)
					{
						case SurrogatePairSplittingBehavior.IncludeWholePair:
							utf8Length += bytesConsumed;
							break;

						// We don't do anything in the ExcludeWholePair case, so we can just skip it.
						//case SurrogatePairSplittingBehavior.ExcludeWholePair:
						//	break;

						default: // SurrogatePairSplittingBehavior.Fail or any unrecognized value
							return false;
					}

					return true;
				}
			}
			else
			{
				// Well, if that's the case, we're already done and we can just return the results.
				// Just as a side node, being at this point implies that the given UTF-16 length was 0,
				// and therefore the resulting UTF-8 length should be 0 as well.
				return true;
			}

			utf8TextSpan = utf8TextSpan[bytesConsumed..];
		}

		// This means we've reached the end of the given UTF-8 text without successfully counting the given UTF-16 offset and length,
		// which means the UTF-8 text must have been too short to encompass the total of the UTF-16 offset and length.
		// For us, this is a failure case.

		return false;
	}
}
