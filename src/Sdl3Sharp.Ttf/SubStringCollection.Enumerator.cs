using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

partial struct SubStringCollection
{
	/// <summary>
	/// Enumerates the <see cref="SubString"/>s in a <see cref="SubStringCollection"/>
	/// </summary>
	/// <param name="subStrings">The <see cref="SubStringCollection"/> to enumerate</param>
	[StructLayout(LayoutKind.Sequential)]
	[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public ref struct Enumerator(SubStringCollection subStrings) : IEnumerator<SubString>
	{
		private readonly SubStringCollection mSubStrings = subStrings;
		private int mIndex = -1;

		/// <inheritdoc cref="IEnumerator{SubString}.Current"/>
		public readonly ref readonly SubString Current
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => ref mSubStrings[mIndex]; // This will recheck the index bounds, which is good if the enumerator was used incorrectly,
											// but could be a bit of a performance hit if the enumerator is used as intended.
											// I think we should prioritize safety over performance here.
		}

		/// <inheritdoc/>
		readonly SubString IEnumerator<SubString>.Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Current; }

		/// <inheritdoc/>
		readonly object IEnumerator.Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Current; }

		/// <inheritdoc/>
		public bool MoveNext()
		{
			var index = mIndex + 1;

			if (index < mSubStrings.Count)
			{
				mIndex = index;
				return true;
			}

			return false;
		}

		/// <inheritdoc/>
		public readonly void Dispose() { }

		/// <inheritdoc/>
		public void Reset() => mIndex = -1;
	}
}
