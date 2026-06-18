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
	/// Represents a collection of <see cref="DrawOperation"/>s in a <see cref="TextData"/> instance
	/// </summary>
	/// <param name="data">The <see cref="TextData"/> instance that owns the <see cref="DrawOperation"/>s</param>
	[StructLayout(LayoutKind.Sequential)]
	[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly ref struct DrawOperationCollection(TextData data)
	{
		/// <summary>
		/// Enumerates the <see cref="DrawOperation"/>s in a <see cref="DrawOperationCollection"/>
		/// </summary>
		/// <param name="operations">The <see cref="DrawOperationCollection"/> to enumerate</param>
		[StructLayout(LayoutKind.Sequential)]
		[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public ref struct Enumerator(DrawOperationCollection operations) : IEnumerator<DrawOperation>
		{
			private readonly DrawOperationCollection mOperations = operations;
			private int mIndex = -1;

			/// <inheritdoc/>
			/// <inheritdoc cref="Target"/>
			public readonly DrawOperation Current
			{
				get => mOperations[mIndex]; // This will recheck the index bounds, which is good if the enumerator was used incorrectly,
				                            // but could be a bit of a performance hit if the enumerator is used as intended.
											// I think we should prioritize safety over performance here.
			}

			/// <summary>Not supported. Do not access this property, use the <see cref="Current"/> property instead. This property will always throw a <see cref="NotSupportedException"/> if accessed.</summary>
			/// <exception cref="NotSupportedException">Always</exception>
			[Obsolete($"Not supported. Do not access this property, use the {nameof(Current)} property instead. This property will always throw a {nameof(NotSupportedException)} if accessed.")]
			readonly object IEnumerator.Current { [DoesNotReturn] get => throw new NotSupportedException(); }

			/// <inheritdoc/>
			/// <inheritdoc cref="Target"/>
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			public bool MoveNext()
			{
				var index = mIndex + 1;

				if (index < mOperations.Count)
				{
					mIndex = index;
					return true;
				}

				return false;
			}

			/// <inheritdoc/>
			readonly void IDisposable.Dispose() { }

			/// <inheritdoc/>
			public void Reset() => mIndex = -1;
		}

		private readonly TextData mData = data;

		/// <summary>
		/// Gets the number of <see cref="DrawOperation"/>s in this <see cref="DrawOperationCollection"/>
		/// <inheritdoc cref="Target"/>
		/// </summary>
		public readonly int Count { get { unsafe { return mData.Target->NumOps; } } }

		/// <summary>
		/// Gets the <see cref="DrawOperation"/> at a specified index in this <see cref="DrawOperationCollection"/>
		/// </summary>
		/// <param name="index">The index of the <see cref="DrawOperation"/> to get</param>
		/// <returns>The <see cref="DrawOperation"/> at the specified <paramref name="index"/></returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c> or greater than or equal to <see cref="Count"/></exception>
		/// <inheritdoc cref="Target"/>
		public readonly DrawOperation this[int index]
		{
			get
			{
				unsafe
				{
					var target = mData.Target;

					if (index is < 0 || index >= target->NumOps)
					{
						[DoesNotReturn]
						static void failIndexArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(index), $"The {nameof(index)} must be greater or equal to 0 and less than the number of draw operations in the {nameof(TextData)}");

						failIndexArgumentOutOfRange();
					}

					return new(ref Unsafe.AsRef<DrawOperation.TTF_DrawOperation>(target->Ops + index));
				}
			}
		}

		/// <summary>
		/// Enumerates the <see cref="DrawOperation"/>s in this <see cref="DrawOperationCollection"/>
		/// </summary>
		/// <returns>An <see cref="Enumerator"/> that can be used to enumerate the <see cref="DrawOperation"/>s in this <see cref="DrawOperationCollection"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly Enumerator GetEnumerator() => new(this);
	}
}
