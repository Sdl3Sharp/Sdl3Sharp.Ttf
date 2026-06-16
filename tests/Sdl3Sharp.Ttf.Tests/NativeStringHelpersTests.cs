// DISCLAIMER: This source file was created 100% by AI (GitHub Copilot using GPT-5).

using System;
using Sdl3Sharp.Ttf.Internal;

namespace Sdl3Sharp.Ttf.Tests;

public unsafe sealed class NativeStringHelpersTests
{
	[Fact]
	public void TryGetUtf8OffsetAndLength_WithAsciiRange_ReturnsMatchingOffsetAndLength()
	{
		ReadOnlySpan<byte> utf8Text = "Hello World\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: 1, utf16Length: 5, out var utf8Offset, out var utf8Length);

			Assert.True(success);
			Assert.Equal(1, utf8Offset);
			Assert.Equal(5, utf8Length);
		}
	}

	[Fact]
	public void TryGetUtf8OffsetAndLength_WithMultiByteScalars_ReturnsDifferentUtf8OffsetAndLength()
	{
		ReadOnlySpan<byte> utf8Text = "A€中B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: 1, utf16Length: 2, out var utf8Offset, out var utf8Length);

			Assert.True(success);
			Assert.Equal(1, utf8Offset);
			Assert.Equal(6, utf8Length);
		}
	}

	[Fact]
	public void TryGetUtf8OffsetAndLength_WithNullPointer_ReturnsFalse()
	{
		var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(null, utf16Offset: 0, utf16Length: 1, out var utf8Offset, out var utf8Length);

		Assert.False(success);
		Assert.Equal(0, utf8Offset);
		Assert.Equal(0, utf8Length);
	}

	[Fact]
	public void TryGetUtf8OffsetAndLength_WhenRangeExceedsTextLength_ReturnsFalse()
	{
		ReadOnlySpan<byte> utf8Text = "Hi\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: 3, utf16Length: 0, out _, out _);

			Assert.False(success);
		}
	}

	[Fact]
	public void TryGetUtf8OffsetAndLength_WithZeroOffsetAndLength_ReturnsTrueAndZeroes()
	{
		ReadOnlySpan<byte> utf8Text = "A😂B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: 0, utf16Length: 0, out var utf8Offset, out var utf8Length);

			Assert.True(success);
			Assert.Equal(0, utf8Offset);
			Assert.Equal(0, utf8Length);
		}
	}

	[Theory]
	[InlineData(0, -1)]
	[InlineData(1, -1)]
	[InlineData(-1, -1)]
	public void TryGetUtf8OffsetAndLength_WithNegativeLength_ReturnsFalse(int utf16Offset, int utf16Length)
	{
		ReadOnlySpan<byte> utf8Text = "Hello\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset, utf16Length, out var utf8Offset, out var utf8Length);

			Assert.False(success);
			Assert.Equal(0, utf8Offset);
			Assert.Equal(0, utf8Length);
		}
	}

	[Theory]
	[InlineData(-1, 1, -1, 1)]
	[InlineData(-2, 1, -4, 3)]
	[InlineData(-4, 0, -8, 0)]
	public void TryGetUtf8OffsetAndLength_WithNegativeOffset_ReturnsNegativeUtf8OffsetFromEnd(int utf16Offset, int utf16Length, int expectedUtf8Offset, int expectedUtf8Length)
	{
		ReadOnlySpan<byte> utf8Text = "A€中B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset, utf16Length, out var utf8Offset, out var utf8Length);

			Assert.True(success);
			Assert.Equal(expectedUtf8Offset, utf8Offset);
			Assert.Equal(expectedUtf8Length, utf8Length);
		}
	}

	[Fact]
	public void TryGetUtf8OffsetAndLength_WhenNegativeOffsetExceedsTextLength_ReturnsFalse()
	{
		ReadOnlySpan<byte> utf8Text = "A€中B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: -5, utf16Length: 0, out _, out _);

			Assert.False(success);
		}
	}

	[Fact]
	public void TryGetUtf8OffsetAndLength_WhenNegativeOffsetRangeExceedsTextLength_ReturnsFalse()
	{
		ReadOnlySpan<byte> utf8Text = "A€中B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: -1, utf16Length: 2, out _, out _);

			Assert.False(success);
		}
	}

	[Theory]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.Fail, false, 0, 0)]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.IncludeWholePair, true, -5, 0)]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.ExcludeWholePair, true, -1, 0)]
	public void TryGetUtf8OffsetAndLength_WhenNegativeOffsetSplitsSurrogatePair_AppliesConfiguredBehavior(
		int splittingBehavior,
		bool expectedSuccess,
		int expectedUtf8Offset,
		int expectedUtf8Length)
	{
		ReadOnlySpan<byte> utf8Text = "A😂B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: -2, utf16Length: 0, out var utf8Offset, out var utf8Length, (NativeStringHelpers.SurrogatePairSplittingBehavior)splittingBehavior);

			Assert.Equal(expectedSuccess, success);

			if (success)
			{
				Assert.Equal(expectedUtf8Offset, utf8Offset);
				Assert.Equal(expectedUtf8Length, utf8Length);
			}
		}
	}

	[Fact]
	public void TryGetUtf8OffsetAndLength_WhenNegativeOffsetSplitsSurrogatePairAndLengthIsNonZero_WithExcludeWholePair_ExcludesWholePairFromUtf8Length()
	{
		ReadOnlySpan<byte> utf8Text = "A😂B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(
				utf8Ptr,
				utf16Offset: -2,
				utf16Length: 1,
				out var utf8Offset,
				out var utf8Length,
				NativeStringHelpers.SurrogatePairSplittingBehavior.ExcludeWholePair);

			Assert.True(success);
			Assert.Equal(-1, utf8Offset);
			Assert.Equal(0, utf8Length);
		}
	}

	[Theory]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.Fail, false, 0, 0)]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.ExcludeWholePair, true, -1, 0)]
	public void TryGetUtf8OffsetAndLength_WhenNegativeOffsetAndLengthSplitSurrogatePair_AppliesConfiguredBehavior(
		int splittingBehavior,
		bool expectedSuccess,
		int expectedUtf8Offset,
		int expectedUtf8Length)
	{
		ReadOnlySpan<byte> utf8Text = "A😂B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: -2, utf16Length: 1, out var utf8Offset, out var utf8Length, (NativeStringHelpers.SurrogatePairSplittingBehavior)splittingBehavior);

			Assert.Equal(expectedSuccess, success);

			if (success)
			{
				Assert.Equal(expectedUtf8Offset, utf8Offset);
				Assert.Equal(expectedUtf8Length, utf8Length);
			}
		}
	}

	[Fact]
	public void TryGetUtf8OffsetAndLength_WithOffsetAtUtf16EndAndZeroLength_ReturnsUtf8ByteLengthAsOffset()
	{
		ReadOnlySpan<byte> utf8Text = "A€😂\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: 4, utf16Length: 0, out var utf8Offset, out var utf8Length);

			Assert.True(success);
			Assert.Equal(8, utf8Offset);
			Assert.Equal(0, utf8Length);
		}
	}

	[Fact]
	public void TryGetUtf8OffsetAndLength_WhenOffsetSplitsSurrogatePairAndLengthIsNonZero_WithExcludeWholePair_IncludesWholePairInUtf8Length()
	{
		ReadOnlySpan<byte> utf8Text = "A😂B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(
				utf8Ptr,
				utf16Offset: 2,
				utf16Length: 1,
				out var utf8Offset,
				out var utf8Length,
				NativeStringHelpers.SurrogatePairSplittingBehavior.ExcludeWholePair);

			Assert.True(success);
			Assert.Equal(1, utf8Offset);
			Assert.Equal(4, utf8Length);
		}
	}

	[Theory]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.Fail, false, 0, 0)]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.IncludeWholePair, true, 5, 0)]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.ExcludeWholePair, true, 1, 0)]
	public void TryGetUtf8OffsetAndLength_WhenOffsetSplitsSurrogatePair_AppliesConfiguredBehavior(
		int splittingBehavior,
		bool expectedSuccess,
		int expectedUtf8Offset,
		int expectedUtf8Length)
	{
		ReadOnlySpan<byte> utf8Text = "A😂B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: 2, utf16Length: 0, out var utf8Offset, out var utf8Length, (NativeStringHelpers.SurrogatePairSplittingBehavior)splittingBehavior);

			Assert.Equal(expectedSuccess, success);

			if (success)
			{
				Assert.Equal(expectedUtf8Offset, utf8Offset);
				Assert.Equal(expectedUtf8Length, utf8Length);
			}
		}
	}

	[Theory]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.Fail, false, 0)]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.IncludeWholePair, true, 4)]
	[InlineData((int)NativeStringHelpers.SurrogatePairSplittingBehavior.ExcludeWholePair, true, 0)]
	public void TryGetUtf8OffsetAndLength_WhenLengthSplitsSurrogatePair_AppliesConfiguredBehavior(
		int splittingBehavior,
		bool expectedSuccess,
		int expectedUtf8Length)
	{
		ReadOnlySpan<byte> utf8Text = "A😂B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: 1, utf16Length: 1, out var utf8Offset, out var utf8Length, (NativeStringHelpers.SurrogatePairSplittingBehavior)splittingBehavior);

			Assert.Equal(expectedSuccess, success);

			if (success)
			{
				Assert.Equal(1, utf8Offset);
				Assert.Equal(expectedUtf8Length, utf8Length);
			}
		}
	}

	[Fact]
	public void TryGetUtf8OffsetAndLength_WithUnrecognizedSurrogateBehavior_ReturnsFalse()
	{
		ReadOnlySpan<byte> utf8Text = "A😂B\0"u8;

		fixed (byte* utf8Ptr = utf8Text)
		{
			var success = NativeStringHelpers.TryGetUtf8OffsetAndLength(utf8Ptr, utf16Offset: 2, utf16Length: 0, out _, out _, (NativeStringHelpers.SurrogatePairSplittingBehavior)999);

			Assert.False(success);
		}
	}
}
