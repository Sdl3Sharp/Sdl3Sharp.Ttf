using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

partial struct GpuAtlasDrawSequenceEnumerable
{
	/// <summary>
	/// Enumerates the <see cref="GpuAtlasDrawSequence"/>s in a <see cref="GpuAtlasDrawSequenceEnumerable"/>
	/// </summary>
	/// <param name="sequences">The <see cref="GpuAtlasDrawSequenceEnumerable"/> to enumerate the <see cref="GpuAtlasDrawSequence"/>s of</param>
	[StructLayout(LayoutKind.Sequential)]
	[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public ref struct Enumerator(GpuAtlasDrawSequenceEnumerable sequences) : IEnumerator<GpuAtlasDrawSequence>
	{
		private unsafe GpuAtlasDrawSequence mCurrent;
		private unsafe GpuAtlasDrawSequence.TTF_GPUAtlasDrawSequence* mNext = sequences.mSequences;

		/// <inheritdoc/>
		public readonly GpuAtlasDrawSequence Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mCurrent; }

		/// <summary>Not supported. Do not access this property, use the <see cref="Current"/> property instead. This property will always throw a <see cref="NotSupportedException"/> if accessed.</summary>
		/// <exception cref="NotSupportedException">Always</exception>
		[Obsolete($"Not supported. Do not access this property, use the {nameof(Current)} property instead. This property will always throw a {nameof(NotSupportedException)} if accessed.")]
		readonly object IEnumerator.Current { [DoesNotReturn] get => throw new NotSupportedException(); }

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool MoveNext()
		{
			unsafe
			{
				if (mNext is null)
				{
					return false;
				}

				mCurrent = new(ref Unsafe.AsRef<GpuAtlasDrawSequence.TTF_GPUAtlasDrawSequence>(mNext));
				mNext = mNext->Next;

				return true;
			}
		}

		/// <inheritdoc/>
		readonly void IDisposable.Dispose() { }

		/// <summary>Not supported. Do not use this method. This method will always throw a <see cref="NotSupportedException"/> if called.</summary>
		/// <exception cref="NotSupportedException">Always</exception>
		[Obsolete($"Not supported. Do not use this method. This method will always throw a {nameof(NotSupportedException)} if called.")]
		[DoesNotReturn]
		readonly void IEnumerator.Reset() => throw new NotSupportedException();
	}
}
