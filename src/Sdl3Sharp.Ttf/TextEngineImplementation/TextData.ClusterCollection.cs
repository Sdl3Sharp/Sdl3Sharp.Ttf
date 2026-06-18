using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

partial class TextData
{
	/// <summary>
	/// Represents a collection of clusters of glyphs, represented as <see cref="SubString"/>s, in a <see cref="TextData"/> instance
	/// </summary>
	/// <param name="data">The <see cref="TextData"/> instance that owns the clusters of glyphs represented by this collection</param>
	[StructLayout(LayoutKind.Sequential)]
	[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly ref struct ClusterCollection(TextData data)
	{
		/// <summary>
		/// Enumerates the clusters of glyphs, represented as <see cref="SubString"/>s, in a <see cref="ClusterCollection"/>
		/// </summary>
		/// <param name="clusters">The <see cref="ClusterCollection"/> to enumerate</param>
		[StructLayout(LayoutKind.Sequential)]
		[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public ref struct Enumerator(ClusterCollection clusters) : IEnumerator<SubString>
		{
			private readonly ClusterCollection mClusters = clusters;
			private int mIndex = -1;

			/// <inheritdoc cref="IEnumerator{SubString}.Current"/>
			/// <inheritdoc cref="Target"/>
			public readonly ref readonly SubString Current
			{
				get => ref mClusters[mIndex]; // This will recheck the index bounds, which is good if the enumerator was used incorrectly,
											  // but could be a bit of a performance hit if the enumerator is used as intended.
											  // I think we should prioritize safety over performance here.
			}

			/// <inheritdoc/>
			/// <inheritdoc cref="Target"/>
			readonly SubString IEnumerator<SubString>.Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Current; }

			/// <inheritdoc/>
			/// <inheritdoc cref="Target"/>
			readonly object IEnumerator.Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Current; }

			/// <inheritdoc/>
			/// <inheritdoc cref="Target"/>
			public bool MoveNext()
			{
				unsafe
				{
					var index = mIndex + 1;

					if (index < mClusters.Count)
					{
						mIndex = index;
						return true;
					}

					return false;
				}
			}

			/// <inheritdoc/>
			readonly void IDisposable.Dispose() { }

			/// <inheritdoc/>
			public void Reset() => mIndex = -1;
		}

		private readonly TextData mData = data;

		/// <summary>
		/// Gets the number of clusters of glyphs, represented as <see cref="SubString"/>s, in this <see cref="ClusterCollection"/>
		/// </summary>
		/// <inheritdoc cref="Target"/>
		public readonly int Count { get { unsafe { return mData.Target->NumClusters; } } }

		/// <summary>
		/// Gets a reference to the cluster of glyphs, represented as a <see cref="SubString"/>, at the specified <paramref name="index"/> in this <see cref="ClusterCollection"/>
		/// </summary>
		/// <param name="index">The index of the cluster to get a reference to</param>
		/// <returns>A reference to the <see cref="SubString"/> representing the cluster of glyphs at the specified <paramref name="index"/></returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c> or greater than or equal to <see cref="Count"/></exception>
		/// <inheritdoc cref="Target"/>
		public readonly ref readonly SubString this[int index]
		{
			get
			{
				unsafe
				{
					var target = mData.Target;

					if (index is < 0 || index >= target->NumClusters)
					{
						[DoesNotReturn]
						static void failIndexArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(index), $"The {nameof(index)} must be greater or equal to 0 and less than the number of clusters in the {nameof(TextData)}");

						failIndexArgumentOutOfRange();
					}

					return ref Unsafe.AsRef<SubString>(target->Clusters + index);
				}
			}
		}

		/// <summary>
		/// Enumerates the clusters of glyphs, represented as <see cref="SubString"/>s, in this <see cref="ClusterCollection"/>
		/// </summary>
		/// <returns>An <see cref="Enumerator"/> that can be used to enumerate the clusters of glyphs, represented as <see cref="SubString"/>s, in this <see cref="ClusterCollection"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly Enumerator GetEnumerator() => new(this);
	}
}
